using System;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;

namespace Assembler
{
    public class CodeReader
    {
        //private static readonly Regex regex = new Regex(@"(?:\S|(?<=\\))+",
        private static readonly Regex preregex = new Regex(@"\!.*$*",
            RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static readonly Regex regex = new Regex(@"\S+",
            RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public static ArrayList ReadAll(String fileName)
        {
            var text = new ArrayList();
            foreach (string line in File.ReadAllLines(fileName))
                text.Add(line);
            return text;
        }
        public static ArrayList deleteComments(ArrayList input){
            for (int i = 0; i < input.Count; i++)
            {
                var element = input[i] as string;
                MatchCollection matches = preregex.Matches(element);
                foreach (Match match in matches)
                {
                    input[i] = ((String)input[i]).Substring(1, match.Index-1);
                }
            }
                return input;
        }
        public static ArrayList NormalizeText(ArrayList input)
        {
            input = deleteComments(input);
            for (int i = 0; i < input.Count; i++)
            {
                var element = input[i] as string;
                MatchCollection matches = regex.Matches(element);
                ArrayList g = new ArrayList();
                foreach (Match match in matches)
                {
                    g.Add(match.Groups[0]);
                }
                input[i] = g;
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