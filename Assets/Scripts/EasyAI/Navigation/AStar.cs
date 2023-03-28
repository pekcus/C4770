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
            foreach (Connection c in connections)
            {
                if (c.A == current)
                    nodes.Add(new AStarNode(c.B, goal, node));
                else if (c.B == current)
                    nodes.Add(new AStarNode(c.A, goal, node));
            }
            AStarNode next = nodes.OrderBy(n => n.CostF).First();
            
            return new();
        }
    }
}