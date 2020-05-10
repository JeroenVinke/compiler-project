namespace Compiler.CodeGeneration.Operations
{
    public class JumpEqualToOperation : AssemblyOperation
    {
        public string Label { get; set; }

        public override string ToString()
        {
            return $"JE {Label}";
        }
    }
}
