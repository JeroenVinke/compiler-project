namespace Compiler.CodeGeneration.Operations
{
    public class JumpGreaterThanOperation : AssemblyOperation
    {
        public string Label { get; set; }

        public override string ToString()
        {
            return $"JG {Label}";
        }
    }
}
