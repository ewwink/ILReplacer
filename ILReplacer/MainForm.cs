using dnlib.DotNet;
using dnlib.DotNet.Emit;
using dnlib.DotNet.Writer;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace ILReplacer
{
    public partial class MainForm : Form
    {
        #region Variable

        public bool IsRunning;
        public bool PreserveAllFlags;
        public bool ShowLogs = true;
        public int EditedMethodsCount;
        public int ReplacedBlocks;
        public List<List<Instruction>> BlocksFind = new List<List<Instruction>>();
        public List<List<Instruction>> BlocksReplace = new List<List<Instruction>>();
        public ModuleDefMD Module;
        public string InputFindText = "";
        public string InputReplaceText = "";
        public string LogInfo = "";
        public string ModuleName = "";

        #endregion

        #region Form
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Text = $@"ILReplacer v{Application.ProductVersion} | by ewwink";
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            txtFile.Size = new Size(Size.Width - 214, txtFile.Size.Height);
        }

        #endregion

        #region File

        private void txtFile_DragDrop(object sender, DragEventArgs e)
        {
            var fileList = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            if (File.Exists(fileList[0]))
                txtFile.Text = fileList[0];
        }

        private void txtFile_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.Copy : DragDropEffects.None;
        }
        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog
            {
                Title = @"Select File",
                Filter = @"Executables files|*.exe;*.dll|All files (*.*)|*.*"
            };

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                txtFile.Text = ofd.FileName;
            }
        }

        #endregion

        #region Textbox

        private void txtInputFind_TextChanged(object sender, EventArgs e)
        {
            var blockCount = Regex.Split(txtInputFind.Text, @"^={3,}", RegexOptions.Multiline).Length;
            lblFindBlocks.Text = @"Find Blocks: " + blockCount;
        }

        private void txtInputReplace_TextChanged(object sender, EventArgs e)
        {
            var blockCount = Regex.Split(txtInputReplace.Text, @"^={3,}", RegexOptions.Multiline).Length;
            lblReplaceBlocks.Text = @"Replace With Blocks: " + blockCount;
        }

        private void SelectAll(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
            {
                ((TextBox)sender)?.SelectAll();
            }
        }

        #endregion

        #region Checkbox

        private void cboxForceSave_CheckedChanged(object sender, EventArgs e)
        {
            PreserveAllFlags = cboxFlagAll.Checked;
        }
        private void cboxShowLog_CheckedChanged(object sender, EventArgs e)
        {
            ShowLogs = cboxShowLog.Checked;
        }

        #endregion

        #region Menu

        private void MenuExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void menuSaveBLocks_Click(object sender, EventArgs e)
        {
            var sfd = new SaveFileDialog
            {
                Title = @"Save Blocks File",
                Filter = @"ILReplace files|*.ilr|Text files|*.txt|All files (*.*)|*.*"
            };

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                var blocksText = txtInputFind.Text.Trim() + "\r\n###########\r\n" + txtInputReplace.Text.Trim();
                File.WriteAllText(sfd.FileName, blocksText);
                WriteStatus("Blocks Saved to: " + sfd.FileName);
            }
        }

        private void menuLoadBLocks_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog
            {
                Title = @"Select Blocks File",
                Filter = @"ILReplace files|*.ilr|Text files|*.txt|All files (*.*)|*.*"
            };

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                var blocksText = File.ReadAllText(ofd.FileName)
                    .Split(new[] { "###########" }, StringSplitOptions.None);
                if (blocksText.Length > 1)
                {
                    txtInputFind.Text = blocksText[0].Trim();
                    txtInputReplace.Text = blocksText[1].Trim();
                    WriteStatus("Blocks loaded from: " + ofd.FileName);
                }
                else
                {
                    WriteStatus("Blocks file empty or invalid: " + ofd.FileName);
                }
            }
        }

        #endregion

        #region Replace

        private void btnReplace_Click(object sender, EventArgs e)
        {
            if (IsRunning)
                return;

            ModuleName = txtFile.Text.Trim();
            if (!File.Exists(ModuleName))
            {
                MessageBox.Show(@"File not Defined or not Exist: " + ModuleName, @"Warning!",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (txtInputFind.Text.Trim() == "" || txtInputReplace.Text.Trim() == "")
            {
                MessageBox.Show(@"instructions to Find or Replace is empty!", @"Warning!",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            LogInfo = "";
            IsRunning = true;
            lblStatus.Text = @"Running..";
            InputFindText = txtInputFind.Text;
            InputReplaceText = txtInputReplace.Text;
            ReplacedBlocks = 0;
            EditedMethodsCount = 0;
            EnableControl(false);
            new Thread(PrepareReplace)
            {
                IsBackground = true
            }.Start();
        }

        private void EnableControl(bool isEnable)
        {
            Invoke((MethodInvoker)delegate
            {
                txtFile.Enabled = txtInputFind.Enabled = txtInputReplace.Enabled = isEnable;
            });
        }

        private void PrepareReplace()
        {
            var allFlags = MetadataFlags.PreserveAll | MetadataFlags.KeepOldMaxStack
                | MetadataFlags.AlwaysCreateGuidHeap | MetadataFlags.RoslynSortInterfaceImpl;

            try
            {
                var isBlockGood = CheckBlocks();
                if (!isBlockGood)
                    return;

                if (!Module.IsILOnly)
                {
                    WriteStatus("Info: The Assembly maybe contains unmanaged code");
                    Thread.Sleep(2000);
                }

                WriteStatus("Processing: " + ModuleName);

                ReplaceInstructions();

                var newName = $"{Path.GetFileNameWithoutExtension(ModuleName)}_ILR{Path.GetExtension(ModuleName)}";

                if (ReplacedBlocks > 0)
                {
                    if (Module.IsILOnly)
                    {
                        var managedOptions = new ModuleWriterOptions(Module);
                        if (PreserveAllFlags)
                            managedOptions.MetadataOptions.Flags = allFlags;

                        Module.Write(Path.GetDirectoryName(ModuleName) + "\\" + newName, managedOptions);
                    }
                    else
                    {
                        var unmanagedOptions = new NativeModuleWriterOptions(Module, true);
                        if (PreserveAllFlags)
                            unmanagedOptions.MetadataOptions.Flags = allFlags;

                        Module.NativeWrite(newName, unmanagedOptions);
                    }

                    if (ShowLogs)
                    {
                        WriteStatus(
                            $"Done: Saved as {newName} || Replaced {ReplacedBlocks} Blocks Instructions in {EditedMethodsCount} Methods");
                        ShowFormLog(LogInfo);
                    }
                }
                else
                    WriteStatus("Info: No Block Instructions Replaced!");

                //MessageBox.Show("File Saved as:\r\n" + newName, "Replaces Done",
                //        MessageBoxButtons.OK, MessageBoxIcon.Information);

                Module?.Dispose();

                EnableControl(true);
                IsRunning = false;
            }
            catch (Exception ex)
            {
                Module?.Dispose();

                IsRunning = false;
                if (ex.Message.Contains("Error calculating max stack"))
                    WriteStatus("Error calculating max stack value..., try Check \"Preserve All Metadata\"");
                else
                {
                    if (ex.Message.Length > 100)
                        ShowFormLog(ex.Message);
                    else
                        WriteStatus("Error: " + ex.Message.Trim());
                }
                EnableControl(true);
            }
        }

        #region Check Blocks

        private bool CheckBlocks()
        {
            var isError = false;
            var logInfo = "";

            var blocksToFind = Regex.Split(InputFindText, @"^={3,}", RegexOptions.Multiline).Select(block => block.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries).ToList()).ToList();

            var blocksToReplace = Regex.Split(InputReplaceText, @"^={3,}", RegexOptions.Multiline).Select(block => block.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries).ToList()).ToList();

            if (blocksToFind.Count != blocksToReplace.Count)
            {
                MessageBox.Show(
                    $@"Size of Blocks Find and Replace not Match\r\nBlocks Find: {blocksToFind.Count}\r\nBlocks Replace: {blocksToReplace.Count}",
                    @"Blocks Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                isError = true;
            }

            if (!isError)
            {
                for (var i = 0; i < blocksToFind.Count; i++)
                {
                    if (blocksToFind[i].Count != blocksToReplace[i].Count)
                    {
                        logInfo +=
                            $"Error, Block {i + 1} has different size\r\nFind ({blocksToFind[i].Count}):\r\n{string.Join("\r\n", blocksToFind[i].ToArray())}\r\n\r\nReplace ({blocksToReplace[i].Count}):\r\n{string.Join("\r\n", blocksToReplace[i].ToArray())}\r\n================================================\r\n";

                        isError = true;
                    }
                }
            }

            try
            {
                Module = ModuleDefMD.Load(ModuleName);
            }
            catch
            {
                WriteStatus($"Error Loading \"{Path.GetFileName(ModuleName)}\" Maybe not .NET Executable");
                isError = true;
            }

            if (!isError)
            {
                for (var i = 0; i < blocksToFind.Count; i++)
                {
                    var block = blocksToFind[i];
                    var instructions = new List<Instruction>();
                    for (var j = 0; j < block.Count; j++)
                    {
                        var line = block[j];
                        OpCode opCode = null;
                        var instr = new Instruction();
                        var toInstr = line.Trim().Split(new[] { ' ', '\t' }, 2);
                        if (toInstr.Length > 0)
                            opCode = GetOpCodeFromString(toInstr[0]);
                        if (opCode != null)
                        {
                            instr.OpCode = opCode;
                            if (toInstr.Length == 2)
                            {
                                instr.Operand = toInstr[1].Trim().Trim('"');
                            }
                            instructions.Add(instr);
                        }
                        else
                        {
                            logInfo +=
                                $"\"{toInstr[0]}\" is not valid OpCode, Block Find {i + 1} Line {j + 1}\r\n{line}";
                            isError = true;
                        }
                    }

                    BlocksFind.Add(instructions);
                }
            }

            if (!isError)
            {
                for (var i = 0; i < blocksToReplace.Count; i++)
                {
                    var block = blocksToReplace[i];
                    var instructions = new List<Instruction>();
                    for (var j = 0; j < block.Count; j++)
                    {
                        var line = block[j];
                        var instr = new Instruction();
                        var toInstr = line.Trim().Split(new[] { ' ', '\t' }, 2);

                        if (toInstr[0].Trim() == "=")
                        {
                            instr.Operand = "=";
                            instructions.Add(instr);
                            continue;
                        }

                        var opCode = GetOpCodeFromString(toInstr[0]);

                        if (opCode != null)
                        {
                            instr.OpCode = opCode;
                            if (toInstr.Length == 2)
                            {
                                if (opCode == OpCodes.Call)
                                {
                                    var mdToken = toInstr[1].Trim().ToUpper();
                                    if (mdToken == "" || !Regex.IsMatch(mdToken, @"^(0[Xx])?[A-F0-9]{8}$"))
                                        isError = true;
                                    else
                                    {
                                        var rid = uint.Parse(mdToken.ToUpper().Replace("0X", ""), System.Globalization.NumberStyles.HexNumber);
                                        var isMethod = Module.ResolveToken(rid);
                                        if (isMethod == null)
                                            isError = true;
                                        instr.Operand = rid;
                                    }

                                    if (isError)
                                        logInfo +=
                                            $"\"{mdToken}\" is invalid Operand/MDToken for \"Call\", Block Replace {i + 1} Line {j + 1}\r\nThe value Should be Hex or MDToken like 06000001 or 0x06000001\r\n{line}\r\n";
                                }
                                else
                                    instr.Operand = toInstr[1].Trim().Trim('"');
                            }

                            instructions.Add(instr);
                        }
                        else
                        {
                            logInfo +=
                                $"\"{toInstr[0]}\" is not valid OpCode, Block Replace {i + 1} Line {j + 1}\r\n{line}";
                            isError = true;
                        }
                    }

                    BlocksReplace.Add(instructions);
                }
            }

            if (isError)
            {
                if (logInfo != "")
                    ShowFormLog(logInfo);
                EnableControl(true);
                IsRunning = false;
                Module?.Dispose();
                return false;
            }

            return true;
        }

        private static OpCode GetOpCodeFromString(string str)
        {
            var opString = str.Trim().ToLower().Replace(".", "_");
            opString = char.ToUpper(opString[0]) + opString.Substring(1);
            opString = Regex.Replace(opString, @"_[a-z]", match => match.ToString().ToUpper());
            var opCodeResult = typeof(OpCodes).GetField(opString);
            if (opCodeResult != null)
                return (OpCode)opCodeResult.GetValue(null);
            return null;
        }

        #endregion
        
        private void WriteStatus(string text)
        {
            Invoke((MethodInvoker)delegate
            {
                lblStatus.Text = text;
            });
        }

        private void ReplaceInstructions()
        {
            var currentBlockNum = "";

            foreach (var type in Module.GetTypes())
            {
                foreach (var method in type.Methods)
                {
                    var isNewMethod = true;
                    if (!method.HasBody)
                        continue;
                    var instructions = method.Body.Instructions;
                    for (var i = 0; i < instructions.Count; i++)
                    {
                        // start replace
                        for (var b = 0; b < BlocksFind.Count; b++)
                        {
                            var isMatched = false;
                            var j = i;
                            var blockFind = BlocksFind[b];

                            // check
                            foreach (var t in blockFind)
                            {
                                if (instructions[j].OpCode == t.OpCode)
                                {
                                    if (t.OpCode == OpCodes.Call)
                                    {
                                        isMatched = instructions[j].Operand != null && instructions[j].Operand.ToString().Contains(t.Operand.ToString());
                                    }
                                    else if (t.Operand != null)
                                    {
                                        isMatched = instructions[j].Operand.ToString().Contains(t.Operand.ToString());
                                    }
                                    else
                                        isMatched = true;
                                }
                                else
                                {
                                    isMatched = false;
                                    break;
                                }
                                j++;
                            }

                            if (isMatched)
                            {
                                if (isNewMethod)
                                {
                                    isNewMethod = false;
                                    EditedMethodsCount++;
                                    LogInfo +=
                                        $"==========================================\r\nMethod: {method.FullName} || MDToken: 0x06{method.MDToken.Rid:X6}\r\n";
                                    currentBlockNum = "";
                                }
                                if (currentBlockNum != (b + 1).ToString())
                                {
                                    currentBlockNum = (b + 1).ToString();
                                    LogInfo += $"\r\nBlock Find and Replace: {(b + 1)}\r\n";
                                }

                                // Do replace
                                j = i;
                                for (var k = 0; k < blockFind.Count; k++)
                                {
                                    var newOperand = BlocksReplace[b][k].Operand;
                                    if (newOperand.ToString() == "=")
                                    {
                                        LogInfo +=
                                            $"#{j} --> {instructions[j].OpCode}  {instructions[j].Operand}  ==>  No Change\r\n";
                                    }
                                    else
                                    {
                                        if (BlocksReplace[b][k].OpCode == OpCodes.Call)
                                            newOperand = Module.ResolveToken((uint)BlocksReplace[b][k].Operand);

                                        LogInfo +=
                                            $"#{j} --> {instructions[j].OpCode}  {instructions[j].Operand}  ==>  {BlocksReplace[b][k].OpCode}  {newOperand}\r\n";

                                        method.Body.Instructions[j].OpCode = BlocksReplace[b][k].OpCode;
                                        method.Body.Instructions[j].Operand = newOperand;
                                    }

                                    j++;
                                }

                                ReplacedBlocks++;
                                // set method.Body.Instructions loop position
                                i = j;
                            }
                        }
                    }
                }
            }
        }

        private void ShowFormLog(string text)
        {
            Invoke((MethodInvoker)delegate
            {
                ShowInTaskbar = false;
                var frmLog = new FormLog(this) { txtFormLog = { Text = text } };
                frmLog.ShowDialog();
                ShowInTaskbar = true;
            });
        }

        #endregion


    }
}