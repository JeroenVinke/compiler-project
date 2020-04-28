using Compiler.Common;
using Compiler.Common.Instructions;
using System.Collections.Generic;

namespace Compiler.CodeGeneration
{
    public static class CodeGenerator
    {
        public static void GenerateCode(List<Instruction> instructions)
        {
            Registers registers = new Registers();

            LiveVariableAnalysis.Analyse(instructions);
            //foreach (Instruction instruction in instructions)
            //{
            //    if (instruction is AddInstruction addInstruction)
            //    {

            //    }
            //    else if (instruction is AssignmentInstruction assignmentInstruction)
            //    {

            //    }
            //}
        }
    }

    public class Registers
    {
        public Address R1 { get; set; }
        public Address R2 { get; set; }
        public Address R3 { get; set; }
        public Address R4 { get; set; }
    }
}
