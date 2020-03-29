using Compiler.Common;
using System.Collections.Generic;

namespace Compiler.RegularExpressionEngine
{
    public class RegexTokenizer
    {
        private int CurrentIndex { get; set; }
        public string Regex { get; set; }
        public char Peek
        {
            get
            {
                if (Regex.Length > CurrentIndex)
                {
                    return Regex[CurrentIndex];
                }
                return '\0';
            }
        }
        public char Lookahead
        {
            get
            {
                if (Regex.Length > CurrentIndex + 1)
                {
                    return Regex[CurrentIndex + 1];
                }
                return '\0';
            }
        }

        public bool Finished => Regex.Length <= CurrentIndex;

        private RegexTokenizer()
        {
        }

        public static List<Token> Tokenize(string regex)
        {
            RegexTokenizer tokenizer = new RegexTokenizer();
            return tokenizer.DoTokenize(regex);
        }

        private List<Token> DoTokenize(string regex)
        {
            Regex = regex;

            List<Token> tokens = new List<Token>();
            string buffer = "";

            while (!Finished)
            {
                char peek = Peek;

                if (peek == '\\' || !IsSpecialCharacter(peek))
                {
                    if (peek == '\\')
                    {
                        Next();
                        buffer += Peek;
                        Next();
                        continue;
                    }

                    buffer += Peek;
                    Next();
                }
                else 
                {
                    if (!string.IsNullOrEmpty(buffer))
                    {
                        tokens.Add(new WordToken { Lexeme = buffer });
                        buffer = "";
                    }

                    if (peek != '\0')
                    {
                        Next();
                        tokens.Add(new Token { Character = peek });
                    }
                }
            }

            if (!string.IsNullOrEmpty(buffer))
            {
                tokens.Add(new WordToken { Lexeme = buffer });
                buffer = "";
            }

            return tokens;
        }

        private bool IsSpecialCharacter(char c)
        {
            return "()*+|[]#".Contains(c.ToString().ToLower());
        }

        private void Next()
        {
            CurrentIndex++;
        }
    }
}
