namespace Compiler.CodeGeneration.Operations
{
    public class MoveOperation : AssemblyOperation
    {
        public string Value { get; set; }
        public string Target { get; set; }

        public override string ToString()
        {
            return $"MOV {Target}, {Value}";
        }
    }
}
