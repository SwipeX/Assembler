namespace Assembler
{
    public class Opcodes
    {
        //to get string for instruction do: NAMES[LDA];
        public const int LDA = 0x1;
        public const int STA = 0x2;
        public const int ADD = 0x2;
        public const int SUB = 0x3;
        public const int AND = 0x4;
        public const int OR = 0x5;
        public const int NOTA = 0x6;
        public const int BA = 0x7;
        public const int BE = 0x8;
        public const int BL = 0x9;
        public const int BG = 0x10;
        public const int NOP = 0x11;
        public const int HLT = 0x12;
        public static string[] NAMES =
        {
            "LDA",
            "STA",
            "ADD",
            "SUB",
            "AND",
            "OR",
            "NOT",
            "BA",
            "BE",
            "BL",
            "BG",
            "NOP",
            "HLT"
        };
    }
}