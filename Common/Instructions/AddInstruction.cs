namespace Compiler.Common.Instructions
{
    public class AddInstruction : Instruction
    {
        public AddInstruction(Address address1, Address address2, Address result) : base(address1, address2, result)
        {
        }

        public override string GenerateCodeString()
        {
            return $"{Address3.ToString()} = {Address1.ToString()} + {Address2}";
        }
    }
}
