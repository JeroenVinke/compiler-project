using Compiler.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Compiler.RegularExpressionEngine
{
    public class RegexToSubExpressionConverter
    {
        private List<Token> Tokens { get; set; }
        private Token Peek { get { return Finished ? null : Tokens[_currentToken]; } }
        private int _currentToken { get; set; } = 0;
        private bool Finished => Tokens.Count <= _currentToken;

        public List<List<char>> Ranges = new List<List<char>>
        {
            new List<char> { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x','y','z'},
            new List<char> { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X','Y','Z'},
            new List<char> { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' }
        };

        private Token Next()
        {
            Token p = Peek;
            _currentToken++;
            return p;
        }

        private Token Next(char token)
        {
            if (Peek.Character == token)
            {
                Token p = Peek;
                _currentToken++;
                return p;
            }

            throw new Exception($"Expected the next character to be '{token}' but was '{Peek.Character}'");
        }

        private RegexToSubExpressionConverter()
        {
        }

        public static SubExpression Convert(string regex)
        {
            RegexToSubExpressionConverter converter = new RegexToSubExpressionConverter();
            return converter.DoConvert(regex);
        }

        private SubExpression DoConvert(string regex)
        {
            Tokens = RegexTokenizer.Tokenize(regex);
            SubExpression subExpression = And();

            return subExpression;
        }

        private SubExpression And()
        {
            SubExpression expr = Or();

            while (Peek != null)
            {
                expr = new AndExpression(new List<SubExpression> { expr, Or() });
            }

            return expr;
        }

        private SubExpression Or()
        {
            SubExpression expr = Star();
            List<SubExpression> subExpressions = new List<SubExpression>();

            while (Peek?.Character == '|')
            {
                Next();
                subExpressions.Add(Star());
            }

            if (subExpressions.Count > 0)
            {
                subExpressions.Insert(0, expr);
                expr = new OrExpression(subExpressions);
            }

            return expr;
        }

        private SubExpression Star()
        {
            SubExpression expr = Plus();

            if(Peek?.Character == '*')
            {
                Next();
                expr = new RepeatedExpression(expr, '*');
            }

            return expr;
        }

        private SubExpression Plus()
        {
            SubExpression expr = Factor();

            if (Peek?.Character == '+')
            {
                Next();
                expr = new RepeatedExpression(expr, '+');
            }

            return expr;
        }

        private SubExpression Factor()
        {
            Token peek = Peek;

            if (Peek is WordToken word)
            {
                SubExpression sub = null;

                foreach (char c in word.Lexeme)
                {
                    if (sub == null)
                    {
                         sub = new SingleCharacterExpression(c);
                    }
                    else
                    {
                        sub = new AndExpression(new List<SubExpression> { sub, new SingleCharacterExpression(c) });
                    }
                }

                Next();

                return sub;
            }
            else if (peek.Character == '(')
            {
                Next();
                SubExpression sub = Or();
                Next(')');
                return sub;
            }
            else if (peek.Character == '[')
            {
                Next();

                WordToken token = ((WordToken)Peek);
                char rangeStart = '\0';
                List<char> unionedRange = new List<char>();
                foreach (char c in token.Lexeme)
                {
                    if (rangeStart == '\0')
                    {
                        rangeStart = c;
                    }
                    else if (c != '-')
                    {
                        List<char> range = Ranges.First(x => x.Contains(rangeStart) && x.Contains(c));

                        for (int i = range.IndexOf(rangeStart); i < range.IndexOf(c) + 1; i++)
                        {
                            unionedRange.Add(range[i]);
                        }

                        rangeStart = '\0';
                    }
                }

                Next();
                Next(']');

                List<SubExpression> subExpressions = new List<SubExpression>();

                foreach(char c in unionedRange)
                {
                    subExpressions.Add(new SingleCharacterExpression(c));
                }

                return new OrExpression(subExpressions);
            }
            else
            {
                SubExpression sub = new SingleCharacterExpression(peek.Character);
                Next();
                return sub;
            }
        }
    }
}
