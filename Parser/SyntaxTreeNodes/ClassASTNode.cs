using Compiler.Common;
using Compiler.Parser.Common;
using Compiler.Parser.Instructions;
using System.Collections.Generic;

namespace Compiler.Parser.SyntaxTreeNodes
{
    public class ClassASTNode : SyntaxTreeNode
    {
        public SymbolTableEntry ClassName { get; set; }
        public List<SyntaxTreeNode> Children { get; set; }

        public ClassASTNode() : base(SyntaxTreeNodeType.Class)
        {
        }

        public override Address GenerateCode(List<Instruction> instructions)
        {
            foreach (SyntaxTreeNode child in Children)
            {
                child.GenerateCode(instructions);
            }

            return base.GenerateCode(instructions);
        }

        public override string ToString()
        {
            return "Class";
        }

        protected override List<SyntaxTreeNode> GetChildren()
        {
            return Children;
        }
    }
}
