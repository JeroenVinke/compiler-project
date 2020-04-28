namespace Compiler.Common.Instructions
{
    public class CallInstruction : Instruction
    {
        public Label Label { get; set;  }

        public CallInstruction(Label label)
        {
            Label = label;
        }

        public override string GenerateCodeString()
        {
            return $"call {Label.ToString()}";
        }
    }
}
