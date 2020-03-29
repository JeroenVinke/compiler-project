using System;
using System.Collections.Generic;
using System.Linq;

namespace Compiler.RegularExpressionEngine
{
    public class Node
    {
        public int Id { get; set; }
        public bool IsEntry { get; set; }
        public List<Edge> OutgoingEdges { get; set; } = new List<Edge>();
        public List<Edge> IncomingEdges { get; set; } = new List<Edge>();
        public bool IsAccepting { get; set; }
        public Action Action { get; internal set; }

        public static int IdCounter = 0;

        public Node()
        {
            Id = ++IdCounter;
        }

        public List<Node> GetNodesFor(char character, NodeSet result = null, NodeSet visited = null)
        {
            result = result ?? new NodeSet();
            visited = visited ?? new NodeSet();

            if (visited.Contains(this))
            {
                return new List<Node>();
            }
                
            visited.Add(this);

            foreach (Edge edge in OutgoingEdges)
            {
                if (edge.Character == character)
                {
                    result.AddIfNotExists(edge.To);
                }
            }

            return result;
        }

        public List<Node> GetAllNodes()
        {
            List<Edge> edges = GetEdgesRecursive();
            List<Node> nodes = edges.Select(x => x.From).ToList();
            nodes.AddRange(edges.Select(x => x.To).ToList());
            return nodes.ToList().Distinct().ToList();
        }

        public Node Simulate(char c)
        {
            return GetNodesFor(c).FirstOrDefault();
        }

        private List<Edge> GetEdgesRecursive(List<Edge> edges = null, List<Edge> visited = null)
        {
            visited = visited ?? new List<Edge> { };
            edges = edges ?? new List<Edge> { };

            foreach(Edge edge in OutgoingEdges)
            {
                if (!visited.Contains(edge))
                {
                    visited.Add(edge);
                    if (!edges.Any(x => x.From.Id == edge.From.Id && x.To.Id == edge.To.Id))
                    {
                        edges.Add(edge);
                    }
                    edge.To.GetEdgesRecursive(edges, visited);
                }
            }

            return edges;
        }

        public string ToDot()
        {
            List<Edge> edges = GetEdgesRecursive();

            string result = "";
            foreach (Edge edge in edges)
            {
                result += $"{edge.ToString()};\r\n";
            }

            return result;
        }
    }
}
