using Compiler.Parser.Instances;
using System.Collections.Generic;

namespace Compiler.Parser
{
    public class ParsingTableSegment
    {
        public ItemSet Set { get; set; }

        public List<ParsingTableEntry> Entries { get; set; } = new List<ParsingTableEntry>();
    }
}
