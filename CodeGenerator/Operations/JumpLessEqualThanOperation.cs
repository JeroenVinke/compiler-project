namespace Compiler.CodeGeneration.Operations
{
    public class JumpLessEqualThanOperation : AssemblyOperation
    {
        public string Label { get; set; }

        public override string ToString()
        {
            return $"JLE {Label}";
        }
    }
}
