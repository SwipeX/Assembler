using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Assembler
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void loadFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog {Filter = "assembly files (*.s)|*.s|All files (*.*)|*.*"};
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                codeView.Text = String.Join(Environment.NewLine, CodeReader.ReadAll(dialog.FileName));
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
