using Compiler.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Compiler.RegularExpressionEngine
{
    public class NodeSet : List<Node>
    {
        public Func<string, Token> Action { get; set; }

        public NodeSet(IEnumerable<Node> collection) : base(collection)
        {
        }
        public NodeSet(List<Node> collection) : base(collection)
        {
        }

        public NodeSet()
        {
        }

        public override string ToString()
        {
            return $"{string.Join(",", this.Select(x => x.Id).ToList())}";
        }

        public void AddIfNotExists(Node to)
        {
            if (!Contains(to))
            {
                Add(to);
            }
        }

        public NodeSet NodesFor(char character)
        {
            NodeSet result = new NodeSet();

            foreach(Node node in this)
            {
                NodeSet nodesForCharacter = (NodeSet)node.GetNodesFor(character);

                nodesForCharacter.ForEach(x => result.AddIfNotExists(x));
            }

            return result;
        }

        public NodeSet AcceptingNodes()
        {
            return new NodeSet(this.Where(x => x.IsAccepting).ToList());
        }

        public Node EntryNode()
        {
            return this.First(x => x.IsEntry);
        }

        public Node Simulate(string v)
        {
            Node curNode = EntryNode();
            int curIndex = 0;
            while (curIndex < v.Length)
            {
                char cur = v[curIndex];
                curNode = curNode.GetNodesFor(cur).FirstOrDefault();

                if (curNode == null)
                {
                    return null;
                }

                curIndex++;
            }

            return curNode;
        }

        public bool IsEqualTo(List<Node> other)
        {
            return string.Join(",", this.OrderBy(x => x.Id).Select(x => x.Id).ToList()) ==
                string.Join(",", other.OrderBy(x => x.Id).Select(x => x.Id).ToList());
        }

        public static NodeSet NodeToSet(Node node)
        {
            return new NodeSet(node.GetAllNodes());
        }
    }
}
