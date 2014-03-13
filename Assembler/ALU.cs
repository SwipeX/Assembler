namespace Assembler
{
    internal class ALU
    {
        public static void Add(int value)
        {
            Memory.ACC += value;
            Memory.CC = Memory.ACC == 0? 0 : ((Memory.ACC > 0) ? 1 : -1);
        }

        public static void Sub(int value)
        {
            Memory.ACC -= value;
            Memory.CC = Memory.ACC == 0 ? 0 : ((Memory.ACC > 0) ? 1 : -1);
        }

        public static void Mul(int value)
        {
            Memory.ACC *= value;
            Memory.CC = Memory.ACC == 0 ? 0 : ((Memory.ACC > 0) ? 1 : -1);
        }

        public static void Div(int value)
        {
            Memory.ACC /= value;
            Memory.CC = Memory.ACC == 0 ? 0 : ((Memory.ACC > 0) ? 1 : -1);
        }

        public static void And(int value)
        {
            Memory.ACC &= value;
            Memory.CC = Memory.ACC == 0 ? 0 : ((Memory.ACC > 0) ? 1 : -1);
        }

        public static void Or(int value)
        {
            Memory.ACC |= value;
            Memory.CC = Memory.ACC == 0 ? 0 : ((Memory.ACC > 0) ? 1 : -1);
        }

        internal static void Nor()
        {
            Memory.ACC ^= Memory.ACC;
            Memory.CC = Memory.ACC == 0 ? 0 : ((Memory.ACC > 0) ? 1 : -1);
        }
    }
}