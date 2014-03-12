using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

//Written by Ryan Serva
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

        public static ArrayList deleteComments(ArrayList input)
        {
            for (int i = 0; i < input.Count; i++)
            {
                var element = input[i] as string;
                MatchCollection matches = preregex.Matches(element);
                foreach (Match match in matches)
                {
                    input[i] = ((String) input[i]).Substring(1, match.Index - 1);
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
                var g = new ArrayList();
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
            var temp = new ArrayList();
            var newNormalizedText = new ArrayList();
            var indexes = new Dictionary<string, int>();
            int linecount = 0;
            for (int i = 0; i < normalizedText.Count; i++)
            {
                temp = normalizedText[i] as ArrayList;
                if (temp.Count > 2)
                {
                    throw new SyntaxErrorException();
                }
                if (temp.Count == 1)
                {
                    if (temp[0].Equals("nop")||temp[0].Equals("nota"))
                    {
                        newNormalizedText.Add(temp);
                        linecount++;
                    }
                    else if ((temp[0] as string).EndsWith(":"))
                    {
                        //do jumpy stuff
                        indexes.Add((temp[0] as string).Substring(0, (temp[0] as string).Length - 1), linecount);
                    }
                    else
                    {
                        throw new SyntaxErrorException("Unknown instruction line:"+(i+1));
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
                if (textEquals((newNormalizedText[i] as ArrayList)[0] as string, "ba", "be", "bl", "bg"))
                {
                    foreach (var pair in indexes)
                    {
                        if (pair.Key.Equals(((newNormalizedText[i] as ArrayList)[1] as string)))
                        {
                            (newNormalizedText[i] as ArrayList)[1] = Convert.ToString(pair.Value);
                        }
                    }
                }
            }
            return newNormalizedText;
        }

        private static Boolean textEquals(String input, params String[] possible)
        {
            foreach (String str in possible)
            {
                if (str.Equals(input))
                    return true;
            }
            return false;
        }

        public static ArrayList getInstructions(ArrayList normalizedText)
        {
            try
            {
                normalizedText = handleJumps(normalizedText);
            }
            catch (SyntaxErrorException e)
            {
                throw e;
            }
            var instructions = new ArrayList();
            for (int index = 0; index < normalizedText.Count; index++)
            {
                var element = (normalizedText[index] as ArrayList)[0] as string;
                var value = "0";
                bool flag = false;
                if((normalizedText[index] as ArrayList).Count>1){
                    var temp = ((normalizedText[index] as ArrayList)[1] as string);
                    try
                    {
                        if (temp.Length > 2 && temp.StartsWith("#$"))
                        {
                            //immediate value
                            value = temp.Substring(2, temp.Length - 2);
                            flag = true;
                        }
                        else if (temp.Length > 1 && temp.StartsWith("$"))
                        {
                            //register value
                            value = temp.Substring(1, temp.Length - 1);
                        }
                        else
                        {
                            value = temp;
                            //jump
                        }
                    }
                    catch (Exception e)
                    {
                        throw new SyntaxErrorException(temp+" unrecognized");
                    }
                }
                int theOpcode = Opcodes.GetOpcode(element);
                if (theOpcode < 0)
                {
                    throw new SyntaxErrorException(element+" unrecognized");
                }
                instructions.Add(new Instruction(theOpcode, Convert.ToInt32(value), index,flag));
            }
            return instructions;
        }

        public static int[] readBinary(String fileName)
        {
            FileStream inFile = new FileStream(fileName, FileMode.Open);
            var reader = new BinaryReader(inFile);
            ArrayList instructions = new ArrayList();
            int i = 0;
            while (inFile.Position != inFile.Length)
            {
                instructions.Add(reader.ReadInt32());
                i++;
            }
            /*
            for (int i = 0; i < count; i++)
            {
                instructions[i] = reader.ReadInt32();
            }
            */
            int[] fuckYOUCSHARP = new int[instructions.Count];
            for (int g = 0; g < instructions.Count; g++)
            {
                fuckYOUCSHARP[g] = (int)instructions[g];
            }
            return fuckYOUCSHARP;
        }
    }
}