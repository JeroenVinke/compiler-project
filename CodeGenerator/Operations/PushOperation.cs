namespace Compiler.CodeGeneration.Operations
{
    public class PushOperation : AssemblyOperation
    {
        public string Target { get; set; }

        public override string ToString()
        {
            return $"PUSH {Target}";
        }
    }
}
