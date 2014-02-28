using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assembler
{
   public class CodeReader
    {
       public static LinkedList<String> ReadAll(String fileName)
       {
        var text = new LinkedList<String>();
        foreach (var line in File.ReadAllLines(fileName))
            text.AddLast(line);
        return text;
       }
    }
}
