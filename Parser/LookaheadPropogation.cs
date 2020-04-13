using Compiler.Parser.Instances;

namespace Compiler.Parser
{
    public class LookaheadPropogation
    {
        public ItemSet FromSet { get; set; }
        public Item FromItem { get; set; }
        public ItemSet ToSet { get; set; }
        public Item ToItem { get; set; }
    }
}
