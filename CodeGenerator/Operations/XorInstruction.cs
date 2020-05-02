namespace Compiler.CodeGeneration.Operations
{
    public class XorInstruction : AssemblyOperation
    {
        public string Value { get; set; }
        public string Target { get; set; }

        public override string ToString()
        {
            return $"XOR {Target}, {Value}";
        }
    }
}
