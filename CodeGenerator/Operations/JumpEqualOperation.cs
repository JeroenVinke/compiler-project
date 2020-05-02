namespace Compiler.CodeGeneration.Operations
{
    public class JumpEqualOperation : AssemblyOperation
    {
        public string Label { get; set; }

        public override string ToString()
        {
            return $"JEQ {Label}";
        }
    }
}
