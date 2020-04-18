using Compiler.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Compiler.Parser.Instances
{
    public class Item : List<ExpressionDefinition>
    {
        public SubProduction SubProduction { get; set; }

        public int DotIndex { get; set; } = 0;
        public List<TerminalExpressionDefinition> Lookahead { get; set; }
        public ExpressionDefinition ExpressionAfterDot => WithoutActions().Count > DotIndex ? WithoutActions().ElementAt(DotIndex) : null;

        public List<ExpressionDefinition> SpontaneousLookaheads { get; set; } = new List<ExpressionDefinition>();
        public List<ExpressionDefinition> PropogatedLookaheads { get; set; } = new List<ExpressionDefinition>();

        private ItemSet _closure = null;

        public Item(List<ExpressionDefinition> expressions, List<TerminalExpressionDefinition> lookahead = null) : base(expressions)
        {
            Lookahead = lookahead ?? new List<TerminalExpressionDefinition>();
        }

        public Item(SubProduction subProduction, List<TerminalExpressionDefinition> lookahead = null)
        {
            SubProduction = subProduction;
            Lookahead = lookahead ?? new List<TerminalExpressionDefinition>();

            foreach (ExpressionDefinition expressionDefinition in subProduction)
            {
                Add(expressionDefinition);
            }
        }

        public void Next()
        {
            DotIndex++;
        }

        public ItemSet Closure()
        {
            if (_closure != null)
            {
                return _closure;
            }

            _closure = new ItemSet();
            _closure.Add(Clone());

            if (ExpressionAfterDot is NonTerminalExpressionDefinition a)
            {
                foreach (Production production in Grammar.Instance)
                {
                    if (production.Identifier == a.Identifier)
                    {
                        foreach (SubProduction subProduction in production)
                        {
                            List<ExpressionDefinition> tail = WithoutActions().Skip(DotIndex).ToList();

                            if (Lookahead != null)
                            {
                                tail.AddRange(Lookahead);
                            }

                            foreach (TerminalExpressionDefinition ted in First(tail))
                            {
                                if (!_closure.Any(x => x.SubProduction == subProduction && x.DotIndex == 0))
                                {
                                    _closure.Add(new Item(subProduction, new List<TerminalExpressionDefinition> { ted }));
                                }
                            }
                        }
                    }
                }
            }

            return _closure;
        }

        public ExpressionSet First(List<ExpressionDefinition> expressionDefinitions)
        {
            ExpressionSet result = new ExpressionSet();

            bool previousCanBeEmpty = true;
            foreach (ExpressionDefinition expressionDefinition in expressionDefinitions)
            {
                if (previousCanBeEmpty)
                {
                    ExpressionSet first = expressionDefinition.First();
                    result.AddRange(first.Where(x => x.TokenType != TokenType.EmptyString));

                    if (!first.Any(x => x.TokenType == TokenType.EmptyString))
                    {
                        previousCanBeEmpty = false;
                        break;
                    }
                }
            }

            if (previousCanBeEmpty)
            {
                result.Add(new TerminalExpressionDefinition { TokenType = TokenType.EmptyString });
            }

            return result;
        }

        internal bool IsEqualTo(Item x, bool ignoreDotIndex = false)
        {
            if (x.Count != Count)
            {
                return false;
            }

            if (!ignoreDotIndex && x.DotIndex != DotIndex)
            {
                return false;
            }

            for (int i = 0; i < Count; i++)
            {
                if (x[i].SubProduction != this[i].SubProduction)
                {
                    return false;
                }
            }

            return true;
        }

        public Item Clone()
        {
            Item item = new Item(SubProduction, Lookahead);
            item.DotIndex = DotIndex;
            return item;
        }

        public string ToDot()
        {
            string result = $"{SubProduction.Production.Identifier} -> ";

            foreach(ExpressionDefinition expressionDefinition in this)
            {
                if (expressionDefinition == ExpressionAfterDot)
                {
                    result += " DOT";
                }

                result += $" {expressionDefinition.ToString()} ";
            }

            if (DotIndex >= WithoutActions().Count)
            {
                result += "DOT";
            }

            if (Lookahead.Count > 0)
            {
                result += ", ";
                result += string.Join("|", Lookahead);
            }

            return result;
        }

        public List<ExpressionDefinition> WithoutActions()
        {
            return this.Where(x => !(x is SemanticActionDefinition)
                && !(x is TerminalExpressionDefinition ted && ted.TokenType == TokenType.EmptyString)).ToList();
        }

        internal bool IsDotIndexAtEnd()
        {
            return DotIndex >= WithoutActions().Count;
            //return WithoutActions().Count
        }
    }
}
