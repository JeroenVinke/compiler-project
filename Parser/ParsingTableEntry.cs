using Compiler.Parser.Instances;
using System;

namespace Compiler.Parser
{
    public abstract class ParsingTableEntry
    {
        public ItemSet ItemSet { get; set; }

        internal abstract string ShortDescription();
    }
}
