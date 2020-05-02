namespace Compiler.CodeGeneration.Operations
{
    public class JumpGreaterEqualThanOperation : AssemblyOperation
    {
        public string Label { get; set; }

        public override string ToString()
        {
            return $"JGE {Label}";
        }
    }
}
