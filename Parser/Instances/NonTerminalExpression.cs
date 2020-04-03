namespace Compiler.Parser
{
    public class NonTerminalExpression : Expression
    {
        private string identifier;

        public string Identifier
        {
            get => identifier;
            set
            {
                identifier = value;
                if (string.IsNullOrEmpty(Key))
                {
                    Key = value;
                }
            }
        }

        public override string ToString()
        {
            return Identifier + "(" + Key + ")";
        }
    }
}