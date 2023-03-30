using System.Collections.Generic;
using System.Linq;
using EasyAI.Navigation.Nodes;
using UnityEditor.VersionControl;
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
                // Exit loop when we reach the goal!
                if (node.Position == goal)
                {
                    break;
                }

                node.Close();
                OpenNodes(node, goal, nodes, connections);
                node = nodes.Where(n => n.IsOpen).OrderBy(n => n.CostF).First();
            }
            
            // Creating the Path
            while (node != null)
            {
                path.Add(node.Position);
                node = node.Previous;
            }
            path.Reverse();
            //DebugPrint(path, current, goal);
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
                // Efficiency: we can skip irrelevant connections right away
                // Skip connections that don't have the node
                if (c.A != node.Position && c.B != node.Position)
                    continue;
                
                // SR: Make a new node for comparison
                Vector3 p = c.A == node.Position ? c.B : c.A;
                AStarNode newNode = new(p, goal, node);
					
                // With valid connection and a "new" node, we can then insert the new node
                // into the nodes list, if it does not already exist.
                // SR: See if there is already an existing node at this position
                AStarNode existing = nodes.FirstOrDefault(n => n.Position == p);
                // SR: If there is no node, this is easy and just add the new node to the list
                if (existing == null)
                {
                    nodes.Add(newNode);
                    continue;
                }
                
                // If the node already exists, we need to compare which cost is better.
                // The node with the best cost stays, the node with the worse cost is discarded.
                
                // SR: Otherwise, there is an existing node
                // SR: If the existing node has a better (or equal) score, then just discard this new node
                if (existing.CostF <= newNode.CostF)
                {
                    continue;
                }
					
                // SR: Otherwise, the new node is better than the existing one
                // SR: update the existing node to use the new cost and
                existing.UpdatePrevious(node);
                // re-open the updated node
                existing.Open();
            }
        }

        private static void DebugPrint(List<Vector3> path, Vector3 current, Vector3 goal)
        {
            // Debugging the Path
            Debug.Log("Path from " + current.ToString() + " to " + goal.ToString());
            for (int i = 0; i < path.Count; i++)
            {
                Debug.Log(path[i].x + ", " + path[i].z);
                
            }
        }
    }
}