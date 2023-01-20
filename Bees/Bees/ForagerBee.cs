using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bees
{
    internal class ForagerBee
    {
        private BeeColony Colony;
        public SubGraph SubGraph { get; set; }

        public ForagerBee(BeeColony colony)
        {
            Colony = colony;
        }

        public void PickArea(int areaSize)
        {
            var sources = Colony.GetDiscoveredFoodSources().OrderBy(a => a.Item2).ToList();

            int cumulativeNectar = 0;
            int i = 0;
            foreach (var source in sources)
            {
                cumulativeNectar += source.Item2 * (sources.Count - i);
                i++;
            }

            var subGraphsAndChances = new Dictionary<SubGraph, double>();
            i = 0;
            foreach (var source in sources)
            {
                subGraphsAndChances.Add(source.Item1, (double)source.Item2 * (sources.Count - i) / (double)cumulativeNectar);
            }

            var rand = new Random();

            var chance = rand.NextDouble();
            double cumulativeChance = 0;
            foreach (var probability in subGraphsAndChances)
            {
                if (chance > cumulativeChance && chance < (cumulativeChance + probability.Value))
                {
                    SubGraph = probability.Key;
                    return;
                }
                cumulativeChance += probability.Value;
            }
        }

        public void DiscoverAdjacentArea(int areaSize)
        {
            if (SubGraph.GetDescendants().Count == 0) return;
            var subgraph = SubGraph.GetDescendants().First();

            SubGraph.Descendants.Remove(subgraph);
            SubGraph = subgraph;

            SubGraph.AddDescendants(SubGraph.AnalyzeAdjacentCommonNodes().ToList());
        }
    }
}
