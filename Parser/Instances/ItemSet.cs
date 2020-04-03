using System;
using System.Collections.Generic;
using System.Linq;

namespace Compiler.Parser.Instances
{
    public class ItemSet : List<Item>
    {
        public ItemSet Closure()
        {
            ItemSet set = Clone();

            foreach(Item item in this)
            {
                if (item.ExpressionAfterDot is NonTerminalExpression nte)
                {
                    foreach(Production production in Grammar.Instance)
                    {
                        foreach(SubProduction subProduction in production)
                        {
                            if (subProduction.Where(x => !(x is SemanticActionDefinition)).First() is NonTerminalExpressionDefinition nted
                                && nted.Identifier == nte.Identifier)
                            {
                                if (!set.Any(x => x.SubProduction == subProduction))
                                {
                                    set.Add(new Item(subProduction));
                                }
                            }
                        }
                    }
                }
            }

            return set;
        }

        private ItemSet Clone()
        {
            ItemSet set = new ItemSet();

            foreach(Item item in this)
            {
                set.Add(item.Clone());
            }

            return set;
        }
    }
}
