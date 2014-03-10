using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assembler
{  

   public class Instruction
   {
       private readonly int _opcode;
       private readonly int _value;
       private readonly int _index;
       private readonly bool _flag;

       public Instruction(int opcode, int value, int index, bool flag)
       {
           this._opcode = opcode;
           this._value = value;
           this._index = index;
           this._flag = flag;
       }

       public int intValueOfInstruction()
       {
           int intvalue = _opcode << 25;//7 bits for instruction
           if (_flag)
                intvalue += 1 << 24;//1 bit for flag
           intvalue += _value;//value is on the end
           return intvalue;
       }

   }
}
