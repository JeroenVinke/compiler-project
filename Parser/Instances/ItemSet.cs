using System;
using System.Collections.Generic;
using System.Linq;

namespace Compiler.Parser.Instances
{
    public class ItemSet : List<Item>
    {
        public static int MaxId { get; set; }
        public int Id { get; set; }
        public Dictionary<ExpressionDefinition, ItemSet> Transitions { get; set; } = new Dictionary<ExpressionDefinition, ItemSet>();

        public Item InitialItem => this.First();

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

        public ItemSet Closure()
        {
            ItemSet set = Clone();

            foreach(Item item in this)
            {
                ItemSet closure = item.Closure();
                set.AddRange(closure.Where(x => !set.Any(y => y.IsEqualTo(x))));
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
            List<Item> result = this.Where(x => x.DotIndex == 0).ToList();

            if (!result.Contains(InitialItem))
            {
                result.Add(InitialItem);
            }

            return result;
        }

        public List<Item> KernelItems()
        {
            return this.Except(NonKernelItems()).ToList();
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
