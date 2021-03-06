﻿namespace Compiler.CodeGeneration.Operations
{
    public class AddOperation : AssemblyOperation
    {
        public string Target { get; set; }
        public string Value { get; set; }

        public override string ToString()
        {
            return $"ADD {Target}, {Value}";
        }
    }
}
