namespace Compiler.CodeGeneration.Operations
{
    public class LabelOperation : AssemblyOperation
    {
        public string Label { get; set; }

        public override string ToString()
        {
            return $"{Label}:";
        }
    }
}
