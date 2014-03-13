using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assembler
{
    public class Processor
    {
        public static void execute(int instruction)
        {
            int opcode = instruction >> 25;
            bool immediate = (instruction ^ (1 << 24)) == 1;
            int value = (instruction >> (8 * 0)) & 0xff + (instruction >> (8 * 1)) & 0xff;
            switch (opcode)
            {
                case Opcodes.LDA:
                    if (immediate)
                    {
                        Memory.ACC = value;
                    }
                    else
                    {
                      Memory.ACC =  Memory.getValueAt(value);
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
                case Opcodes.OR: if (immediate)
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
                    Memory.PC = value-1;
                    break;
                case Opcodes.BE: 
                    if(Memory.CC == 0)
                        Memory.PC = value-1;break;
                case Opcodes.BL:
                    if(Memory.CC < 0 )
                        Memory.PC = value-1;break;
                case Opcodes.BG: 
                    if(Memory.CC > 0 )
                        Memory.PC = value-1;break;
                case Opcodes.NOP: ALU.Add(0); break;
                case Opcodes.HLT: break;
            }
        }

        internal static void executeAll(int[] thisstuff)
        {
            while(Memory.PC < thisstuff.Length-1)
            {
                execute(thisstuff[Memory.PC]);
                Memory.PC++;
            }
        }
    }
}
