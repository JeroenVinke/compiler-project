using System;
using System.Collections.Generic;
using System.Linq;

namespace Compiler.RegularExpressionEngine
{
    public class RegexToDFAConverter
    {
        public List<char> Alphabet;
        public SyntaxTreeNode TopSyntaxTreeNode { get; set; }

        private RegexToDFAConverter()
        {
        }

        public static Node Convert(string regex)
        {
            RegexToDFAConverter converter = new RegexToDFAConverter();
            return converter.DoConvert(regex);
        }

        private Node DoConvert(string regex)
        {
            SyntaxTreeNode result = RegexToSyntaxTreeConverter.Convert(regex);

            SyntaxTreeNode s = result;
            SyntaxTreeNode topParent = s;

            while (s != null)
            {
                if (s.Parent != null)
                {
                    topParent = s.Parent;
                }
                s = s.Parent;
            }

            TopSyntaxTreeNode = topParent;
            Alphabet = topParent.LeafNodes().Distinct().ToList();

            Dictionary<Node, SyntaxTreeNodeSet> DStates = new Dictionary<Node, SyntaxTreeNodeSet>();
            Node startNode = new Node();
            startNode.IsEntry = true;
            DStates.Add(startNode, new SyntaxTreeNodeSet(topParent.FirstPos()));
            List<Node> MarkedStates = new List<Node>();
            List<(Node, char, Node)> DTrans = new List<(Node, char, Node)>();

            while (DStates.Keys.Any(x => !MarkedStates.Contains(x)))
            {
                Node S = DStates.Keys.First(x => !MarkedStates.Contains(x));
                MarkedStates.Add(S);
                foreach (char a in Alphabet)
                {
                    SyntaxTreeNodeSet U = new SyntaxTreeNodeSet(DStates[S].Where(x => x.Character == a).SelectMany(x => x.FollowPos).Distinct().ToList());
                    bool isAccepting = U.Any(x => x.Character == '#');
                    if (U.Any())
                    {
                        Node targetNode = DStates.FirstOrDefault(x => x.Value.IsEqualTo(U)).Key;
                        if (targetNode == null)
                        {
                            targetNode = new Node();
                            if (isAccepting)
                            {
                                targetNode.IsAccepting = true;
                            }
                            DStates.Add(targetNode, U);
                        }
                        else
                        {
                            targetNode.IsAccepting = true;
                        }

                        if (a != '#')
                        {
                            DTrans.Add((S, a, targetNode));
                            new Edge(a, S, targetNode);
                        }
                    }
                }
            }

            return startNode;
        }
    }
}
