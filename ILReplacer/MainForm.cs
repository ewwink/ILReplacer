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
        public bool isRunning = false;
        public bool PreserveAllFlags = false;
        public bool ShowLogs = true;
        public List<List<Instruction>> BlocksFind = new List<List<Instruction>>();
        public List<List<Instruction>> BlocksReplace = new List<List<Instruction>>();
        public string inputFindText = "";
        public string inputReplaceText = "";
        public string moduleName = "";
        public ModuleDefMD module = null;
        public int ReplacedBlocks = 0;
        public int EditedMethodsCount = 0;
        public string logInfo = "";

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.Text = string.Format("ILReplacer v{0} | by ewwink", Application.ProductVersion);
        }

        private void txtFile_DragDrop(object sender, DragEventArgs e)
        {
            var FileList = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            if (File.Exists(FileList[0]))
                txtFile.Text = FileList[0];
        }

        private void txtFile_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            txtFile.Size = new Size(this.Size.Width - 214, txtFile.Size.Height);
        }

        private void selectAll(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
            {
                if (sender != null)
                    ((TextBox)sender).SelectAll();
            }
        }

        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog
            {
                Title = "Select File",
                Filter = "Executables files|*.exe;*.dll|All files (*.*)|*.*"
            };

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                txtFile.Text = ofd.FileName;
            }
        }

        private void enableControl(bool isEnable)
        {
            Invoke((MethodInvoker)delegate
            {
                txtFile.Enabled = txtInputFind.Enabled = txtInputReplace.Enabled = isEnable;
            });
        }

        private void writeStatus(string text)
        {
            Invoke((MethodInvoker)delegate
            {
                lblStatus.Text = text;
            });
        }

        private void cboxForceSave_CheckedChanged(object sender, EventArgs e)
        {
            PreserveAllFlags = cboxFlagAll.Checked;
        }

        private void btnReplace_Click(object sender, EventArgs e)
        {
            if (isRunning)
                return;

            moduleName = txtFile.Text.Trim();
            if (!File.Exists(moduleName))
            {
                MessageBox.Show("File not Defined or not Exist: " + moduleName, "Warning!",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (txtInputFind.Text.Trim() == "" || txtInputReplace.Text.Trim() == "")
            {
                MessageBox.Show("instructions to Find or Replace is empty!", "Warning!",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            logInfo = "";
            isRunning = true;
            lblStatus.Text = "Running..";
            inputFindText = txtInputFind.Text;
            inputReplaceText = txtInputReplace.Text;
            ReplacedBlocks = 0;
            EditedMethodsCount = 0;
            enableControl(false);
            new Thread(prepareReplace)
            {
                IsBackground = true
            }.Start();
        }

        private void showFormLog(string text)
        {
            Invoke((MethodInvoker)delegate
            {
                this.ShowInTaskbar = false;
                FormLog frmLog = new FormLog(this);
                frmLog.txtFormLog.Text = text;
                frmLog.ShowDialog();
                this.ShowInTaskbar = true;
            });
        }

        private OpCode GetOpCodeFromString(string str)
        {
            string opString = str.Trim().ToLower().Replace(".", "_");
            opString = char.ToUpper(opString[0]) + opString.Substring(1);
            opString = Regex.Replace(opString, @"_[a-z]", (Match match) => match.ToString().ToUpper());
            var opCodeResult = typeof(OpCodes).GetField(opString);
            if (opCodeResult != null)
                return (OpCode)opCodeResult.GetValue(null);
            return null;
        }

        private void ReplaceInstructions()
        {
            string currentBlockNum = "";

            foreach (var type in module.GetTypes())
            {
                foreach (var method in type.Methods)
                {
                    bool isNewMethod = true;
                    if (!method.HasBody)
                        continue;
                    var instrs = method.Body.Instructions;
                    for (int i = 0; i < instrs.Count; i++)
                    {
                        // start replace
                        for (int B = 0; B < BlocksFind.Count; B++)
                        {
                            bool isMatched = false;
                            int j = i;
                            var blockFind = BlocksFind[B];

                            // check
                            for (int k = 0; k < blockFind.Count; k++)
                            {
                                if (instrs[j].OpCode == blockFind[k].OpCode)
                                {
                                    if (blockFind[k].OpCode == OpCodes.Call)
                                    {
                                        if (instrs[j].Operand == null)
                                        {
                                            isMatched = false;
                                        }
                                        else
                                        {
                                            if (instrs[j].Operand.ToString().Contains(blockFind[k].Operand.ToString()))
                                                isMatched = true;
                                            else
                                                isMatched = false;
                                        }
                                    }
                                    else if (blockFind[k].Operand != null)
                                    {
                                        if (instrs[j].Operand.ToString().Contains(blockFind[k].Operand.ToString()))
                                            isMatched = true;
                                        else
                                            isMatched = false;
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
                                    logInfo += string.Format(
                                        "==========================================\r\nMethod: {0} || MDToken: 0x06{1:X6}\r\n",
                                        method.FullName, method.MDToken.Rid);
                                    currentBlockNum = "";
                                }
                                if (currentBlockNum != (B + 1).ToString())
                                {
                                    currentBlockNum = (B + 1).ToString();
                                    logInfo += string.Format("\r\nBlock Find and Replace: {0}\r\n", (B + 1));
                                }

                                // Do replace
                                j = i;
                                for (int k = 0; k < blockFind.Count; k++)
                                {
                                    var newOperand = BlocksReplace[B][k].Operand;
                                    if (BlocksReplace[B][k].OpCode == OpCodes.Call)
                                        newOperand = module.ResolveToken((uint)BlocksReplace[B][k].Operand);

                                    logInfo += string.Format("#{0} --> {1}  {2}  ==>  {3}  {4}\r\n",
                                        j, instrs[j].OpCode, instrs[j].Operand,
                                        BlocksReplace[B][k].OpCode, newOperand);

                                    method.Body.Instructions[j].OpCode = BlocksReplace[B][k].OpCode;
                                    method.Body.Instructions[j].Operand = newOperand;
 
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

        private bool checkBlocks()
        {
            bool isError = false;
            string logInfo = "";
            var blocksToFind = new List<List<string>>();
            var blocksToReplace = new List<List<string>>();

            foreach (var block in Regex.Split(inputFindText, @"^={3,}", RegexOptions.Multiline))
            {
                var instrs = block.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                blocksToFind.Add(instrs);
            }

            foreach (var block in Regex.Split(inputReplaceText, @"^={3,}", RegexOptions.Multiline))
            {
                var instrs = block.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                blocksToReplace.Add(instrs);
            }

            if (blocksToFind.Count != blocksToReplace.Count)
            {
                MessageBox.Show(
                    string.Format("Size of Blocks Find and Replace not Match\r\nBlocks Find: {0}\r\nBlocks Replace: {1}",
                    blocksToFind.Count, blocksToReplace.Count),
                    "Blocks Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                isError = true;
            }

            if (!isError)
            {
                for (int i = 0; i < blocksToFind.Count; i++)
                {
                    if (blocksToFind[i].Count != blocksToReplace[i].Count)
                    {
                        logInfo += string.Format(
                            "Error, Block {0} has different size\r\nFind ({1}):\r\n{2}\r\n\r\nReplace ({3}):\r\n{4}\r\n{5}\r\n", i + 1,
                           blocksToFind[i].Count, string.Join("\r\n", blocksToFind[i].ToArray()),
                        blocksToReplace[i].Count, string.Join("\r\n", blocksToReplace[i].ToArray()),
                            "================================================");

                        isError = true;
                    }
                }
            }

            try
            {
                module = ModuleDefMD.Load(moduleName);
            }
            catch
            {
                writeStatus(string.Format("Error Loading \"{0}\" Maybe not .NET Executable", Path.GetFileName(moduleName)));
                isError = true;
            }

            if (!isError)
            {
                for (int i = 0; i < blocksToFind.Count; i++)
                {
                    var block = blocksToFind[i];
                    var instructions = new List<Instruction>();
                    for (int j = 0; j < block.Count; j++)
                    {
                        var line = block[j];
                        OpCode opCode = null;
                        Instruction instr = new Instruction();
                        string[] toInstr = line.Trim().Split(new char[] { ' ', '\t' }, 2);
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
                            logInfo += string.Format(
                                "\"{0}\" is not valid OpCode, Block Find {1} Line {2}\r\n{3}",
                                toInstr[0], i + 1, j + 1, line);
                            isError = true;
                        }
                    }

                    BlocksFind.Add(instructions);
                }
            }

            if (!isError)
            {
                for (int i = 0; i < blocksToReplace.Count; i++)
                {
                    var block = blocksToReplace[i];
                    var instructions = new List<Instruction>();
                    for (int j = 0; j < block.Count; j++)
                    {
                        var line = block[j];
                        OpCode opCode = null;
                        Instruction instr = new Instruction();
                        string[] toInstr = line.Trim().Split(new char[] { ' ', '\t' }, 2);
                        opCode = GetOpCodeFromString(toInstr[0]);

                        if (opCode != null)
                        {
                            instr.OpCode = opCode;
                            if (toInstr.Length == 2)
                            {
                                if (opCode == OpCodes.Call)
                                {
                                    string mdtoken = toInstr[1].Trim().ToUpper();
                                    if (mdtoken == "" || !Regex.IsMatch(mdtoken, @"^(0[Xx])?[A-F0-9]{8}$"))
                                        isError = true;
                                    else
                                    {
                                        uint rid = uint.Parse(mdtoken.ToUpper().Replace("0X", ""), System.Globalization.NumberStyles.HexNumber);
                                        var isMethod = module.ResolveToken(rid);
                                        if (isMethod == null)
                                            isError = true;
                                        instr.Operand = rid;
                                    }

                                    if (isError)
                                        logInfo += string.Format(
                                            "\"{0}\" is invalid Operand/MDToken for \"Call\", Block Replace {1} Line {2}\r\n{3}\r\n{4}\r\n",
                                            mdtoken, i + 1, j + 1, "The value Should be Hex or MDToken like 06000001 or 0x06000001", line);
                                }
                                else
                                    instr.Operand = toInstr[1].Trim().Trim('"');
                            }

                            instructions.Add(instr);
                        }
                        else
                        {
                            logInfo += string.Format(
                                "\"{0}\" is not valid OpCode, Block Replace {1} Line {2}\r\n{3}",
                                toInstr[0], i + 1, j + 1, line);
                            isError = true;
                        }
                    }

                    BlocksReplace.Add(instructions);
                }
            }

            if (isError)
            {
                if (logInfo != "")
                    showFormLog(logInfo);
                enableControl(true);
                isRunning = false;
                if (module != null)
                    module.Dispose();
                return false;
            }

            return true;
        }

        private void prepareReplace()
        {
            MetadataFlags AllFlags = MetadataFlags.PreserveAll | MetadataFlags.KeepOldMaxStack
                | MetadataFlags.AlwaysCreateGuidHeap | MetadataFlags.RoslynSortInterfaceImpl;

            try
            {
                bool isBlockGood = checkBlocks();
                if (!isBlockGood)
                    return;

                if (!module.IsILOnly)
                {
                    writeStatus("Info: The Assembly maybe contains unmanaged code");
                    Thread.Sleep(2000);
                }

                writeStatus("Processing: " + moduleName);

                ReplaceInstructions();

                string newName = string.Format("{0}_ILR{1}",
                    Path.GetFileNameWithoutExtension(moduleName),
                    Path.GetExtension(moduleName));

                if (ReplacedBlocks > 0)
                {
                    if (module.IsILOnly)
                    {
                        var ManagedOptions = new ModuleWriterOptions(module);
                        if (PreserveAllFlags)
                            ManagedOptions.MetadataOptions.Flags = AllFlags;

                        module.Write(Path.GetDirectoryName(moduleName) + "\\" + newName, ManagedOptions);
                    }
                    else
                    {
                        var UnmanagedOptions = new NativeModuleWriterOptions(module, true);
                        if (PreserveAllFlags)
                            UnmanagedOptions.MetadataOptions.Flags = AllFlags;

                        module.NativeWrite(newName, UnmanagedOptions);
                    }

                    if (ShowLogs)
                    {
                        writeStatus(string.Format(
                            "Done: Saved as {0} || Replaced {1} Blocks Instructions in {2} Methods",
                            newName, ReplacedBlocks, EditedMethodsCount));
                        showFormLog(logInfo);
                    }
                }
                else
                    writeStatus("Info: No Block Instructions Replaced!");

                //MessageBox.Show("File Saved as:\r\n" + newName, "Replaces Done",
                //        MessageBoxButtons.OK, MessageBoxIcon.Information);

                if (module != null)
                    module.Dispose();

                enableControl(true);
                isRunning = false;
            }
            catch (Exception ex)
            {
                if (module != null)
                    module.Dispose();

                isRunning = false;
                if (ex.Message.Contains("Error calculating max stack"))
                    writeStatus("Error calculating max stack value..., try Check \"Preserve All Metadata\"");
                else
                {
                    if (ex.Message.Length > 100)
                        showFormLog(ex.Message);
                    else
                        writeStatus("Error: " + ex.Message.Trim());
                }
                enableControl(true);
            }
        }

        private void cboxShowLog_CheckedChanged(object sender, EventArgs e)
        {
            ShowLogs = cboxShowLog.Checked;
        }

        private void MenuExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void menuSaveBLocks_Click(object sender, EventArgs e)
        {
            var sfd = new SaveFileDialog
            {
                Title = "Save Blocks File",
                Filter = "ILReplace files|*.ilr|Text files|*.txt|All files (*.*)|*.*"
            };

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                string blocksText = txtInputFind.Text.Trim() + "\r\n###########\r\n" + txtInputReplace.Text.Trim();
                File.WriteAllText(sfd.FileName, blocksText);
                writeStatus("Blocks Saved to: " + sfd.FileName);
            }
        }

        private void menuLoadBLocks_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog
            {
                Title = "Select Blocks File",
                Filter = "ILReplace files|*.ilr|Text files|*.txt|All files (*.*)|*.*"
            };

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string[] blocksText = File.ReadAllText(ofd.FileName)
                    .Split(new string[] { "###########" }, StringSplitOptions.None);
                if (blocksText.Length > 1)
                {
                    txtInputFind.Text = blocksText[0].Trim();
                    txtInputReplace.Text = blocksText[1].Trim();
                    writeStatus("Blocks loaded from: " + ofd.FileName);
                }
                else
                {
                    writeStatus("Blocks file empty or invalid: " + ofd.FileName);
                }
            }
        }

        private void txtInputFind_TextChanged(object sender, EventArgs e)
        {
            int blockCount = Regex.Split(txtInputFind.Text, @"^={3,}", RegexOptions.Multiline).Length;
            lblFindBlocks.Text = "Find Blocks: " + blockCount;
        }

        private void txtInputReplace_TextChanged(object sender, EventArgs e)
        {
            int blockCount = Regex.Split(txtInputReplace.Text, @"^={3,}", RegexOptions.Multiline).Length;
            lblReplaceBlocks.Text = "Replace With Blocks: " + blockCount;
        }
    }
}