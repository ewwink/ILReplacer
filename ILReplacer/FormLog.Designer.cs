namespace ILReplacer
{
    partial class FormLog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormLog));
            this.btnFormLogClose = new System.Windows.Forms.Button();
            this.txtFormLog = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnFormLogClose
            // 
            this.btnFormLogClose.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnFormLogClose.Location = new System.Drawing.Point(303, 336);
            this.btnFormLogClose.Name = "btnFormLogClose";
            this.btnFormLogClose.Size = new System.Drawing.Size(68, 32);
            this.btnFormLogClose.TabIndex = 0;
            this.btnFormLogClose.Text = "Close";
            this.btnFormLogClose.UseVisualStyleBackColor = true;
            this.btnFormLogClose.Click += new System.EventHandler(this.btnFormLogClose_Click);
            // 
            // txtFormLog
            // 
            this.txtFormLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFormLog.Location = new System.Drawing.Point(12, 12);
            this.txtFormLog.Multiline = true;
            this.txtFormLog.Name = "txtFormLog";
            this.txtFormLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtFormLog.Size = new System.Drawing.Size(651, 318);
            this.txtFormLog.TabIndex = 1;
            this.txtFormLog.WordWrap = false;
            this.txtFormLog.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtFormLog_KeyDown);
            // 
            // FormLog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(675, 380);
            this.Controls.Add(this.txtFormLog);
            this.Controls.Add(this.btnFormLogClose);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimizeBox = false;
            this.Name = "FormLog";
            this.Text = "ILReplacer Log";
            this.Load += new System.EventHandler(this.FormLog_Load);
            this.Move += new System.EventHandler(this.FormLog_Move);
            this.Resize += new System.EventHandler(this.FormLog_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnFormLogClose;
        public System.Windows.Forms.TextBox txtFormLog;
    }
}