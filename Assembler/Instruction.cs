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

       public Instruction(int opcode, int value, int index)
       {
           this._opcode = opcode;
           this._value = value;
           this._index = index;
       }

       public int GetOpcode()
       {
           return _opcode;
       }
       public int GetIndex()
       {
           return _index;
       }

       public int GetValue()
       {
           return _value;
       }
   }
}
