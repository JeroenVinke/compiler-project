using Compiler.Common;
using Compiler.Common.Instructions;
using System.Collections.Generic;

namespace Compiler.Parser.SyntaxTreeNodes
{
    public class FunctionASTNode : SyntaxTreeNode
    {
        public TypeASTNode ReturnType { get; set; }
        public SymbolTableEntry FunctionName { get; set; }
        public SyntaxTreeNode Body { get; set; }
        public List<SymbolTableEntry> Arguments { get; set; } = new List<SymbolTableEntry>();

        public FunctionASTNode() : base(SyntaxTreeNodeType.Function)
        {
        }

        public override Address GenerateCode(List<Instruction> instructions)
        {
            Label functionlabel = new Label();

            instructions.Add(new FunctionInstruction(functionlabel, Arguments));
            FunctionName.Label = functionlabel;

            Body.GenerateCode(instructions);

            return base.GenerateCode(instructions);
        }

        public override string ToString()
        {
            return "Function";
        }

        protected override List<SyntaxTreeNode> GetChildren()
        {
            return new List<SyntaxTreeNode> { Body };
        }
    }
}
