using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Bees
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Graph graph = new Graph(DataGenerator.GenerateNodes(200));

            Console.WriteLine("Enter amount of bees");
            int beeCount = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Enter clique size:");
            int cliqueSize = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Enter memory coefficient:");
            double memoryCoefficient = Convert.ToDouble(Console.ReadLine());
            
            BeeColony beeColony = new BeeColony(graph, cliqueSize, memoryCoefficient);

            List<EmployedBee> employedBees = DataGenerator.GenerateEmployedBees((int)(beeCount * 0.1), beeColony);
            List<ForagerBee> foragerBees = DataGenerator.GenerateForagerBees(beeCount - employedBees.Count, beeColony);
            beeColony.EmployedBees = employedBees;
            beeColony.ForagerBees = foragerBees;

            DateTime start = DateTime.Now;
            SubGraph clique = beeColony.FindClique();
            DateTime end = DateTime.Now;
            TimeSpan time = end - start;
            
            Console.WriteLine($"Time to find: {time.TotalMilliseconds} ms");
            Console.WriteLine("Nodes and their connections:");
            List<Node> temp = clique.Members;
            foreach (Node member in temp)
            {
                Console.WriteLine($"Member id: {member.Id}");
                Console.Write("Connections: ");
                foreach (Node connection in member.Connections)
                {
                    Console.Write($"{connection.Id} ");
                }
                Console.WriteLine();
            }

        }
    }
}
