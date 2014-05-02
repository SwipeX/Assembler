using System;
using System.Threading;

namespace Assembler
{
    public class Processor
    {
       static int[] instructions = new int[2];
        public static int execute(int opcode, bool immediate, int value)
        {
            try
            {
                /*
                int opcode = instruction >> 25;
                bool immediate = ((instruction >> 24) & 1) == 1; // ((instruction ^ (opcode << 25)) >> 24) == 1;
                int value = instruction & ((1 << 16) - 1);
                 * */
//                if (immediate)
//                {
//                    value = ((instruction ^ ((opcode << 25) | (1 << 24))));
//                }
//                else
//                {
//                    value = ((instruction ^ ((opcode << 25))));
//                }
                //throw new NullReferenceException();
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
                        throw new BranchException();
                    case Opcodes.BE:
                        if (Memory.CC == 0)
                        {
                            Memory.PC = value - 1;
                            wait = 1;
                            throw new BranchException();
                        }
                        break;
                    case Opcodes.BL:
                        if (Memory.CC < 0) { 
                            Memory.PC = value - 1;
                        wait = 1;
                        throw new BranchException();
                }
                        break;
                    case Opcodes.BG:
                        if (Memory.CC > 0)
                        {
                            Memory.PC = value - 1;
                            wait = 1;
                            throw new BranchException();
                        }
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
        static int opcode = -1;
        static int opcode2 = -1;
        static bool immediate = false;
        static bool immediate2 = false;
        static int value = 0;
        static int value2 = 0;
        static bool returnstuff = true;
        static int storeval = 0;
        static int storeval2 = 0;
        static bool hasBranched;
        static byte isFinished = 0;
        static byte wait = 0;
        static int acclast;
     public  static  int waitCount = 0;
        static bool returnstuffpart2=false;
        internal static void executeAll(int[] packedInstructions)
        {
            instructions[1] = -1;

            
            while (Memory.PC <= packedInstructions.Length+5)
            {
                if(hasBranched){
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
                System.Threading.Thread t1 = new System.Threading.Thread
                (delegate()
                {
                    if (Memory.PC < packedInstructions.Length)
                        instructions[0] = packedInstructions[Memory.PC];
                    else
                        isFinished = 2;
                 
                 });
             

                //Decode 
                 System.Threading.Thread t2 = new System.Threading.Thread
                (delegate()
                {
                if (instructions != null && instructions[1] != -1) {
                   opcode = instructions[1] >> 25;
                   immediate = ((instructions[1] >> 24) & 1) == 1; 
                   value = instructions[1] & ((1 << 16) - 1);
                }
                });
                
                //execute
                 System.Threading.Thread t3 = new System.Threading.Thread
               (delegate()
               {
                   if (opcode2 != -1) { 
                   try
                   {
                       Console.WriteLine(opcode2+":"+immediate2+":"+value2);
                       
                       returnstuff = true;
                       storeval = execute(opcode2, immediate2, value2);
                       
                   }
                   catch (DidntException e)
                   {
                       returnstuff = false;
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
                 System.Threading.Thread t4 = new System.Threading.Thread
               (delegate()
               {
                   if (returnstuffpart2)
                   {
                       Memory.setValueAt(storeval2, acclast);
                   }
               });
                 if (wait == 0) { 
                 if (isFinished <= 1)                 
                     t1.Start();
                 if (isFinished <= 2)
                t2.Start();
                 if (isFinished <= 3)
                t3.Start();
                 if (isFinished <= 4)
                t4.Start();

                while (t1.IsAlive || t2.IsAlive || t3.IsAlive || t4.IsAlive)
                {
                    //go get coffee
                }
                returnstuffpart2 = returnstuff;
                value2 = value;
                immediate2 = immediate;
                opcode2 = opcode;
                storeval2 = storeval;
                acclast = Memory.ACC;
                
                //if (Memory.PC <= packedInstructions.Length) { 
                    Memory.PC++;
                // }
                
                    instructions[1] = instructions[0];
                 
                if (isFinished != 0)
                {
                    isFinished++;
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