namespace Compiler.CodeGeneration.Operations
{
    public class CompareOperation : AssemblyOperation
    {
        public string Value { get; set; }
        public string Target { get; set; }

        public override string ToString()
        {
            return $"CMP {Target}, {Value}";
        }
    }
}
