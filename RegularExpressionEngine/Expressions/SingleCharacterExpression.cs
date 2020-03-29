using Compiler.Common;

namespace Compiler.RegularExpressionEngine
{
    internal class SingleCharacterExpression : SubExpression
    {
        public char Character { get; set; }

        public SingleCharacterExpression(char c)
        {
            Character = c;
        }

        public override string ToString()
        {
            return Character + "";
        }
    }
}
