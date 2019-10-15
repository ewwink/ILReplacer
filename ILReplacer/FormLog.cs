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
        public MainForm MainFrm;
        public FormLog(MainForm mf)
        {
            InitializeComponent();
            MainFrm = mf;
        }

        private void btnFormLogClose_Click(object sender, EventArgs e)
        {
            txtFormLog.Text = "";
            Close();
        }

        private void txtFormLog_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
            {
                ((TextBox) sender)?.SelectAll();
            }
        }

        private void FormLog_Load(object sender, EventArgs e)
        {
            Text = $@"ILReplacer v{Application.ProductVersion} Logs | by ewwink";
            Size = MainFrm.Size;
            Location = MainFrm.Location;
        }

        private void FormLog_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Maximized)
                MainFrm.WindowState = FormWindowState.Maximized;               
            else
            {
                MainFrm.WindowState = FormWindowState.Normal;
                MainFrm.Size = Size;
            }
                
        }

        private void FormLog_Move(object sender, EventArgs e)
        {
            MainFrm.Location = Location;
        }
    }
}
