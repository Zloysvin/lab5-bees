using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bees
{
    internal static class DataGenerator
    {
        public static List<Node> GenerateNodes(int size)
        {
            var generatedNodes = new List<Node>();
            for (int i = 0; i < size; i++)
            {
                generatedNodes.Add(new Node(i));
            }
            return generatedNodes;
        }

        public static List<EmployedBee> GenerateEmployedBees(int count, BeeColony colony)
        {
            var bees = new List<EmployedBee>();

            for (int i = 0; i < count; i++)
            {
                bees.Add(new EmployedBee(colony));
            }

            return bees;
        }

        public static List<ForagerBee> GenerateForagerBees(int count, BeeColony colony)
        {
            var bees = new List<ForagerBee>();

            for (int i = 0; i < count; i++)
            {
                bees.Add(new ForagerBee(colony));
            }
            return bees;
        }
    }
}
