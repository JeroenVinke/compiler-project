using Compiler.Parser.Common;
using System.Collections.Generic;
using System.Linq;

namespace Compiler.Parser.SyntaxTreeNodes
{
    public class StatementsASTNode : SyntaxTreeNode
    {
        public List<StatementASTNode> Statements { get; set; } = new List<StatementASTNode>();

        public StatementsASTNode() : base(SyntaxTreeNodeType.Statements)
        {
        }

        public override Address GenerateCode(List<Instruction> instructions)
        {
            Label label = new Label();
            Address address = new Address();

            foreach (StatementASTNode statement in Statements)
            {
                statement.GenerateCode(instructions);
            }

            return base.GenerateCode(instructions);
        }

        public override string ToString()
        {
            return "Statements";
        }

        protected override List<SyntaxTreeNode> GetChildren()
        {
            return Statements.Select(x => (SyntaxTreeNode)x).ToList();
        }
    }
}
