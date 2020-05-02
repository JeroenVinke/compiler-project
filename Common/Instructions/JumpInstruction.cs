namespace Compiler.Common.Instructions
{
    public class JumpInstruction : Instruction
    {
        public Label Label { get; set;  }
        public Block TargetBlock { get; set; }

        public JumpInstruction(Label label)
        {
            Label = label;
        }

        public override string GenerateCodeString()
        {
            return $"goto {Label.ToString()}";
        }
    }
}
