using System.Collections.Generic;

namespace Compiler.Common
{
    public static class Registers
    {
        public static Register EAX { get; set; } = new Register() { Name = "EAX" };
        public static Register EBX { get; set; } = new Register() { Name = "EBX" };
        public static Register ECX { get; set; } = new Register() { Name = "ECX" };
        public static List<Register> All { get; } = new List<Register>
        {
            EAX, EBX, ECX
        };
    }
}
