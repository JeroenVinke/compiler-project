namespace Compiler.Common.Instructions
{
    public class ReturnInstruction : Instruction
    {
        public ReturnInstruction(Address returnValue) : base(returnValue)
        {
        }

        public override string GenerateCodeString()
        {
            return $"ret {Address1.ToString()}";
        }
    }
}
