namespace Compiler.Common.Instructions
{
    public class CallInstruction : JumpInstruction
    {
        public CallInstruction(Label label) : base(label)
        {
        }

        public override string GenerateCodeString()
        {
            return $"call {Label.ToString()}";
        }
    }
}
