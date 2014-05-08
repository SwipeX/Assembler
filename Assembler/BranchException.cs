using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assembler
{
    class BranchException : Exception
    {
        
	public BranchException()
	{

	}
    public BranchException(string message)
        : base(message){
    }
    public BranchException(string message, Exception inner)
        : base(message, inner){
    }
    }
}
