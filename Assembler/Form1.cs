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
                ArrayList instructions= new ArrayList();
                try
                {
                    instructions = CodeReader.getInstructions(read);
                }
                catch (SyntaxErrorException ex)
                {
                    //OH NOES!!! syntax error. make a message box or something.
                    MessageBox.Show(ex.Message, "Error Message",
                                 MessageBoxButtons.OK,
                                 MessageBoxIcon.Error);
                }
                int[] binaryinstructions= new int[instructions.Count];
                for (int i = 0; i < instructions.Count;i++ )
                {
                    binaryinstructions[i] = (instructions[i] as Instruction).intValueOfInstruction();
                }
                CodeWriter.writeBinary(binaryinstructions, "code.out");
                int[] thisstuff = CodeReader.readBinary("code.out");
                Processor.executeAll(thisstuff);
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