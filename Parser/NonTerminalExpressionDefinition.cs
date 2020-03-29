﻿using Compiler.Common;
using System.Linq;

namespace Compiler.Parser
{
    public class NonTerminalExpressionDefinition : ExpressionDefinition
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

        public ExpressionSet Follow()
        {
            ExpressionSet result = new ExpressionSet();
            result.AddRangeUnique(GetFollow());
            return result;
        }

        private ExpressionSet GetFollow()
        {
            ExpressionSet result = new ExpressionSet();

            foreach (Production production in Parser.Grammar)
            {
                foreach (SubProduction subProduction in production)
                {
                    bool found = false;

                    foreach (ExpressionDefinition expression in subProduction.Where(x => !(x is SemanticAction)))
                    {
                        if (expression is NonTerminalExpressionDefinition ne && ne.Identifier == Identifier)
                        {
                            found = true;
                        }
                        else

                        {
                            if (found)
                            {
                                if (expression.First().Any(x => x.TokenType == TokenType.EmptyString)
                                    && ((NonTerminalExpressionDefinition)expression).Identifier != production.Identifier)
                                {
                                    result.AddRangeUnique(new NonTerminalExpressionDefinition { Identifier = production.Identifier }.GetFollow());
                                }

                                result.AddRangeUnique(new ExpressionSet(expression.First().Where(x => x.TokenType != TokenType.EmptyString).ToList()));
                                found = false;
                            }
                        }
                    }

                    // when last
                    if (found && production.Identifier != Identifier)
                    {
                        result.AddRangeUnique(new NonTerminalExpressionDefinition { Identifier = production.Identifier }.GetFollow());
                    }

                    // when last
                    if (found && production.Identifier == Identifier)
                    {
                        result.AddRangeUnique(new ExpressionSet { new TerminalExpressionDefinition { TokenType = TokenType.EndMarker } });
                    }
                }
            }

            return result;
        }

        public override ExpressionSet First()
        {
            ExpressionSet result = new ExpressionSet();

            if (Parser.Grammar.Any(x => x.Identifier == Identifier
                && x.Any(y => y.Count == 1
                && y.First() is TerminalExpressionDefinition te
                && te.TokenType == TokenType.EmptyString)))
            {
                result.Add(new TerminalExpressionDefinition { TokenType = TokenType.EmptyString });
            }

            foreach (SubProduction subProduction in Parser.Grammar.First(x => x.Identifier == Identifier))
            {
                bool canContinue = true;

                foreach (ExpressionDefinition expression in subProduction.Where(x => !(x is SemanticAction)))
                {
                    if (canContinue)
                    {
                        ExpressionSet first = expression.First();

                        result.AddRangeUnique(first);

                        canContinue = first.Any(x => x.TokenType == TokenType.EmptyString);
                    }
                }
            }

            return result;
        }

        public override string ToString()
        {
            return Identifier;
        }
    }
}