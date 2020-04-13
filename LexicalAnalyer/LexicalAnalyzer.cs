using Compiler;
using Compiler.Common;
using Compiler.RegularExpressionEngine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Compiler.LexicalAnalyer
{
    public class LexicalAnalyzer
    {
        public Dictionary<string, Func<string, Token>> Language { get; set; }
        public string SourceText { get; private set; }
        public List<Token> ReadTokens { get; set; } = new List<Token>();

        public LexicalAnalyzer(Dictionary<string, Func<string, Token>> language, string source)
        {
            Language = language;
            SourceText = source;
        }

        public Token GetNextToken()
        {
            List<SimulationResult> results = ResetResults();
            int i = 0;
            string buffer = "";
            while (SourceText.Length > i)
            {
                buffer += SourceText[i];

                foreach (SimulationResult result in results.Where(x => x.IsCandidate))
                {
                    // todo: don't start simulation all over again
                    SimulationState state = RegexEngine.Simulate(result.Regex, buffer);

                    if (state != SimulationState.OffTrack)
                    {
                        result.MatchedLength++;
                        result.ExactMatch = result.ExactMatch || state == SimulationState.Accepting;
                        result.Accepted = state == SimulationState.Accepting;
                    }
                    else
                    {
                        result.IsCandidate = false;
                    }
                }

                if (!results.Any(x => x.IsCandidate) || SourceText.Length == buffer.Length)
                {
                    if (!results.Any(x => x.MatchedLength > 0 && x.Accepted))
                    {
                        throw new Exception("No matches");
                    }

                    if (!results.Any(x => x.IsCandidate))
                    {
                        buffer = buffer.Substring(0, buffer.Length - 1);
                    }

                    Token token = results
                        .OrderByDescending(x => x.ExactMatch)
                        .ThenByDescending(x => x.MatchedLength).First().Action(buffer);
                    if (token.Type != TokenType.Nothing)
                    {
                        if (token is WordToken wordToken)
                        {
                            wordToken.Lexeme = buffer;
                        }

                        SourceText = SourceText.Substring(buffer.Length);
                        ReadTokens.Add(token);
                        return token;
                    }
                    else
                    {
                        SourceText = SourceText.Substring(i);
                        return GetNextToken();
                    }
                }

                i++;
            }

            if (SourceText.Length == 0)
            {
                ReadTokens.Add(new EndOfFileToken());
                return new EndOfFileToken();
            }

            return null;
        }

        private List<SimulationResult> ResetResults()
        {
            List<SimulationResult> results = new List<SimulationResult>();
            foreach (KeyValuePair<string, Func<string, Token>> construct in Language)
            {
                results.Add(new SimulationResult
                {
                    Regex = construct.Key,
                    MatchedLength = 0,
                    Action = construct.Value
                });
            }

            return results;
        }
    }
}
