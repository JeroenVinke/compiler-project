namespace Compiler.Common.Instructions
{
    public class ParamInstruction : Instruction
    {
        public ParamInstruction(Address address) : base(address, null, null)
        {
        }

        public override string GenerateCodeString()
        {
            return $"param {Address1.ToString()}";
        }
    }
}
