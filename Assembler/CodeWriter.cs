using System;
using System.IO;

namespace Assembler
{
    internal class CodeWriter
    {
        //Write length of int[], then write the int's one by one.
        public static void writeBinary(int[] instructions, String fileName)
        {
            using (BinaryWriter writer = new BinaryWriter(new FileStream(fileName, FileMode.Create))) {
                foreach (int instruction in instructions)
                {
                    writer.Write(instruction);
                }
            }
           }
    }
}