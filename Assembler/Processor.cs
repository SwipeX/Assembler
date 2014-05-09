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
        public static int waitCount = 0;
        private static bool returnValB;
        private static readonly Dictionary<int, int> pastBranches = new Dictionary<int, int>();
        private static int predictedJump = -1; //-1 for none, else the line #
        private static byte predictLoad;
        public static int THRESHOLD = 1;

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
                            // int i = Memory.getValueAt(value);
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
                        if (predictedJump > -1 && predictedJump - 1 != Memory.PC)
                        {
                            wait = 3;
                            hasBranched = true;
                        }
                        predictLoad = 0;
                        predictedJump = -1;
                        add(value, 1);
                        throw new BranchException();
                    case Opcodes.BE:
                        if (Memory.CC == 0)
                        {
                            Memory.PC = value - 1;
                            wait = 1;
                            if (predictedJump > -1 && predictedJump - 1 != Memory.PC)
                            {
                                wait = 3;
                                hasBranched = true;
                            }
                            throw new BranchException();
                        }
                        predictedJump = -1;
                        predictLoad = 0;
                        add(value, (Memory.CC == 0 ? 1 : 0));
                        break;
                    case Opcodes.BL:
                        if (Memory.CC < 0)
                        {
                            Memory.PC = value - 1;
                            wait = 1;
                            if (predictedJump > -1 && predictedJump - 1 != Memory.PC)
                            {
                                wait = 3;
                                hasBranched = true;
                            }
                            throw new BranchException();
                        }
                        predictedJump = -1;
                        predictLoad = 0;
                        add(value, (Memory.CC < 0 ? 1 : 0));
                        break;
                    case Opcodes.BG:
                        if (Memory.CC > 0)
                        {
                            Memory.PC = value - 1;
                            wait = 1;
                            if (predictedJump > -1 && predictedJump - 1 != Memory.PC)
                            {
                                wait = 3;
                                predictedJump = -1;
                                hasBranched = true;
                            }
                            throw new BranchException();
                        }
                        predictedJump = -1;
                        predictLoad = 0;
                        add(value, (Memory.CC > 0 ? 1 : 0));
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

        public static void put(int key, int val)
        {
            if (pastBranches.ContainsKey(key))
                pastBranches[key] = val;
            else
            {
                pastBranches.Add(key, val);
            }
        }

        public static void add(int key, int amount)
        {
            put(get(key), amount);
        }

        public static int get(int key)
        {
            if (pastBranches.ContainsKey(key))
                return pastBranches[key];
            pastBranches.Add(key, 0);
            return get(key);
        }

        /**
         * Pipelined function for executing an array of raw instructions
         */

        internal static void executeAll(int[] packedInstructions)
        {
            instructions[1] = -1;


            while (Memory.PC <= packedInstructions.Length + 4)
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
                        {
                            if (predictedJump <= 0)
                            {
                                instructions[0] = packedInstructions[Memory.PC];
                            }
                            else
                            {
                                instructions[0] = packedInstructions[predictedJump + predictLoad];
                                predictLoad++;
                            }
                        }
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
                            int pastJump = !pastBranches.ContainsKey(value) ? -2 : (pastBranches[value] > 1 ? value : 1);
                            //pastJump = -2 if branch has never been used, we can ignore that.
                            //pastJump = how many times branch has been taken                            
                            if (pastJump <= 0)
                            {
                                //this is essentially the same as doing nothing,
                                //so it is slightly more efficient to not even set it.
                                //predictedJump = Memory.PC;
                            }
                            else if (pastJump > THRESHOLD)
                            {
                                predictedJump = value;
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
                                shouldReturn = false;
                            }
                            Console.WriteLine(Memory.getStackAt(10));
                            //Console.WriteLine(Memory.getValueAt(10));
                            Console.WriteLine(Memory.ACC);
                        }
                    });
                //Store
                var t4 = new Thread
                    (delegate()
                    {
                        if (returnValB)
                        {
                            Memory.setValueAt(storeValB, Memory.ACC2); //handle data hazard
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
                    Memory.ACC2 = Memory.ACC; //handle data hazard
                    Memory.PC++;
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

                if (Memory.PC == packedInstructions.Length + 4)
                {
                    Memory.PC = packedInstructions.Length;
                    break;
                }
            }
        }
    }
}
