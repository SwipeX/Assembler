using System;
using System.IO;

namespace Assembler
{
    internal class CodeWriter
    {
        //Write length of int[], then write the int's one by one.
        public static void writeBinary(int[] instructions, String fileName)
        {
            var writer = new BinaryWriter(new FileStream(fileName, FileMode.Create));
            writer.Write(instructions.Length);
            foreach (int instruction in instructions)
            {
                writer.Write(instruction);
            }
            writer.Flush();
            writer.Close();
        }
    }
}