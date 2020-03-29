using Compiler.Common;

namespace Compiler.RegularExpressionEngine
{
    internal class WordExpression : SubExpression
    {
        public WordToken Word { get; set; }

        public WordExpression(WordToken word)
        {
            this.Word = word;
        }

        public override string ToString()
        {
            return Word.Lexeme;
        }
    }
}
