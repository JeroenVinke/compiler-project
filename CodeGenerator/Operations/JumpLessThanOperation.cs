namespace Compiler.CodeGeneration.Operations
{
    public class JumpLessThanOperation : AssemblyOperation
    {
        public string Label { get; set; }

        public override string ToString()
        {
            return $"JL {Label}";
        }
    }
}
