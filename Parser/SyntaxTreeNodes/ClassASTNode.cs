using Compiler.Common;
using Compiler.Common.Instructions;
using System.Collections.Generic;
using System.Linq;

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
            List<Instruction> childInstructions = new List<Instruction>();
            foreach (SyntaxTreeNode child in Children)
            {
                child.GenerateCode(childInstructions);
            }

            FunctionASTNode main = (FunctionASTNode)Children.FirstOrDefault(x => x is FunctionASTNode functionASTNode && functionASTNode.FunctionName.Name.ToLower() == "main");
            if (main != null)
            {
                instructions.Add(new JumpInstruction(main.FunctionName.Label));
            }

            instructions.AddRange(childInstructions);

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
