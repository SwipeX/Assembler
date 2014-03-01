using System;
using System.Collections;
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
                ArrayList read = CodeReader.ReadAll(dialog.FileName);
                read = CodeReader.NormalizeText(read);
                ArrayList instructions = CodeReader.getInstructions(read);
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Memory.ACC++;
            registerTable.DataSource = Memory.getValues();
        }
    }
}