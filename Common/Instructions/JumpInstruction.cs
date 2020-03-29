namespace Compiler.Parser.Common
{
    public class JumpInstruction : Instruction
    {
        public Label Label { get; set;  }

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
