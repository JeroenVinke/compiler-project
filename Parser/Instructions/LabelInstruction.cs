using Compiler.Parser.Common;

namespace Compiler.Parser.Instructions
{
    public class LabelInstruction : Instruction
    {
        public Label Label { get; set; }

        public LabelInstruction(Label label)
        {
            Label = label;
        }

        public override string GenerateCodeString()
        {
            return $"label {Label.ToString()}:";
        }

        public override string ToString()
        {
            return $"label {Label.ToString()}";
        }
    }
}
