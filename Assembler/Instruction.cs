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

       public int GetOpcode()
       {
           return _opcode;
       }
       public bool GetFlag()
       {
           return _flag;
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
