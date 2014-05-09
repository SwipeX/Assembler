using System;
using System.Collections;
using System.Diagnostics;
using System.Windows.Forms;

namespace Assembler
{
    public partial class Form1 : Form
    {
        private static string lastLoadedFile = "";
        private static int[] stepInstructions;

        public Form1()
        {
            InitializeComponent();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            updateUI();
        }

        public static void updateUI()
        {
            if (Memory.LocalCache != null && Memory.LocalCache.hits + Memory.LocalCache.misses > 0)
            hitLabel.Text = "Hits: " + Memory.LocalCache.hits + " Misses: " + Memory.LocalCache.misses
                + " Hit Percentage:"+(Memory.LocalCache.hits*100 / (Memory.LocalCache.hits+Memory.LocalCache.misses)) + " %";
            if (registerTable == null)
            {
                registerTable = new DataGridView();
                statusStrip1 = new StatusStrip();
            }
            registerTable.DataSource = Memory.getValues();
            registerTable.Invalidate();
            statusStrip1.Refresh();
            registerTable.Update();
            statusStrip1.Update();
            Application.DoEvents();
        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog {Filter = "assembly files (*.s)|*.s|All files (*.*)|*.*"};
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                ArrayList read = CodeReader.ReadAll(dialog.FileName);
                read = CodeReader.NormalizeText(read);
                var instructions = new ArrayList();
                try
                {
                    instructions = CodeReader.getInstructions(read);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error Message",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
                var binaryinstructions = new int[instructions.Count];
                for (int i = 0; i < instructions.Count; i++)
                {
                    binaryinstructions[i] = (instructions[i] as Instruction).intValueOfInstruction();
                }
                string name = dialog.SafeFileName;
                name = name.Substring(0, name.IndexOf('.')) + ".out";
                CodeWriter.writeBinary(binaryinstructions, name);
                lastLoadedFile = name;
            }
        }
        private void stepToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (stepInstructions == null)
            {
                Memory.LocalCache = new Cache(Memory.cacheSize, Memory.blockSize);
                stepInstructions = CodeReader.readBinary(lastLoadedFile);
                int[] tempstep = new int[1];
                tempstep[0] = stepInstructions[Memory.PC];
                Processor.executeAll(tempstep);
                toolStripStatusLabel1.Text = "Wait cycles: " + Processor.waitCount;
                updateUI();
            }
            else if (Memory.PC < stepInstructions.Length)
            {
                int[] tempstep = new int[1];
                tempstep[0] = stepInstructions[Memory.PC];
                Processor.executeAll(tempstep);
                toolStripStatusLabel1.Text = Opcodes.NAMES[stepInstructions[Memory.PC] >> 25];
                //Processor.execute(stepInstructions[Memory.PC]);
                //Memory.PC++;
                updateUI();
            }
            else
            {
                stepInstructions = null;
                toolStripStatusLabel1.Text = "Done!";
                updateUI();
                Memory.clear();
            }
        }

        private void runToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Memory.LocalCache = new Cache(Memory.cacheSize, Memory.blockSize);
            int[] packedInstructions = CodeReader.readBinary(lastLoadedFile);
            try
            {
                Processor.executeAll(packedInstructions);
                toolStripStatusLabel1.Text = "Wait cycles: " + Processor.waitCount;
                updateUI();
            }
            catch (Exception ep)
            {
                MessageBox.Show(ep.Message, "Error Message",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            Memory.clear();
        }

        private void resetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Memory.reset();
            updateUI();
        }
        private void toggle_Click(object sender, EventArgs e)
        {
            Processor.THRESHOLD = Processor.THRESHOLD == 1 ? 2 : 1;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            Memory.directCache = true;
        }

        private void twoWayBtn_CheckedChanged(object sender, EventArgs e)
        {
            Memory.directCache = false;
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            int value = (int)numericUpDown2.Value;
            if (value < 2) value = 2;
            if (value > 16) value = 16;
            numericUpDown2.Value = value;
            Memory.cacheSize = value;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            int value = (int)numericUpDown1.Value;
            if (value < 1) value = 1;
            numericUpDown1.Value = value;
            Memory.blockSize = value;
        }
    }
}
