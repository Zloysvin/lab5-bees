using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Bees
{
    internal class BeeColony
    {
        public List<EmployedBee> EmployedBees { get; set; }
        public List<ForagerBee> ForagerBees { get; set; }
        
        private List<Tuple<SubGraph, int>> FoodSourcesAndSizes;
        private int CliqueSize;
        private Graph Graph;
        private double Coefficient;

        public BeeColony(Graph graph, int cliqueSize, double coefficient)
        {
            FoodSourcesAndSizes = new List<Tuple<SubGraph, int>>();
            this.Graph = graph;
            this.CliqueSize = cliqueSize;
            this.Coefficient = coefficient;
        }

        public void SendBeesRecon()
        {
            List<SubGraph> discoveredAreas = new List<SubGraph>();
            foreach (EmployedBee bee in EmployedBees)
            {
                bee.PickArea(CliqueSize);
                discoveredAreas.Add(Createlique(bee.GetLocation()));
            }
            ShareDiscoveredAreas(discoveredAreas);
            FoodSourcesAndSizes = FoodSourcesAndSizes.Where(a => a.Item1.GetDescendants().Count != 0).ToList();
            discoveredAreas = new List<SubGraph>();
            foreach (ForagerBee bee in ForagerBees)
            {
                bee.PickArea(CliqueSize);
                bee.DiscoverAdjacentArea(CliqueSize);
                discoveredAreas.Add(Createlique(bee.SubGraph));
            }
            ShareDiscoveredAreas(discoveredAreas);
        }

        public void ShareDiscoveredAreas(List<SubGraph> discoveredAreas)
        {
            foreach (var area in discoveredAreas)
            {
                FoodSourcesAndSizes.Add(Tuple.Create(area,
                    area.Members.Count * area.Members.Count * (1 + area.GetDescendants().Count)));
            }
        }
        
        public void UpdateFoodSourcesData()
        {
            FoodSourcesAndSizes = FoodSourcesAndSizes
                .Where(a => a.Item1.GetDescendants().Count != 0)
                .OrderByDescending(a => a.Item2)
                .Take((int)(FoodSourcesAndSizes.Count * Coefficient)).ToList();
        }

        public Graph GetSearchArea()
        {
            return Graph;
        }

        public List<Tuple<SubGraph, int>> GetDiscoveredFoodSources()
        {
            return FoodSourcesAndSizes;
        }

        public SubGraph FindClique()
        {
            for (int i = 0; i < int.MaxValue; i++)
            {
                SendBeesRecon();
                foreach (var source in FoodSourcesAndSizes)
                {
                    if (source.Item1.Members.Count >= CliqueSize) return source.Item1;
                }
                UpdateFoodSourcesData();
                if (i % 100 == 0)
                    Console.WriteLine($"Biggest clique in memory:{FoodSourcesAndSizes.Max(a => a.Item1.Members.Count)}");
            }
            return null;
        }

        public SubGraph Createlique(SubGraph original)
        {
            List<Node> traversed = new List<Node>();
            List<Node> toRemove = new List<Node>();

            foreach (var member in original.Members)
            {
                foreach (var traversedOne in traversed)
                {
                    if (!traversedOne.Connections.Contains(member)) { toRemove.Add(member); break; }
                }
                if (!toRemove.Contains(member) && member.Connections.Count >= CliqueSize) traversed.Add(member);
            }
            return new SubGraph(traversed);
        }
    }
}
