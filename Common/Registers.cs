using System.Collections.Generic;

namespace Compiler.Common
{
    public class Registers
    {
        public Register EAX { get; set; } = new Register() { Name = "EAX" };
        public Register EBX { get; set; } = new Register() { Name = "EBX" };
        public Register ECX { get; set; } = new Register() { Name = "ECX" };
        public Register RSP { get; set; } = new Register() { Name = "RSP" };
        public Register RBP { get; set; } = new Register() { Name = "RBP" };

        private List<Register> _all;
        public List<Register> All
        {
            get
            {
                if (_all == null)
                {
                    _all = new List<Register>
                    {
                        EAX, EBX, ECX
                    };
                }

                return _all;
            }
        }
    }
}
