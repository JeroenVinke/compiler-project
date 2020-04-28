using Compiler.Common;
using Compiler.Common.Instructions;
using System.Collections.Generic;
using System.Linq;

namespace Compiler.Parser.SyntaxTreeNodes
{
    public class ClassesASTNode : SyntaxTreeNode
    {
        public List<ClassASTNode> Classes { get; set; } = new List<ClassASTNode>();

        public ClassesASTNode() : base(SyntaxTreeNodeType.Classes)
        {
        }

        public override Address GenerateCode(List<Instruction> instructions)
        {
            foreach(ClassASTNode classNode in Classes)
            {
                classNode.GenerateCode(instructions);
            }

            return base.GenerateCode(instructions);
        }

        public override string ToString()
        {
            return "Classes";
        }

        protected override List<SyntaxTreeNode> GetChildren()
        {
            return Classes.Select(x => (SyntaxTreeNode)x).ToList();
        }
    }
}
