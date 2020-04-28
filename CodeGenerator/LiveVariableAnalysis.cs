using Compiler.Common;
using Compiler.Common.Instructions;
using System.Collections.Generic;

namespace Compiler.CodeGeneration
{
    public static class LiveVariableAnalysis
    {
        public static void Analyse(List<Instruction> instructions)
        {
            foreach (Instruction instruction in instructions)
            {
                //instruction.GetAddresses();
                if (instruction is AddInstruction addInstruction)
                {

                }
                else if (instruction is AssignmentInstruction assignmentInstruction)
                {

                }
            }
        }
    }
}
