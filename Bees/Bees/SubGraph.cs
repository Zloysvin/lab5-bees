using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bees
{
    internal class SubGraph
    {
        public List<Node> Members { get; private set; }
        public List<SubGraph> Descendants = new();

        public int Common;

        public SubGraph(List<Node> members)
        {
            Members = members;
            Common = AnalyzeAdjacentCommonNodes().Count;
            AddDescendants(AnalyzeAdjacentCommonNodes().ToList());
        }
        public SubGraph(List<Node> members, SubGraph parent)
        {
            Members = members;
            Common = AnalyzeAdjacentCommonNodes().Count;
        }

        public List<SubGraph> GetDescendants()
        {
            return Descendants;
        }

        public void AddDescendants(List<Node> ddescendants)
        {
            foreach (var node in ddescendants)
            {
                List<Node> tempList = new List<Node>();
                foreach (var member in Members)
                {
                    tempList.Add(member);
                }

                tempList.Add(node);
                var tempGraph = new SubGraph(tempList, this);
                Descendants.Add(tempGraph);
            }
        }

        public HashSet<Node> AnalyzeAdjacentCommonNodes()
        {
            HashSet<Node> adjacents = new HashSet<Node>();
            foreach (var a in Members[0].Connections)
            {
                adjacents.Add(a);
            }

            foreach (var b in Members)
            {
                adjacents.IntersectWith(b.Connections);
            }
            return adjacents;
        }
    }
}
