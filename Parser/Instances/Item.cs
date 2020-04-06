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
        public TerminalExpressionDefinition Lookahead { get; set; }
        public ExpressionDefinition ExpressionAfterDot => Count > DotIndex ? this[DotIndex] : null;

        public Item(List<ExpressionDefinition> expressions) : base(expressions)
        {
        }

        public Item(SubProduction subProduction, TerminalExpressionDefinition lookahead = null)
        {
            SubProduction = subProduction;
            Lookahead = lookahead;

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
            ItemSet set = new ItemSet();
            set.Add(Clone());

            if (ExpressionAfterDot is NonTerminalExpressionDefinition a)
            {
                foreach (Production production in Grammar.Instance)
                {
                    if (production.Identifier == a.Identifier)
                    {
                        foreach (SubProduction subProduction in production)
                        {
                            List<ExpressionDefinition> tail = this.Skip(DotIndex).ToList();

                            if (Lookahead != null)
                            {
                                tail.Add(Lookahead);
                            }

                            foreach (TerminalExpressionDefinition ted in First(tail))
                            {
                                if (!set.Any(x => x.SubProduction == subProduction))
                                {
                                    set.AddRange(new Item(subProduction, ted).Closure());
                                }
                            }
                        }
                    }
                }
            }

            return set;
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
                    }
                }
            }

            if (previousCanBeEmpty)
            {
                result.Add(new TerminalExpressionDefinition { TokenType = TokenType.EmptyString });
            }

            return result;
        }

        internal bool IsEqualTo(Item x)
        {
            if (x.Count != Count)
            {
                return false;
            }

            if (x.DotIndex != DotIndex)
            {
                return false;
            }

            for (int i = 0; i < Count; i++)
            {
                if (x[i].SubProduction.Production.Identifier != this[i].SubProduction.Production.Identifier)
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

            int i = 0;
            foreach(ExpressionDefinition expressionDefinition in this)
            {
                if (i == DotIndex)
                {
                    result += " DOT";
                }

                result += $" {expressionDefinition.ToString()} ";

                i++;
            }

            if (DotIndex >= Count)
            {
                result += "DOT";
            }

            return result;
        }
    }
}
