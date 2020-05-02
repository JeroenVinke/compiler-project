using Compiler.CodeGeneration.Operations;
using Compiler.Common;
using Compiler.Common.Instructions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Compiler.CodeGeneration
{
    public static class CodeGenerator
    {
        public static void Generate(List<Instruction> instructions)
        {
            List<Instruction> leaders = GetLeaders(instructions);

            List<Block> blocks = GetBlocks(instructions, leaders);

            foreach (Block block in blocks)
            {
                block.ReplaceJumpsToBlocks(blocks);
            }

            List<AssemblyOperation> operations = new List<AssemblyOperation>();

            operations.Add(new PushOperation()
            {
                Target = "RBP"
            });

            operations.Add(new MoveOperation()
            {
                Value = "RSP",
                Target = "RBP"
            });

            foreach (Block block in blocks)
            {
                block.LiveVariableAnalysis();

                int relativeAddress = 0;

                foreach (Instruction instruction in block.Instructions)
                {
                    foreach (Address variable in instruction.GetAddresses())
                    {
                        variable.CalculateRelativeAddress(ref relativeAddress);
                    }
                }

                int blockSize = block.GetSize();
                if (blockSize > 0)
                {
                    operations.Add(new SubtractOperation()
                    {
                        Value = "" + blockSize,
                        Target = "RSP"
                    });
                }

                foreach (Instruction instruction in block.Instructions)
                {
                    AllocateRegisters(operations, instruction);

                    if (instruction is LabelInstruction labelInstruction)
                    {
                        operations.Add(new LabelOperation()
                        {
                            Label = labelInstruction.Label.ToString()
                        });
                    }
                    else if (instruction is IfJumpInstruction ifJumpInstruction)
                    {
                        operations.Add(new CompareOperation()
                        {
                            Value = ifJumpInstruction.Address2.ToMemoryLocation(),
                            Target = ifJumpInstruction.Address1.ToMemoryLocation()
                        });

                        if (ifJumpInstruction.RelOp == "<")
                        {
                            operations.Add(new JumpLessThanOperation()
                            {
                                Label = ifJumpInstruction.Label.ToString()
                            });
                        }
                        else if (ifJumpInstruction.RelOp == ">=")
                        {
                            operations.Add(new JumpGreaterEqualThanOperation()
                            {
                                Label = ifJumpInstruction.Label.ToString()
                            });
                        }
                    }
                    else if (instruction is AssignmentInstruction assignmentInstruction)
                    {
                        operations.Add(new MoveOperation()
                        {
                            Value = assignmentInstruction.Address1.ToMemoryLocation(),
                            Target = assignmentInstruction.Address3.ToMemoryLocation()
                        });
                    }
                    else if (instruction is AddInstruction addInstruction)
                    {
                        operations.Add(new XorInstruction()
                        {
                            Target = addInstruction.Address3.ToMemoryLocation(),
                            Value = addInstruction.Address3.ToMemoryLocation()
                        });

                        operations.Add(new AddOperation()
                        {
                            Value = addInstruction.Address1.ToMemoryLocation(),
                            Target = addInstruction.Address3.ToMemoryLocation(),
                        });

                        operations.Add(new AddOperation()
                        {
                            Value = addInstruction.Address2.ToMemoryLocation(),
                            Target = addInstruction.Address3.ToMemoryLocation(),
                        });
                    }
                    else if (instruction is ReturnInstruction)
                    {
                        operations.Add(new MoveOperation()
                        {
                            Value = "RBP",
                            Target = "RSP"
                        });

                        operations.Add(new PopOperation()
                        {
                            Target = "RBP"
                        });

                        operations.Add(new ReturnOperation());
                    }
                    else if (instruction is CallInstruction callInstruction)
                    {
                        operations.Add(new CallOperation
                        {
                            Label = callInstruction.Label.ToString()
                        });
                    }
                    else if (instruction is JumpInstruction jumpInstruction)
                    {
                        operations.Add(new JumpOperation
                        {
                            Label = jumpInstruction.Label.ToString()
                        });
                    }
                }
            }

            foreach (AssemblyOperation operation in operations)
            {
                Console.WriteLine(operation);
            }

            ;
        }

        private static void AllocateRegisters(List<AssemblyOperation> operations, Instruction instruction)
        {
            AllocateRegister(instruction, instruction.Address1, operations);
            AllocateRegister(instruction, instruction.Address2, operations);
            AllocateRegister(instruction, instruction.Address3, operations);
        }

        private static void AllocateRegister(Instruction instruction, Address address, List<AssemblyOperation> operations)
        {
            if (address != null && !(address is ConstantValue) && !TryAssignRegister(address))
            {
                LiveAnalysisEntry deadEntry = instruction.LiveVariableEntries.FirstOrDefault(x => x.NextUse == null && x.Variable.Register != null);
                if (deadEntry != null)
                {
                    deadEntry.Variable.Register.Clear();
                    TryAssignRegister(address);

                    if (address.Spilled)
                    {
                        operations.Add(new MoveOperation()
                        {
                            Target = address.Register.Name,
                            Value = address.ToMemoryLocation()
                        });
                    }
                    else
                    {
                        operations.Add(new MoveOperation()
                        {
                            Target = address.Register.Name,
                            Value = $"0"
                        });
                    }
                }
                else
                {
                    Spill(operations, instruction, address);
                }
            }
        }

        private static void Spill(List<AssemblyOperation> operations, Instruction instruction, Address address)
        {
            foreach (Register register in Registers.All)
            {
                LiveAnalysisEntry candidateEntry = instruction.LiveVariableEntries.FirstOrDefault(x => x.Variable.Register != null && !instruction.GetAddresses().Contains(x.Variable));

                if (candidateEntry != null)
                {
                    string registerName = candidateEntry.Variable.Register.Name;

                    candidateEntry.Variable.Register.Clear();
                    operations.Add(new MoveOperation()
                    {
                        Target = candidateEntry.Variable.ToMemoryLocation(),
                        Value = registerName
                    });
                    operations.Add(new XorInstruction()
                    {
                        Target = registerName,
                        Value = registerName
                    });
                    candidateEntry.Variable.Spilled = true;

                    TryAssignRegister(address);
                    return;
                }
            }
        }

        private static bool TryAssignRegister(Address address)
        {
            if (address.Register != null)
            {
                return true;
            }

            foreach (Register register in Registers.All)
            {
                if (register.Address == null)
                {
                    register.Address = address;
                    address.Register = register;
                    return true;
                }
            }

            return false;
        }

        private static List<Block> GetBlocks(List<Instruction> instructions, List<Instruction> leaders)
        {
            List<Block> blocks = new List<Block>();
            Block currentBlock = null;
            foreach (Instruction instruction in instructions)
            {
                if (leaders.Contains(instruction))
                {
                    currentBlock = new Block();
                    blocks.Add(currentBlock);
                }

                currentBlock.Instructions.Add(instruction);
            }

            return blocks;
        }

        private static List<Instruction> GetLeaders(List<Instruction> instructions)
        {
            List<Instruction> leaders = new List<Instruction>();
            bool lastInstructionWasJump = false;

            foreach (Instruction instruction in instructions)
            {
                if (instruction is LabelInstruction)
                {
                    if (!leaders.Contains(instruction))
                    {
                        leaders.Add(instruction);
                    }
                }
                else if (leaders.Count == 0)
                {
                    if (instruction is JumpInstruction)
                    {
                        lastInstructionWasJump = true;
                    }

                    leaders.Add(instruction);
                }
                else
                {
                    if (instruction is JumpInstruction jumpInstruction)
                    {
                        Instruction targetInstruction = instructions.First(x => x is LabelInstruction labelInstruction
                            && labelInstruction.Label == jumpInstruction.Label);

                        if (!leaders.Contains(targetInstruction))
                        {
                            leaders.Add(targetInstruction);
                        }

                        lastInstructionWasJump = true;
                    }
                    else if (lastInstructionWasJump)
                    {
                        if (!leaders.Contains(instruction))
                        {
                            leaders.Add(instruction);
                        }
                        lastInstructionWasJump = false;
                    }
                }
            }

            return leaders;
        }
    }
}
