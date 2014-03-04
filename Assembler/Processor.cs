namespace Assembler
{
    internal class Processor
    {
        public static void Add(int value)
        {
            Memory.ACC += value;
        }

        public static void Sub(int value)
        {
            Memory.ACC -= value;
        }

        public static void Mul(int value)
        {
            Memory.ACC *= value;
        }

        public static void Div(int value)
        {
            Memory.ACC /= value;
        }

        public static void And(int value)
        {
            Memory.ACC &= value;
        }

        public static void Or(int value)
        {
            Memory.ACC |= value;
        }
    }
}