using Compiler.Common;
using Compiler.Common.Instructions;
using System.Collections.Generic;

namespace Compiler.Parser.SyntaxTreeNodes
{
    public class FunctionCallASTNode : FactorASTNode
    {
        public FunctionASTNode FunctionASTNode { get; set; }
        public SymbolTableEntry Target { get; set; }
        public List<FactorASTNode> Arguments { get; set; }

        public FunctionCallASTNode() : base(SyntaxTreeNodeType.FunctionCall)
        {
        }

        public override Address GenerateCode(List<Instruction> instructions)
        {
            List<Address> argumentAddresses = new List<Address>();
            foreach (FactorASTNode factor in Arguments)
            {
                argumentAddresses.Add(factor.GenerateCode(instructions));
            }

            foreach(Address argumentAddress in argumentAddresses)
            {
                instructions.Add(new ParamInstruction(argumentAddress));
            }

            instructions.Add(new CallInstruction(Target.Label));

            return new Address();
        }

        public override string ToString()
        {
            return $"{Target.ToString()}";
        }

        protected override List<SyntaxTreeNode> GetChildren()
        {
            return new List<SyntaxTreeNode> { };
        }
    }
}
