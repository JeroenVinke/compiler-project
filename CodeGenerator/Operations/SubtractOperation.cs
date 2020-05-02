namespace Compiler.CodeGeneration.Operations
{
    public class SubtractOperation : AssemblyOperation
    {
        public string Target { get; set; }
        public string Value { get; set; }

        public override string ToString()
        {
            return $"SUB {Target}, {Value}";
        }
    }
}
