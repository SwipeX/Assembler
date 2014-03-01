using System.Data;

namespace Assembler
{
    internal class Memory
    {
        public static int A, B, ACC, ZERO, ONE, PC, MAR, MDR, TEMP, IR, CC;

        private static readonly string[] RegisterNames =
        {
            "A", "B", "ACC", "ZERO", "ONE", "PC", "MAR", "MDR", "TEMP",
            "IR", "CC"
        };

        private static readonly int[] Stack = new int[256];

        public static int getValueAt(int index)
        {
            if (index >= 0 && index < Stack.Length)
                return Stack[index];
            return -1;
        }

        public void setValueAt(int index, int value)
        {
            if (index >= 0 && index < Stack.Length)
                Stack[index] = value;
        }

        internal static object getValues()
        {
            int[] RegisterValues =
            {
                A, B, ACC, ZERO, ONE, PC, MAR, MDR, TEMP, IR, CC
            };
            var table = new DataTable();
            table.Columns.Add("Register");
            table.Columns.Add("Value");
            for (int i = 0; i < RegisterNames.Length; i++)
            {
                table.Rows.Add(new object[] {RegisterNames[i], RegisterValues[i]});
            }
            return table;
        }
    }
}