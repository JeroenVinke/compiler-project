using Compiler.Common.Instructions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Compiler.Common
{
    public class Block
    {
        public int Id { get; set; }
        public static int MaxId { get; set; } = 0;

        public Block()
        {
            Id = ++MaxId;
        }

        public void ReplaceJumpsToBlocks(List<Block> blocks)
        {
            foreach (Instruction instruction in Instructions)
            {
                if (instruction is JumpInstruction jumpInstruction)
                {
                    Block targetBlock = blocks.First(x => x.Instructions.First() is LabelInstruction labelInstruction
                    && labelInstruction.Label == jumpInstruction.Label);
                    jumpInstruction.TargetBlock = targetBlock;
                }
            }
        }

        public List<Address> GetVariables()
        {
            List<Address> variables = new List<Address>();

            foreach (Instruction instruction in Instructions)
            {
                variables.AddRange(instruction.GetAddresses().Except(variables).Where(x => !(x is ConstantValue)));
            }

            return variables;
        }

        public void LiveVariableAnalysis()
        {
            List<Instruction> instructionsReverse = Instructions.ToList();
            instructionsReverse.Reverse();

            List<LiveAnalysisEntry> entries = new List<LiveAnalysisEntry>();

            foreach(Address variable in GetVariables())
            {
                entries.Add(new LiveAnalysisEntry
                {
                    Variable = variable,
                    Live = false,
                    NextUse = null
                });
            }
            
            foreach (Instruction instruction in instructionsReverse)
            {
                // make copy of list
                entries = entries.ToList();

                if (instruction.Address3 != null
                    && !(instruction.Address3 is ConstantValue))
                {
                    ReplaceEntry(entries, new LiveAnalysisEntry
                    {
                        Variable = instruction.Address3,
                        Live = false,
                        NextUse = null
                    });
                }

                if (instruction.Address1 != null
                    && !(instruction.Address1 is ConstantValue))
                {
                    ReplaceEntry(entries, new LiveAnalysisEntry
                    {
                        Variable = instruction.Address1,
                        Live = true,
                        NextUse = instruction
                    });
                }

                if (instruction.Address2 != null
                    && !(instruction.Address2 is ConstantValue))
                {
                    ReplaceEntry(entries, new LiveAnalysisEntry
                    {
                        Variable = instruction.Address2,
                        Live = true,
                        NextUse = instruction
                    });
                }

                instruction.LiveVariableEntries = entries;
            }
        }

        public int GetSize()
        {
            return Instructions.Sum(x => x.GetAddresses().Sum(y => y.Size));
        }

        private void ReplaceEntry(List<LiveAnalysisEntry> entries, LiveAnalysisEntry entry)
        {
            LiveAnalysisEntry existingEntry = entries.FirstOrDefault(x => x.Variable == entry.Variable);

            entries.Remove(existingEntry);
            entries.Add(entry);
        }

        public List<Instruction> Instructions { get; set; } = new List<Instruction>();
    }
}
