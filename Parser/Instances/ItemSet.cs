using Compiler.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Compiler.Parser.Instances
{
    public class ItemSet : HashSet<Item>
    {
        public static int MaxId { get; set; }
        public int Id { get; set; }
        private ItemSet _closure;
        public Dictionary<ExpressionDefinition, ItemSet> Transitions { get; set; } = new Dictionary<ExpressionDefinition, ItemSet>();

        public ItemSet() : this(new List<Item>())
        {
        }

        public ItemSet(List<Item> items) : base(items)
        {
            Id = MaxId++;
        }

        public bool IsEqualTo(ItemSet otherSet)
        {
            return GetHashCode() == otherSet.GetHashCode();
        }

        public override int GetHashCode()
        {
            return this.GetSequenceHashCode();
        }

        public List<LookaheadPropogation> DetermineLookaheads(ExpressionDefinition x)
        {
            List<LookaheadPropogation> result = new List<LookaheadPropogation>();
            
            if (!Transitions.Keys.Contains(x))
            {
                return result;
            }

            foreach (Item item in KernelItems())
            {
                ItemSet j = new ItemSet(
                    new List<Item>()
                    {
                        new Item(item.SubProduction,
                            0,
                            new HashSet<TerminalExpressionDefinition>() {
                                new TerminalExpressionDefinition {
                                    TokenType = TokenType.Hash
                                }
                            }
                        )
                    }
                );
    
                foreach (Item closureItem in j.KernelItems())
                {
                    if (closureItem.ExpressionAfterDot != null
                        && closureItem.ExpressionAfterDot.IsEqualTo(x))
                    {
                        Item i = Transitions[x].First(xx => xx.IsEqualTo(closureItem, true));

                        if (!closureItem.Lookahead.Any(y => y.TokenType == TokenType.Hash))
                        {
                            i.AddLookaheads(closureItem.Lookahead.Except(i.Lookahead));
                        }
                        else
                        {
                            result.Add(new LookaheadPropogation
                            {
                                FromItem = item,
                                FromSet = this,
                                ToSet = Transitions[x],
                                ToItem = i
                            });
                        }
                    }
                }
            }

            return result;
        }

        public ItemSet Closure()
        {
            if(_closure != null)
            {
                return _closure;
            }

            ItemSet set = Clone();

            bool addedItem = true;

            while (addedItem)
            {
                addedItem = false;

                foreach (Item item in set.ToList())
                {
                    ItemSet closure = item.Closure();

                    foreach (Item i in closure)
                    {
                        if (set.Add(i))
                        {
                            //set.TryAddItem(i);
                            addedItem = true;
                        }
                        else
                        {
                            ;
                        }
                    }
                }
            }

            _closure = set;

            return set;
        }

        public ItemSet Goto(ExpressionDefinition expression)
        {
            ItemSet result = new ItemSet();

            foreach (Item item in this)
            {
                if (item.ExpressionAfterDot != null
                    && item.ExpressionAfterDot.IsEqualTo(expression))
                {
                    Item cloned = item.Clone(true);
                    result.Add(cloned);
                }
            }

            return result.Closure();
        }

        public List<ItemSet> GetCanonicalSets(List<ExpressionDefinition> symbols)
        {
            List<ItemSet> C = new List<ItemSet>();
            Dictionary<int, ItemSet> setKeys = new Dictionary<int, ItemSet>();
            setKeys.Add(GetHashCode(), this);
            C.Add(this);
            List<ItemSet> lastNewsets = new List<ItemSet>();
            lastNewsets.Add(this);
            while (true)
            {
                List<ItemSet> setsToAdd = new List<ItemSet>();
                foreach (ItemSet set in lastNewsets)
                {
                    foreach (ExpressionDefinition symbol in symbols)
                    {
                        ItemSet _goto = set.Goto(symbol);

                        if (_goto.Count > 0)
                        {
                            ItemSet gotoSet;

                            if (setKeys.TryGetValue(_goto.GetHashCode(), out ItemSet existingGoto))
                            {
                                gotoSet = existingGoto;
                            }
                            else
                            {
                                gotoSet = _goto;
                                setsToAdd.Add(_goto);
                                setKeys.Add(_goto.GetHashCode(), _goto);
                            }

                            if (!set.Transitions.ContainsKey(symbol))
                            {
                                set.Transitions.Add(symbol, gotoSet);
                            }
                        }
                    }
                }

                lastNewsets = setsToAdd.ToList();

                if (setsToAdd.Count == 0)
                {
                    break;
                }
                else
                {
                    C.AddRange(setsToAdd);
                }
            }

            return C;
        }

        public List<Item> NonKernelItems()
        {
            List<Item> result = this.Where(x => x.DotIndex == 0
                && x.SubProduction.Production.Identifier != ParserConstants.Initial).ToList();

            return result;
        }

        public List<Item> KernelItems()
        {
            List<Item> result = this.Except(NonKernelItems()).ToList();

            if (this.Any(x => x.SubProduction.Production.Identifier == ParserConstants.Initial))
            {
                result.Add(this.First(x => x.SubProduction.Production.Identifier == ParserConstants.Initial));
            }

            return result;
        }

        public ItemSet Clone()
        {
            ItemSet set = new ItemSet();

            foreach(Item item in this)
            {
                set.Add(item.Clone());
            }

            return set;
        }

        public string ToDot()
        {
            string result = "node [shape=record fontname=Arial];\r\n";
            result += $"{Id} [label=\"{Id}\\n\\r";
            foreach(Item item in this)
            {
                result += item.ToDot() + "\\r";
            }

            result += "\"]\r\n";

            return result.Replace("-", "\\-").Replace(">","\\>");
        }
    }
}
