using System;
using System.Collections.Generic;
using System.Linq;

namespace Compiler.Parser.Instances
{
    public class Item : List<ExpressionDefinition>
    {
        public SubProduction SubProduction { get; set; }

        public int DotIndex { get; set; } = 0;
        public ExpressionDefinition ExpressionAfterDot => Count > DotIndex ? this[DotIndex] : null;

        public Item(List<ExpressionDefinition> expressions) : base(expressions)
        {
        }

        public Item(SubProduction subProduction)
        {
            SubProduction = subProduction;

            foreach(ExpressionDefinition expressionDefinition in subProduction)
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
                            if (!set.Any(x => x.SubProduction == subProduction))
                            {
                                set.AddRange(new Item(subProduction).Closure());
                            }
                        }
                    }
                }
            }

            return set;
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
            Item item = new Item(SubProduction);
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
