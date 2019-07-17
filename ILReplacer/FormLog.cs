using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ILReplacer
{
    public partial class FormLog : Form
    {
        public MainForm mForm;
        public FormLog(MainForm mf)
        {
            InitializeComponent();
            mForm = mf;
        }

        private void btnFormLogClose_Click(object sender, EventArgs e)
        {
            txtFormLog.Text = "";
            this.Close();
        }

        private void txtFormLog_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
            {
                if (sender != null)
                    ((TextBox)sender).SelectAll();
            }
        }

        private void FormLog_Load(object sender, EventArgs e)
        {
            this.Text = string.Format("ILReplacer v{0} Logs | by ewwink", Application.ProductVersion);
            this.Size = mForm.Size;
            this.Location = mForm.Location;
        }

        private void FormLog_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Maximized)
                mForm.WindowState = FormWindowState.Maximized;               
            else
            {
                mForm.WindowState = FormWindowState.Normal;
                mForm.Size = this.Size;
            }
                
        }

        private void FormLog_Move(object sender, EventArgs e)
        {
            mForm.Location = this.Location;
        }
    }
}
