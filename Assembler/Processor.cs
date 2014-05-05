using System;
using System.Collections.Generic;
using System.Threading;

namespace Assembler
{
    public class Processor
    {
        private static int[] instructions = new int[2];

        private static int opcode = -1;
        private static int opcode2 = -1;
        private static bool immediate;
        private static bool immediate2;
        private static int value;
        private static int value2;
        private static bool shouldReturn = true;
        private static int storeValA;
        private static int storeValB;
        private static bool hasBranched;
        private static byte finishSetting;
        private static byte wait;
        private static int acclast;
        public static int waitCount = 0;
        private static bool returnValB;
        private static readonly Dictionary<int, bool> pastBranches = new Dictionary<int, bool>();
        private static int predictedJump = -1; //-1 for none, else the line #

        public static int execute(int opcode, bool immediate, int value)
        {
            try
            {
                switch (opcode)
                {
                    case Opcodes.LDA:
                        if (immediate)
                        {
                            Memory.ACC = value;
                        }
                        else
                        {
                            int i = Memory.getValueAt(value);
                            Memory.ACC = Memory.getValueAt(value);
                        }
                        wait = 1;
                        break;
                    case Opcodes.STA:
                        return value;

                    case Opcodes.ADD:
                        if (immediate)
                        {
                            ALU.Add(value);
                        }
                        else
                        {
                            ALU.Add(Memory.getValueAt(value));
                        }
                        wait = 2;
                        break;
                    case Opcodes.SUB:
                        if (immediate)
                        {
                            ALU.Sub(value);
                        }
                        else
                        {
                            ALU.Sub(Memory.getValueAt(value));
                        }
                        wait = 2;
                        break;
                    case Opcodes.AND:
                        if (immediate)
                        {
                            ALU.And(value);
                        }
                        else
                        {
                            ALU.And(Memory.getValueAt(value));
                        }

                        break;
                    case Opcodes.OR:
                        if (immediate)
                        {
                            ALU.Or(value);
                        }
                        else
                        {
                            ALU.Or(Memory.getValueAt(value));
                        }
                        break;
                    case Opcodes.NOTA:
                        ALU.Nor();
                        break;
                    case Opcodes.BA:
                        Memory.PC = value - 1;
                        wait = 1;
                        pastBranches.Add(value, true);
                        throw new BranchException();
                        //TODO see if branch prediction was successful and handle it
                    case Opcodes.BE:
                        if (Memory.CC == 0)
                        {
                            Memory.PC = value - 1;
                            wait = 1;
                            throw new BranchException();
                        }
                        pastBranches.Add(value, Memory.CC == 0);
                        break;
                    case Opcodes.BL:
                        if (Memory.CC < 0)
                        {
                            Memory.PC = value - 1;
                            wait = 1;
                            throw new BranchException();
                        }
                        pastBranches.Add(value, Memory.CC < 0);
                        break;
                    case Opcodes.BG:
                        if (Memory.CC > 0)
                        {
                            Memory.PC = value - 1;
                            wait = 1;
                            throw new BranchException();
                        }
                        pastBranches.Add(value, Memory.CC > 0);
                        break;
                    case Opcodes.NOP:
                        ALU.Add(0);
                        break;
                    case Opcodes.HLT:
                        break;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            throw new DidntException();
        }

        /**
         * Pipelined function for executing an array of raw instructions
         */

        internal static void executeAll(int[] packedInstructions)
        {
            instructions[1] = -1;


            while (Memory.PC <= packedInstructions.Length + 5)
            {
                if (hasBranched)
                {
                    instructions = new int[2];
                    instructions[0] = -1;
                    instructions[1] = -1;
                    opcode = -1;
                    opcode2 = -1;
                    immediate = false;
                    immediate2 = false;
                    value = 0;
                    value2 = 0;
                    hasBranched = false;
                }
                //Fetch
                var t1 = new Thread
                    (delegate()
                    {
                        if (Memory.PC < packedInstructions.Length)
                            instructions[0] = packedInstructions[Memory.PC];
                        else
                            finishSetting = 2;
                    });


                //Decode 
                var t2 = new Thread
                    (delegate()
                    {
                        if (instructions != null && instructions[1] != -1)
                        {
                            opcode = instructions[1] >> 25;
                            immediate = ((instructions[1] >> 24) & 1) == 1;
                            value = instructions[1] & ((1 << 16) - 1);

                            /**
                             * Branch Prediction
                             */
                            int pastJump = !pastBranches.ContainsKey(value) ? -2 : (pastBranches[value] ? value : -1);
                            //pastJump = -2 if branch has never been used, we can ignore that.
                            //pastJump = -1 if branch was not taken previously
                            //else branch exists and was taken before
                            if (pastJump == -1)
                            {
                                predictedJump = Memory.PC+1;
                            }
                            else if (pastJump > -1)
                            {
                               predictedJump = value - 1;
                            }
                        }
                    });

                //execute
                var t3 = new Thread
                    (delegate()
                    {
                        if (opcode2 != -1)
                        {
                            try
                            {
                                Console.WriteLine("{0}:{1}:{2}", opcode2, immediate2, value2);

                                shouldReturn = true;
                                storeValA = execute(opcode2, immediate2, value2);
                            }
                            catch (DidntException e)
                            {
                                shouldReturn = false;
                            }
                            catch (BranchException e)
                            {
                                hasBranched = true;
                            }
                            Console.WriteLine(Memory.getStackAt(10));
                            Console.WriteLine(Memory.ACC);
                        }
                    });
                //Store
                var t4 = new Thread
                    (delegate()
                    {
                        if (returnValB)
                        {
                            Memory.setValueAt(storeValB, acclast);
                        }
                    });
                if (wait == 0)
                {
                    if (finishSetting <= 1)
                        t1.Start();
                    if (finishSetting <= 2)
                        t2.Start();
                    if (finishSetting <= 3)
                        t3.Start();
                    if (finishSetting <= 4)
                        t4.Start();

                    while (t1.IsAlive || t2.IsAlive || t3.IsAlive || t4.IsAlive)
                    {
                        //go get coffee
                    }
                    returnValB = shouldReturn;
                    value2 = value;
                    immediate2 = immediate;
                    opcode2 = opcode;
                    storeValB = storeValA;
                    acclast = Memory.ACC;
                    Memory.PC++;
                    if (predictedJump > -1)
                    {
                        Memory.PC = predictedJump;
                        predictedJump = -1;
                    }
                    instructions[1] = instructions[0];
                    if (finishSetting != 0)
                    {
                        finishSetting++;
                    }
                }
                else
                {
                    try
                    {
                        execute(Opcodes.NOP, false, 0); //NOP
                    }
                    catch (DidntException e)
                    {
                    }
                    wait--;
                    waitCount++;
                    if (wait == 0)
                    {
                        //instructions[1] = instructions[0];
                    }
                }
            }
        }
    }
}