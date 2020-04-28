using System.Collections.Generic;
using System.Linq;
using Compiler.Common;
using Compiler.Common.Instructions;

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
            Label lastLabel = null;

            foreach (StatementASTNode statement in Statements)
            {
                lastLabel = new Label();

                if (previous != null && previous.Backpatch(lastLabel))
                {
                    instructions.Add(new LabelInstruction(lastLabel));
                }

                statement.GenerateCode(instructions);

                previous = statement;
            }

            if (previous != null && previous.Backpatch(lastLabel))
            {
                instructions.Add(new LabelInstruction(lastLabel));
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
