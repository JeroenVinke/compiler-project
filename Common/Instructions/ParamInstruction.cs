namespace Compiler.Common.Instructions
{
    public class ParamInstruction : Instruction
    {
        public Address Address { get; set;  }

        public ParamInstruction(Address address)
        {
            Address = address;
        }

        public override string GenerateCodeString()
        {
            return $"param {Address.ToString()}";
        }
    }
}
