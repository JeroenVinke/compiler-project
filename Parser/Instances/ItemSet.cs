using System.Collections.Generic;
using System.Linq;

namespace Compiler.Parser.Instances
{
    public class ItemSet : List<Item>
    {
        public static int MaxId { get; set; }
        public int Id { get; set; }
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
            if (Count != otherSet.Count)
            {
                return false;
            }

            for(int i = 0; i < Count; i++)
            {
                if (!otherSet[i].IsEqualTo(this[i]))
                {
                    return false;
                }
            }

            return true;
        }

        public List<ItemSet> GetCanonicalSets(List<ExpressionDefinition> symbols)
        {
            List<ItemSet> C = new List<ItemSet>();
            C.Add(this);
            while (true)
            {
                List<ItemSet> setsToAdd = new List<ItemSet>();
                foreach (ItemSet set in C)
                {
                    foreach (ExpressionDefinition symbol in symbols)
                    {
                        ItemSet _goto = set.Goto(symbol);

                        if (_goto.Count > 0)
                        {
                            ItemSet _existingGoto = C.FirstOrDefault(x => x.IsEqualTo(_goto));
                            if (_existingGoto == null)
                            {
                                _existingGoto = setsToAdd.FirstOrDefault(x => x.IsEqualTo(_goto));
                                if (_existingGoto == null)
                                {
                                    setsToAdd.Add(_goto);
                                    _existingGoto = _goto;
                                }
                            }

                            if (!set.Transitions.ContainsKey(symbol))
                            {
                                set.Transitions.Add(symbol, _existingGoto);
                            }
                        }
                    }
                }

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

        public ItemSet Closure()
        {
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
                        if (!set.Any(x => x.IsEqualTo(i)))
                        {
                            set.Add(i);
                            addedItem = true;
                        }
                    }
                }
            }

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
                    Item cloned = item.Clone();
                    cloned.Next();
                    result.Add(cloned);
                }
            }

            return result.Closure();
        }

        public List<Item> NonKernelItems()
        {
            List<Item> result = this.Where(x => x.DotIndex == 0
                && x.SubProduction.Production.Identifier != "Initial").ToList();

            return result;
        }

        public List<Item> KernelItems()
        {
            List<Item> result = this.Except(NonKernelItems()).ToList();

            if (this.Any(x => x.SubProduction.Production.Identifier == "Initial"))
            {
                result.Add(this.First(x => x.SubProduction.Production.Identifier == "Initial"));
            }

            return result;
        }

        public void RemoveNonKernels()
        {
            foreach(Item item in NonKernelItems())
            {
                Remove(item);
            }
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

        public ItemSet GetGoto(NonTerminalExpressionDefinition target)
        {
            return Transitions.First(x => x.Key.IsEqualTo(target)).Value;
        }
    }
}
