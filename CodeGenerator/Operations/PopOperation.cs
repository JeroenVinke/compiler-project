namespace Compiler.CodeGeneration.Operations
{
    public class PopOperation : AssemblyOperation
    {
        public string Target { get; set; }

        public override string ToString()
        {
            return $"POP {Target}";
        }
    }
}
