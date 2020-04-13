using Compiler.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        private ExpressionSet GetFollow(List<NonTerminalExpressionDefinition> visited = null)
        {
            visited = visited ?? new List<NonTerminalExpressionDefinition>();

            if (visited.Any(x => x.IsEqualTo(this)))
            {
                return new ExpressionSet();
            }

            visited.Add(this);

            ExpressionSet result = new ExpressionSet();

            foreach (Production production in Grammar.Instance)
            {
                if (production.Identifier == Identifier)
                {
                    continue;
                }

                foreach (SubProduction subProduction in production)
                {
                    bool found = false;

                    foreach (ExpressionDefinition expression in subProduction.Where(x => !(x is SemanticActionDefinition)))
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
                                    && ((NonTerminalExpressionDefinition)expression).Identifier != Identifier)
                                {
                                    result.AddRangeUnique(new NonTerminalExpressionDefinition { Identifier = production.Identifier }.GetFollow(visited));
                                }

                                result.AddRangeUnique(new ExpressionSet(expression.First().Where(x => x.TokenType != TokenType.EmptyString).ToList()));
                                found = false;
                            }
                        }
                    }

                    // when last
                    if (found)
                    {
                        result.AddRangeUnique(new NonTerminalExpressionDefinition { Identifier = production.Identifier }.GetFollow(visited));
                    }
                }
            }

            // when last
            if (Identifier == "Initial")
            {
                result.Add(new TerminalExpressionDefinition { TokenType = TokenType.EndMarker });
            }

            return result;
        }

        public override ExpressionSet First()
        {
            ExpressionSet result = new ExpressionSet();

            if (Grammar.Instance.Any(x => x.Identifier == Identifier
                && x.Any(y => y.Count == 1
                && y.First() is TerminalExpressionDefinition te
                && te.TokenType == TokenType.EmptyString)))
            {
                result.Add(new TerminalExpressionDefinition { TokenType = TokenType.EmptyString });
            }

            foreach (SubProduction subProduction in Grammar.Instance.First(x => x.Identifier == Identifier))
            {
                bool canContinue = true;

                foreach (ExpressionDefinition expression in subProduction.Where(x => !(x is SemanticActionDefinition)))
                {
                    if (expression.IsEqualTo(this))
                    {
                        continue;
                    }


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

        public override bool IsEqualTo(ExpressionDefinition definition)
        {
            if (definition is NonTerminalExpressionDefinition nte)
            {
                return Identifier == nte.Identifier;
            }

            return false;
        }
    }
}