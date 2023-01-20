using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Bees
{
    internal class EmployedBee
    {
        private BeeColony Colony;
        private SubGraph SubGraph;
        public EmployedBee(BeeColony colony)
        {
            Colony = colony;
        }

        public SubGraph GetLocation()
        {
            return SubGraph;
        }

        public void PickArea(int size)
        {
            List<Node> tempSolution = new List<Node>();
            tempSolution.Add(PickNode(Colony.GetSearchArea().Nodes));

            while (tempSolution.Count != size)
            {
                int i = 0;
                Node goal;
                do
                {
                    goal = null;
                    i++;
                    var tempNode = tempSolution[tempSolution.Count - i];
                    goal = PickNode(tempNode.Connections.Where(a => !tempSolution.Contains(a)).ToList());
                } while (tempSolution.Contains(goal));
                tempSolution.Add(goal);
            }

            int counter = 0;
            foreach (var a in tempSolution)
            {
                foreach (var b in tempSolution)
                {
                    if (a.Id == b.Id) counter++;
                }
            }
            if (counter > size) throw new Exception();
            SubGraph = new SubGraph(tempSolution);
            SubGraph.AddDescendants(SubGraph.AnalyzeAdjacentCommonNodes().ToList());
        }

        public Node PickNode(List<Node> nodes)
        {
            var cumulativeNectar = 0;
            foreach (var node in nodes)
            {
                cumulativeNectar += node.Connections.Count;
            }

            Dictionary<Node, double> chances = new Dictionary<Node, double>();
            foreach (var node in nodes)
            {
                chances.Add(node, (double)node.Connections.Count / (double)cumulativeNectar);
            }

            chances.OrderBy(a => a.Value);
            var cumulativeChance = 0D;
            Random rand = new Random();

            var chance = rand.NextDouble();

            foreach (var probability in chances)
            {
                if (chance > cumulativeChance && chance < (cumulativeChance + probability.Value))
                {
                    return probability.Key;
                }
                cumulativeChance += probability.Value;
            }

            return null;
        }
    }
}
