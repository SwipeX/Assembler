using System;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;

namespace Assembler
{
    public class CodeReader
    {
        private static readonly Regex regex = new Regex(@"(?:\S|(?<=\\))+",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public static ArrayList ReadAll(String fileName)
        {
            var text = new ArrayList();
            foreach (string line in File.ReadAllLines(fileName))
                text.Add(line);
            return text;
        }

        public static ArrayList NormalizeText(ArrayList input)
        {
            for (int i = 0; i < input.Count; i++)
            {
                var element = input[i] as string;
                MatchCollection matches = regex.Matches(element);
                foreach (Match match in matches)
                {
                    input[i] = match.Captures[0] + "," + match.Captures[1];
                }
            }
            return input;
        }

        public static ArrayList getInstructions(ArrayList normalizedText)
        {
            var instructions = new ArrayList();
            for (int index = 0; index < normalizedText.Count; index++)
            {
                var element = normalizedText[index] as string;
                string[] splitText = element.Split(new[] {','});
                instructions.Add(new Instruction(Opcodes.GetOpcode(splitText[0]), Convert.ToInt32(splitText[1]), index));
            }
            return instructions;
        }
    }
}