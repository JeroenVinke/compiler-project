namespace Compiler.RegularExpressionEngine
{
    public class Edge
    {
        public Edge(char c, Node from, Node to)
        {
            Character = c;
            From = from;
            To = to;

            From.OutgoingEdges.Add(this);
            To.IncomingEdges.Add(this);
        }

        public char Character { get; set; }
        public Node From { get; set; }
        public Node To { get; set; }

        public override string ToString()
        {
            string result = "";

            if (From.IsEntry)
            {
                result += $"{From.Id} [shape=Mdiamond];\r\n";
            }

            if (To.IsAccepting)
            {
                result += $"{To.Id} [shape=Msquare];\r\n";
            }

            result += $"{From.Id} -> {To.Id} [label=\"{Character}\"]";

            return result;
        }
    }
}
