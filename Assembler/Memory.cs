﻿using System.Data;

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

        private static int[] Stack = new int[256];

        public static int getValueAt(int index)
        {
            if (index >= 0 && index < Stack.Length)
            {
                return Stack[index];
            }
            throw new SegmentationException();
        }

        public static void setValueAt(int index, int value)
        {
            if (index >= 0 && index < Stack.Length)
            {
                Stack[index] = value;
            }
            else
            {
                throw new SegmentationException();
            }
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

        internal static void reset()
        {
            A = 0;
            B = 0;
            ACC = 0;
            ZERO = 0;
            ONE = 0;
            PC = 0;
            MAR = 0;
            MDR = 0;
            TEMP = 0;
            IR = 0;
            CC = 0;
            Stack = new int[256];
        }
    }
}