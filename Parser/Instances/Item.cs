using Compiler.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Compiler.Parser.Instances
{
    public class Item : List<ExpressionDefinition>
    {
        public SubProduction SubProduction {
            get
            {
                return SubProduction.AllSubProductions[SubProductionId];
            }
        }
        public int SubProductionId { get; private set; }
        public int DotIndex { get; private set; }
        public HashSet<TerminalExpressionDefinition> Lookahead { get; set; }

        public ExpressionDefinition ExpressionAfterDot { get; private set; }

        private ItemSet _closure = null;
        public int Id { get; private set; }
        public static int MaxId = 0;

        public Item(SubProduction subProduction, int dotIndex, HashSet<TerminalExpressionDefinition> lookahead = null)
            : this(subProduction.Id, dotIndex, lookahead)
        {
        }

        public Item(int subProductionId, int dotIndex, HashSet<TerminalExpressionDefinition> lookahead = null)
        {
            SubProductionId = subProductionId;
            DotIndex = dotIndex;
            Lookahead = lookahead ?? new HashSet<TerminalExpressionDefinition>();
            Id = MaxId++;

            int i = 0;
            foreach (ExpressionDefinition expressionDefinition in SubProduction)
            {
                Add(expressionDefinition);

                if (ExpressionAfterDot == null)
                {
                    if (!(expressionDefinition is SemanticActionDefinition)
                        && !(expressionDefinition is TerminalExpressionDefinition ted && ted.TokenType == TokenType.EmptyString))
                    {
                        if (DotIndex == i)
                        {
                            ExpressionAfterDot = expressionDefinition;
                        }

                        i++;
                    }
                }
            }
        }

        public void AddLookaheads(IEnumerable<TerminalExpressionDefinition> lookaheads)
        {
            foreach(TerminalExpressionDefinition lookahead in lookaheads)
            {
                Lookahead.Add(lookahead);
            }
        }

        public override bool Equals(object obj)
        {
            return obj.GetHashCode() == GetHashCode();
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(SubProductionId, DotIndex, Lookahead.GetSequenceHashCode());
        }

        public ItemSet Closure()
        {
            if (_closure != null)
            {
                return _closure;
            }

            _closure = new ItemSet(new List<Item> { Clone() });

            List<ExpressionDefinition> tail = WithoutActions().Skip(DotIndex + 1).ToList();

            if (Lookahead != null)
            {
                tail.AddRange(Lookahead);
            }

            ExpressionSet firstTail = First(tail);

            if (ExpressionAfterDot is NonTerminalExpressionDefinition a)
            {
                foreach (Production production in Grammar.Instance)
                {
                    if (production.Identifier == a.Identifier)
                    {
                        foreach (SubProduction subProduction in production)
                        {
                            foreach (TerminalExpressionDefinition ted in firstTail)
                            {
                                Item itemToAdd = new Item(subProduction, 0, new HashSet<TerminalExpressionDefinition> { ted });
                                _closure.Add(itemToAdd);
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
                    result.AddRangeUnique(first.Where(x => x.TokenType != TokenType.EmptyString));

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

        public Item Clone(bool incrementDotIndex = false)
        {
            Item item = new Item(SubProduction, incrementDotIndex ? DotIndex + 1 : DotIndex, Lookahead.ToHashSet());
            return item;
        }

        public string ToDot()
        {
            string result = $"({Id}) {SubProduction.Production.Identifier} -> ";

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
            return ExpressionAfterDot == null;
        }
    }
}
