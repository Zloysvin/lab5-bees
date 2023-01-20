using System;
using System.Collections.Generic;
using System.Linq;

namespace Bees;

internal class Graph
{
    public List<Node> Nodes { get; }

    public Graph(List<Node> nodes)
    {
        Nodes = nodes;

        ReInitNodes();
    }

    public void ReInitNodes()
    {
        var rand = new Random();

        foreach (var node in Nodes) node.SetConnections(new List<Node>());

        foreach (var node in Nodes)
        {
            var limit = rand.Next(2, 31);
            if (node.Connections.Count + limit > 31) continue;
            var tempAdjacents = Nodes.Where(a =>
                    a.Id != node.Id && a.Connections.Count < 31 && !a.Connections.Contains(node))
                .OrderBy(_ => rand.Next())
                .Take(limit)
                .ToList();

            foreach (var a in tempAdjacents)
            {
                node.Connections.Add(a);
                a.Connections.Add(node);
            }
        }
    }
}