using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assembler
{
    class DidntException : Exception
    {

        public DidntException()
        {

        }
        public DidntException(string message)
            : base(message)
        {
        }
        public DidntException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
