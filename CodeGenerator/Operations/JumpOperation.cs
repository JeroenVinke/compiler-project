namespace Compiler.CodeGeneration.Operations
{
    public class JumpOperation : AssemblyOperation
    {
        public string Label { get; set; }

        public override string ToString()
        {
            return $"JMP {Label}";
        }
    }
}
