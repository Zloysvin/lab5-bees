using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bees
{
    internal class Node
    {
        public int Id { get; private set; }

        public List<Node> Connections { get; private set; }

        public Node(int id)
        {
            Id = id;
        }

        public void SetConnections(List<Node> connection)
        {
            Connections = connection;
        }
    }
}
