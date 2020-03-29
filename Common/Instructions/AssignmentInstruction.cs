using Compiler.Parser.Common;

namespace Compiler.Common.Instructions
{
    public class AssignmentInstruction : Instruction
    {
        public Address Address { get; set; }
        public object Value { get; set; }

        public AssignmentInstruction(Address address, object value)
        {
            Address = address;
            Value = value;
        }

        public override string GenerateCodeString()
        {
            string value;

            if (Value is Address addr)
            {
                value = addr.ToString();
            }
            else if (Value is bool b)
            {
                value = b ? bool.TrueString : bool.FalseString;
            }
            else
            {
                value = Value.ToString();
            }

            return $"{Address.ToString()} = {value}";
        }
    }
}
