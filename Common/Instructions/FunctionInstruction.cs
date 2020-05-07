using System.Collections.Generic;

namespace Compiler.Common.Instructions
{
    public class FunctionInstruction : LabelInstruction
    {
        public List<SymbolTableEntry> Arguments { get; set; } = new List<SymbolTableEntry>();

        public FunctionInstruction(Label label, List<SymbolTableEntry> arguments) : base(label)
        {
            Arguments = arguments;
        }

        public override string GenerateCodeString()
        {
            return $"fun {Label.ToString()}:";
        }

        public override string ToString()
        {
            return $"fun {Label.ToString()}";
        }
    }
}
