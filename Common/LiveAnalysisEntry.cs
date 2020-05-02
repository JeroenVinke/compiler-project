using Compiler.Common.Instructions;
using System.Diagnostics;

namespace Compiler.Common
{
    [DebuggerDisplay("Variable = {Variable}, Live={Live}, NextUse={NextUse}")]
    public class LiveAnalysisEntry
    {
        public Address Variable { get; set; }
        public bool Live { get; set; }
        public Instruction NextUse { get; set; }
    }
}
