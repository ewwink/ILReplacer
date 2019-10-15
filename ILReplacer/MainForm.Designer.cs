namespace ILReplacer
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.label1 = new System.Windows.Forms.Label();
            this.txtFile = new System.Windows.Forms.TextBox();
            this.btnOpenFile = new System.Windows.Forms.Button();
            this.txtInputFind = new System.Windows.Forms.TextBox();
            this.lblReplaceBlocks = new System.Windows.Forms.Label();
            this.txtInputReplace = new System.Windows.Forms.TextBox();
            this.btnReplace = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lblFindBlocks = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cboxShowLog = new System.Windows.Forms.CheckBox();
            this.cboxFlagAll = new System.Windows.Forms.CheckBox();
            this.menuTop = new System.Windows.Forms.MenuStrip();
            this.saveBlocksToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSaveBLocks = new System.Windows.Forms.ToolStripMenuItem();
            this.menuLoadBLocks = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuExit = new System.Windows.Forms.ToolStripMenuItem();
            this.tableLayoutPanel1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.menuTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Assembly Name:";
            // 
            // txtFile
            // 
            this.txtFile.AllowDrop = true;
            this.txtFile.Location = new System.Drawing.Point(105, 32);
            this.txtFile.Name = "txtFile";
            this.txtFile.Size = new System.Drawing.Size(410, 20);
            this.txtFile.TabIndex = 1;
            this.txtFile.DragDrop += new System.Windows.Forms.DragEventHandler(this.txtFile_DragDrop);
            this.txtFile.DragEnter += new System.Windows.Forms.DragEventHandler(this.txtFile_DragEnter);
            // 
            // btnOpenFile
            // 
            this.btnOpenFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOpenFile.Location = new System.Drawing.Point(522, 30);
            this.btnOpenFile.Name = "btnOpenFile";
            this.btnOpenFile.Size = new System.Drawing.Size(76, 23);
            this.btnOpenFile.TabIndex = 2;
            this.btnOpenFile.Text = "Browse";
            this.btnOpenFile.UseVisualStyleBackColor = true;
            this.btnOpenFile.Click += new System.EventHandler(this.btnOpenFile_Click);
            // 
            // txtInputFind
            // 
            this.txtInputFind.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtInputFind.Location = new System.Drawing.Point(3, 23);
            this.txtInputFind.Multiline = true;
            this.txtInputFind.Name = "txtInputFind";
            this.txtInputFind.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtInputFind.Size = new System.Drawing.Size(579, 131);
            this.txtInputFind.TabIndex = 4;
            this.txtInputFind.WordWrap = false;
            this.txtInputFind.TextChanged += new System.EventHandler(this.txtInputFind_TextChanged);
            this.txtInputFind.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SelectAll);
            // 
            // lblReplaceBlocks
            // 
            this.lblReplaceBlocks.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblReplaceBlocks.AutoSize = true;
            this.lblReplaceBlocks.Location = new System.Drawing.Point(3, 160);
            this.lblReplaceBlocks.Name = "lblReplaceBlocks";
            this.lblReplaceBlocks.Size = new System.Drawing.Size(119, 13);
            this.lblReplaceBlocks.TabIndex = 5;
            this.lblReplaceBlocks.Text = "Replace With Blocks: 0";
            // 
            // txtInputReplace
            // 
            this.txtInputReplace.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtInputReplace.Location = new System.Drawing.Point(3, 180);
            this.txtInputReplace.Multiline = true;
            this.txtInputReplace.Name = "txtInputReplace";
            this.txtInputReplace.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtInputReplace.Size = new System.Drawing.Size(579, 132);
            this.txtInputReplace.TabIndex = 7;
            this.txtInputReplace.WordWrap = false;
            this.txtInputReplace.TextChanged += new System.EventHandler(this.txtInputReplace_TextChanged);
            this.txtInputReplace.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SelectAll);
            // 
            // btnReplace
            // 
            this.btnReplace.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnReplace.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReplace.Location = new System.Drawing.Point(256, 424);
            this.btnReplace.Name = "btnReplace";
            this.btnReplace.Size = new System.Drawing.Size(91, 25);
            this.btnReplace.TabIndex = 8;
            this.btnReplace.Text = "Replace IL";
            this.btnReplace.UseVisualStyleBackColor = true;
            this.btnReplace.Click += new System.EventHandler(this.btnReplace_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.lblFindBlocks, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtInputFind, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.txtInputReplace, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.lblReplaceBlocks, 0, 2);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(13, 59);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(585, 315);
            this.tableLayoutPanel1.TabIndex = 9;
            // 
            // lblFindBlocks
            // 
            this.lblFindBlocks.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblFindBlocks.AutoSize = true;
            this.lblFindBlocks.Location = new System.Drawing.Point(3, 3);
            this.lblFindBlocks.Name = "lblFindBlocks";
            this.lblFindBlocks.Size = new System.Drawing.Size(74, 13);
            this.lblFindBlocks.TabIndex = 6;
            this.lblFindBlocks.Text = "Find Blocks: 0";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 454);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(607, 22);
            this.statusStrip1.TabIndex = 11;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lblStatus
            // 
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(16, 17);
            this.lblStatus.Text = "...";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.cboxShowLog);
            this.groupBox1.Controls.Add(this.cboxFlagAll);
            this.groupBox1.Location = new System.Drawing.Point(12, 379);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(588, 39);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Options:";
            // 
            // cboxShowLog
            // 
            this.cboxShowLog.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cboxShowLog.AutoSize = true;
            this.cboxShowLog.Checked = true;
            this.cboxShowLog.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cboxShowLog.Location = new System.Drawing.Point(310, 16);
            this.cboxShowLog.Name = "cboxShowLog";
            this.cboxShowLog.Size = new System.Drawing.Size(79, 17);
            this.cboxShowLog.TabIndex = 2;
            this.cboxShowLog.Text = "Show Logs";
            this.cboxShowLog.UseVisualStyleBackColor = true;
            this.cboxShowLog.CheckedChanged += new System.EventHandler(this.cboxShowLog_CheckedChanged);
            // 
            // cboxFlagAll
            // 
            this.cboxFlagAll.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cboxFlagAll.AutoSize = true;
            this.cboxFlagAll.Location = new System.Drawing.Point(174, 16);
            this.cboxFlagAll.Name = "cboxFlagAll";
            this.cboxFlagAll.Size = new System.Drawing.Size(130, 17);
            this.cboxFlagAll.TabIndex = 1;
            this.cboxFlagAll.Text = "Preserve All Metadata";
            this.cboxFlagAll.UseVisualStyleBackColor = true;
            this.cboxFlagAll.CheckedChanged += new System.EventHandler(this.cboxForceSave_CheckedChanged);
            // 
            // menuTop
            // 
            this.menuTop.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.menuTop.GripStyle = System.Windows.Forms.ToolStripGripStyle.Visible;
            this.menuTop.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveBlocksToolStripMenuItem});
            this.menuTop.Location = new System.Drawing.Point(0, 0);
            this.menuTop.Name = "menuTop";
            this.menuTop.Size = new System.Drawing.Size(607, 25);
            this.menuTop.TabIndex = 14;
            // 
            // saveBlocksToolStripMenuItem
            // 
            this.saveBlocksToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuSaveBLocks,
            this.menuLoadBLocks,
            this.MenuExit});
            this.saveBlocksToolStripMenuItem.Name = "saveBlocksToolStripMenuItem";
            this.saveBlocksToolStripMenuItem.Size = new System.Drawing.Size(53, 21);
            this.saveBlocksToolStripMenuItem.Text = "Menu";
            // 
            // menuSaveBLocks
            // 
            this.menuSaveBLocks.Name = "menuSaveBLocks";
            this.menuSaveBLocks.Size = new System.Drawing.Size(145, 22);
            this.menuSaveBLocks.Text = "Save Blocks";
            this.menuSaveBLocks.Click += new System.EventHandler(this.menuSaveBLocks_Click);
            // 
            // menuLoadBLocks
            // 
            this.menuLoadBLocks.Name = "menuLoadBLocks";
            this.menuLoadBLocks.Size = new System.Drawing.Size(145, 22);
            this.menuLoadBLocks.Text = "Load Blocks";
            this.menuLoadBLocks.Click += new System.EventHandler(this.menuLoadBLocks_Click);
            // 
            // MenuExit
            // 
            this.MenuExit.Name = "MenuExit";
            this.MenuExit.Size = new System.Drawing.Size(145, 22);
            this.MenuExit.Text = "Exit";
            this.MenuExit.Click += new System.EventHandler(this.MenuExit_Click);
            // 
            // MainForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(607, 476);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuTop);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.btnOpenFile);
            this.Controls.Add(this.btnReplace);
            this.Controls.Add(this.txtFile);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuTop;
            this.MinimumSize = new System.Drawing.Size(500, 400);
            this.Name = "MainForm";
            this.Text = "ILReplacer";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.txtFile_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.txtFile_DragEnter);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.menuTop.ResumeLayout(false);
            this.menuTop.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtFile;
        private System.Windows.Forms.Button btnOpenFile;
        private System.Windows.Forms.TextBox txtInputFind;
        private System.Windows.Forms.Label lblReplaceBlocks;
        private System.Windows.Forms.TextBox txtInputReplace;
        private System.Windows.Forms.Button btnReplace;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.Label lblFindBlocks;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox cboxFlagAll;
        private System.Windows.Forms.CheckBox cboxShowLog;
        private System.Windows.Forms.MenuStrip menuTop;
        private System.Windows.Forms.ToolStripMenuItem saveBlocksToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem menuSaveBLocks;
        private System.Windows.Forms.ToolStripMenuItem menuLoadBLocks;
        private System.Windows.Forms.ToolStripMenuItem MenuExit;
    }
}

