namespace Compiler.Common
{
    public class Label
    {
        public static int MaxLabelId = 0;
        public int LabelId = 0;

        public Label()
        {
            LabelId = ++MaxLabelId;
        }

        public override string ToString()
        {
            return "L" + LabelId;
        }
    }
}
