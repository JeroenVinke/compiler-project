using Compiler.Common;

namespace Compiler.LexicalAnalyer
{
    public class EndOfFileToken : Token
    {
        public EndOfFileToken()
        {
            Type = TokenType.EndMarker;
        }

        public override string ToString()
        {
            return "<EndMarker>";
        }
    }
}