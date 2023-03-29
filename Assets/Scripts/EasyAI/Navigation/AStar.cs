using System.Collections.Generic;
using System.Linq;
using EasyAI.Navigation.Nodes;
using UnityEngine;

namespace EasyAI.Navigation
{
    /// <summary>
    /// A* pathfinding.
    /// </summary>
    public static class AStar
    {
        /// <summary>
        /// Perform A* pathfinding.
        /// </summary>
        /// <param name="current">The starting position.</param>
        /// <param name="goal">The end goal position.</param>
        /// <param name="connections">All node connections in the scene.</param>
        /// <returns>The path of nodes to take to get from the starting position to the ending position.</returns>
        public static List<Vector3> Perform(Vector3 current, Vector3 goal, List<Connection> connections)
        {
            // TODO - Assignment 4 - Implement A* pathfinding.
            List<Vector3> path = new List<Vector3>();
            List<AStarNode> nodes = new List<AStarNode>();
            AStarNode node = new AStarNode(current, goal, null);
            nodes.Add(node);

            // Visiting Nodes
            while (nodes.Any(n => n.IsOpen)) {
                node.Close();
                OpenNodes(node, goal, nodes, connections);
                node = nodes.Where(n => n.IsOpen).OrderBy(n => n.CostF).First();
            }
            
            // Creating the Path
            path.Add(node.Position);
            while (node.Previous != null)
            {
                path.Add(node.Previous.Position);
                node = node.Previous;
            }
            path.Add(node.Position);
            path.Reverse();
            // Debugging the Path
            Debug.Log("Path from " + current.ToString() + " to " + goal.ToString());
            for (int i = 0; i < path.Count; i++)
            {
                Debug.Log(path[i].x + ", " + path[i].z);
                
            }
            return path;
        }

        /// <summary>
        /// Helper Function to open nodes
        /// </summary>
        /// <param name="node"> the current node </param>
        /// <param name="goal"> the goal node </param>
        /// <param name="nodes"> the list of nodes </param>
        /// <param name="connections"> the list of connections </param>
        private static void OpenNodes(AStarNode node, Vector3 goal, List<AStarNode> nodes, List<Connection> connections)
        {
            foreach (Connection c in connections)
            {
                if (!(nodes.Any(n => n.IsOpen && n.Position == c.A)) && c.A == node.Position)
                {
                    nodes.Add(new AStarNode(c.B, goal, node));
                }
                if (!(nodes.Any(n => n.IsOpen && n.Position == c.B)) && c.B == node.Position)
                {
                    nodes.Add(new AStarNode(c.A, goal, node));
                }
            }
        }
    }
}