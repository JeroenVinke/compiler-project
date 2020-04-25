using Compiler.Parser.Common;
using System.Collections.Generic;
using System.Linq;
using Compiler.Parser.Instructions;

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
            StatementASTNode previous = null;

            foreach (StatementASTNode statement in Statements)
            {
                Label label = new Label();

                if (previous != null && previous.Backpatch(label))
                {
                    instructions.Add(new LabelInstruction(label));
                }

                statement.GenerateCode(instructions);

                previous = statement;
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
