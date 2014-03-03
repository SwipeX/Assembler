using System;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace Assembler
{
    public class CodeReader
    {
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
                    g.Add(match.Captures[0].ToString().ToLower());
                }
                input[i] = g;
            }
            return input;
        }
        public static ArrayList handleJumps(ArrayList normalizedText)
        {
            ArrayList temp = new ArrayList();
            ArrayList newNormalizedText = new ArrayList();
            Dictionary<string, int> indexes = new Dictionary<string, int>();
            int linecount = 0;
            for (int i = 0; i < normalizedText.Count; i++)
            {
                temp = normalizedText[i]as ArrayList;
                if (temp.Count > 2)
                {
                    throw new SyntaxErrorException();
                }
                else if(temp.Count ==1){
                    if (temp[0].Equals("nop"))
                    {
                        newNormalizedText.Add(temp);
                        linecount++;
                    }
                    else if((temp[0] as string).EndsWith(":"))
                    {
                        //do jumpy stuff
                        indexes.Add((temp[0] as string).Substring(0, (temp[0] as string).Length - 1), linecount);
                    }
                    else{
                        throw new SyntaxErrorException();
                        
                    }
                }
                else if (temp.Count == 2)
                {
                    newNormalizedText.Add(temp);
                    linecount++;
                }
            }
            for (int i = 0; i < newNormalizedText.Count; i++)
            {
                if (((newNormalizedText[i] as ArrayList)[0] as string).Equals("ba") || ((newNormalizedText[i] as ArrayList)[0] as string).Equals("be")
                    || ((newNormalizedText[i] as ArrayList)[0] as string).Equals("bl") || ((newNormalizedText[i] as ArrayList)[0] as string).Equals("bg")
                    )
                {
                    foreach(KeyValuePair<string, int> pair in indexes){
                        if (pair.Key.Equals(((newNormalizedText[i] as ArrayList)[1] as string)))
                        {
                            (newNormalizedText[i] as ArrayList)[1] = pair.Value;
                        }
                    }
                }
            }
                return newNormalizedText;
        }
        public static ArrayList getInstructions(ArrayList normalizedText)
        {
            try
            {
                normalizedText = handleJumps(normalizedText);
            }
            catch (Exception e)
            {
                throw e;
            }
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