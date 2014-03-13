using System;

namespace Assembler
{
    public class Processor
    {
        public static void execute(int instruction)
        {
            try
            {
                int opcode = instruction >> 25;
                bool immediate = ((instruction >> 24) & 1) == 1; // ((instruction ^ (opcode << 25)) >> 24) == 1;
                int value = instruction & ((1 << 16) - 1);
//                if (immediate)
//                {
//                    value = ((instruction ^ ((opcode << 25) | (1 << 24))));
//                }
//                else
//                {
//                    value = ((instruction ^ ((opcode << 25))));
//                }
                switch (opcode)
                {
                    case Opcodes.LDA:
                        if (immediate)
                        {
                            Memory.ACC = value;
                        }
                        else
                        {
                            Memory.ACC = Memory.getValueAt(value);
                        }
                        break;
                    case Opcodes.STA:
                        Memory.setValueAt(value, Memory.ACC);
                        break;
                    case Opcodes.ADD:
                        if (immediate)
                        {
                            ALU.Add(value);
                        }
                        else
                        {
                            ALU.Add(Memory.getValueAt(value));
                        }
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
                        break;
                    case Opcodes.BE:
                        if (Memory.CC == 0)
                            Memory.PC = value - 1;
                        break;
                    case Opcodes.BL:
                        if (Memory.CC < 0)
                            Memory.PC = value - 1;
                        break;
                    case Opcodes.BG:
                        if (Memory.CC > 0)
                            Memory.PC = value - 1;
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
        }

        internal static void executeAll(int[] packedInstructions)
        {
            while (Memory.PC < packedInstructions.Length)
            {
                execute(packedInstructions[Memory.PC]);
                Memory.PC++;
            }
        }
    }
}