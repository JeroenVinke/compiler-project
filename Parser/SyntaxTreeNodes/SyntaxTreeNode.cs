using Compiler.Parser.Common;
using Compiler.Parser.Instructions;
using System.Collections.Generic;

namespace Compiler.Parser
{
    public abstract class SyntaxTreeNode
    {
        public SyntaxTreeNodeType Type { get; set; }
        public int Id { get; set; }
        public static int MaxId { get; set; }
        public List<JumpInstruction> NextInstructionsToBackpatch { get; set; } = new List<JumpInstruction>();

        public SyntaxTreeNode(SyntaxTreeNodeType type)
        {
            Type = type;
            Id = MaxId++;
        }

        public virtual Address GenerateCode(List<Instruction> instructions)
        {
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

        public virtual bool Backpatch(Label nextLabel)
        {
            foreach (JumpInstruction instruction in NextInstructionsToBackpatch)
            {
                instruction.Label = nextLabel;
            }

            return NextInstructionsToBackpatch.Count > 0;
        }
    }
}
