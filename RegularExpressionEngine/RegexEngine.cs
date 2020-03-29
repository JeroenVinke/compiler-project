using System.Collections.Generic;

namespace Compiler.RegularExpressionEngine
{
    public class RegexEngine
    {
        private static Dictionary<string, NodeSet> Sets = new Dictionary<string, NodeSet>();

        public static SimulationState Simulate(string regex, string input)
        {
            NodeSet set = GetOrCreateSet(regex);
            Node n = set.Simulate(input);

            if(n != null)
            {
                if (n.IsAccepting)
                {
                    return SimulationState.Accepting;
                }

                return SimulationState.OnTrack;
            }
            else
            {
                return SimulationState.OffTrack;
            }
        }

        private static NodeSet GetOrCreateSet(string regex)
        {
            if (!Sets.TryGetValue(regex, out NodeSet set))
            {
                set = NodeSet.NodeToSet(RegexToDFAConverter.Convert(regex));
                Sets.Add(regex, set);
            }

            return set;
        }
    }
}
