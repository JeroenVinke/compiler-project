namespace Compiler.Common.Instructions
{
    public class ReturnInstruction : Instruction
    {
        public ReturnInstruction()
        {
        }

        public override string GenerateCodeString()
        {
            return $"ret";
        }
    }
}
