using Compiler.Parser.Common;
using System.Collections.Generic;

namespace Compiler.Parser
{
    public abstract class SyntaxTreeNode
    {
        public SyntaxTreeNodeType Type { get; set; }
        public int Id { get; set; }
        public static int MaxId { get; set; }

        public SyntaxTreeNode(SyntaxTreeNodeType type)
        {
            Type = type;
            Id = MaxId++;
        }

        public virtual Address GenerateCode(List<Instruction> instructions)
        {
            //foreach (SyntaxTreeNode child in GetChildren())
            //{
            //    child.GenerateCode(instructions);
            //}

            return null;
        }

        public virtual string ToDot()
        {
            string result = $"{Id}2222 [label=\"{ToString()}\"]\r\n";

            foreach(SyntaxTreeNode child in GetChildren())
            {
                result += $"{child.Id}2222 -> {Id}2222\r\n";
                result += child.ToDot();
            }

            return result;
        }

        protected virtual List<SyntaxTreeNode> GetChildren()
        {
            return new List<SyntaxTreeNode>();
        }
    }
}
