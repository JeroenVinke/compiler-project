using System;
using System.Collections.Generic;
using System.Linq;

namespace Compiler.RegularExpressionEngine
{
    public class SyntaxTreeNode
    {
        public SyntaxTreeNodeType Type { get; set; }

        public SyntaxTreeNode Parent { get; set; }

        public List<SyntaxTreeNode> Children { get; set; } = new List<SyntaxTreeNode>();

        public List<SyntaxTreeNode> FollowPos { get; set; } = new List<SyntaxTreeNode>();

        public char Character { get; set; }
        public int Id { get; internal set; }
        public static int Counter = 0;
        public int Position { get; set; }

        public SyntaxTreeNode()
        {
            Id = ++Counter;
        }

        public SyntaxTreeNode LeftMostNode()
        {
            if (Children.Count > 0)
            {
                return Children[0].LeftMostNode();
            }

            return this;
        }

        internal List<char> LeafNodes()
        {
            List<char> alphabet = new List<char>();

            if (Type == SyntaxTreeNodeType.Leaf)
            {
                return new List<char> { Character };
            }

            foreach(SyntaxTreeNode child in Children)
            {
                alphabet.AddRange(child.LeafNodes());
            }

            return alphabet;
        }

        public SyntaxTreeNode SetPositions()
        {
            SyntaxTreeNode leftMostNode = LeftMostNode();
            int i = 1;
            SetPositions1(leftMostNode, ref i);

            return leftMostNode;
        }

        public SyntaxTreeNode TopParent()
        {
            SyntaxTreeNode cur = this;

            while (cur != null)
            {
                if (cur.Parent != null)
                {
                    cur = cur.Parent;
                }
                else
                {
                    break;
                }
            }

            return cur;
        }

        public void SetPositions1(SyntaxTreeNode node, ref int i)
        {
            if (node.Parent != null)
            {
                foreach (SyntaxTreeNode child in node.Parent.Children.Where(x => x.Type == SyntaxTreeNodeType.Leaf))
                {
                    child.Position = i;
                    i++;
                }

                SetPositions1(node.Parent, ref i);
            }
        }

        public bool Nullable()
        {
            if (Type == SyntaxTreeNodeType.Leaf && Character == 'ε')
            {
                return true;
            }
            if (Type == SyntaxTreeNodeType.Star)
            {
                return true;
            }
            if (Type == SyntaxTreeNodeType.Plus)
            {
                return false;
            }
            if (Type == SyntaxTreeNodeType.And)
            {
                return Children.All(x => x.Nullable());
            }
            if (Type == SyntaxTreeNodeType.Or)
            {
                return Children.Any(x => x.Nullable());
            }

            return false;
        }

        public List<SyntaxTreeNode> FirstPos()
        {
            if (Type == SyntaxTreeNodeType.Star)
            {
                return Children.First().FirstPos();
            }
            if (Type == SyntaxTreeNodeType.Plus)
            {
                return Children.First().FirstPos();
            }
            if (Type == SyntaxTreeNodeType.Or)
            {
                return Children.First().FirstPos().Union(Children.Last().FirstPos()).ToList();
            }
            if (Type == SyntaxTreeNodeType.And)
            {
                if (Children.First().Nullable())
                {
                    return Children.First().FirstPos().Union(Children.Last().FirstPos()).ToList();
                }
                else
                {
                    return Children.First().FirstPos();
                }
            }
            if (Type == SyntaxTreeNodeType.Leaf)
            {
                return new List<SyntaxTreeNode> { this };
            }

            return null;
        }

        public void CalculateFollowPos()
        {
            foreach(SyntaxTreeNode child in Children)
            {
                child.CalculateFollowPos();
            }

            if (Type == SyntaxTreeNodeType.And)
            {
                List<SyntaxTreeNode> followPositions = Children.Last().FirstPos();
                foreach (SyntaxTreeNode node in Children.First().LastPos())
                {
                    node.FollowPos.AddRange(followPositions.Except(node.FollowPos));
                }
            }
            if (Type == SyntaxTreeNodeType.Star
                || Type == SyntaxTreeNodeType.Plus)
            {
                foreach (SyntaxTreeNode node in LastPos())
                {
                    node.FollowPos.AddRange(FirstPos());
                }
            }
        }

        public List<SyntaxTreeNode> LastPos()
        {
            if (Type == SyntaxTreeNodeType.Star)
            {
                return Children.Last().LastPos();
            }
            if (Type == SyntaxTreeNodeType.Plus)
            {
                return Children.Last().LastPos();
            }
            if (Type == SyntaxTreeNodeType.Or)
            {
                return Children.First().LastPos().Union(Children.Last().LastPos()).ToList();
            }
            if (Type == SyntaxTreeNodeType.And)
            {
                List<SyntaxTreeNode> result = new List<SyntaxTreeNode>();
                if (Children.Last().Nullable())
                {
                    return Children.First().LastPos().Union(Children.Last().LastPos()).ToList();
                }
                else
                {
                    return Children.Last().LastPos();
                }
            }
            if (Type == SyntaxTreeNodeType.Leaf)
            {
                return new List<SyntaxTreeNode> { this };
            }

            return null;
        }

        public override string ToString()
        {
            string childrenResults = "";

            foreach (var child in Children)
            {
                string label = "";

                if (child.Type == SyntaxTreeNodeType.Leaf)
                {
                    label = "" + child.Character;
                }
                else if (child.Type == SyntaxTreeNodeType.Star)
                {
                    label = "*";
                }
                else if (child.Type == SyntaxTreeNodeType.Plus)
                {
                    label = "+";
                }
                else if (child.Type == SyntaxTreeNodeType.Or)
                {
                    label = "||";
                }
                else if (child.Type == SyntaxTreeNodeType.And)
                {
                    label = "&&";
                }

                childrenResults += child.ToString();
                childrenResults += $"{child.Id} [label=\"{label}\"]\r\n";
                childrenResults += $"{child.Id} -> {Id}\r\n";
            }

            return childrenResults;
        }
    }

    public enum SyntaxTreeNodeType
    {
        And,
        Or,
        Star,
        Plus,
        Leaf
    }
}
