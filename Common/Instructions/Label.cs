namespace Compiler.Parser.Common
{
    public class Label : Instruction
    {
        public static int MaxLabelId = 0;
        public int LabelId = 0;

        public Label()
        {
            LabelId = ++MaxLabelId;
        }

        public override string GenerateCodeString()
        {
            return $"label L{LabelId}:";
        }

        public override string ToString()
        {
            return $"label L{LabelId}";
        }
    }
}
