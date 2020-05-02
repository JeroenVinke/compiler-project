namespace Compiler.CodeGeneration.Operations
{
    public class CallOperation : AssemblyOperation
    {
        public string Label { get; set; }

        public override string ToString()
        {
            return $"CALL {Label}";
        }
    }
}
