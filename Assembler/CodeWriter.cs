using System;
using System.IO;

namespace Assembler
{
    internal class CodeWriter
    {
        //Write length of int[], then write the int's one by one.
        public static void writeBinary(int[] instructions, String fileName)
        {
            using (var writer = new BinaryWriter(new FileStream(fileName, FileMode.Create)))
            {
                foreach (int instruction in instructions)
                {
                    writer.Write(instruction);
                }
                Form1.toolStripStatusLabel1.Text = "Wrote: " + fileName;
                Form1.updateUI();
            }
        }
    }
}