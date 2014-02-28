using System;
using System.Collections.Generic;
using System.IO;

namespace Assembler
{
    public class CodeReader
    {
        public static LinkedList<String> ReadAll(String fileName)
        {
            var text = new LinkedList<String>();
            foreach (string line in File.ReadAllLines(fileName))
                text.AddLast(line);
            return text;
        }

        public static LinkedList<String> NormalizeText(LinkedList<String> input)
        {
            //TODO regex parse and re-insert the instruction string(s).
            return input;
        }

        public static LinkedList<Instruction> getInstructions(LinkedList<String> normalizedText)
        {
            var instructions = new LinkedList<Instruction>();
            for (int index = 0; index < normalizedText.Count; index++)
            {
                instructions.AddLast(new Instruction(0, 0, index));
            }
            return instructions;
        }
    }
}