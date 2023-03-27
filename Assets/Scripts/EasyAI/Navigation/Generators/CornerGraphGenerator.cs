using UnityEngine;

namespace EasyAI.Navigation.Generators
{
    /// <summary>
    /// Convex corner graph placement for nodes.
    /// </summary>
    public class CornerGraphGenerator : NodeGenerator
    {
        [SerializeField]
        [Min(0)]
        [Tooltip("How far away from corners should the nodes be placed.")]
        private int cornerNodeSteps = 3;
    
        /// <summary>
        /// Place nodes at convex corners.
        /// </summary>
        public override void Generate()
        {
            // TODO - Assignment 4 - Complete corner-graph node generation.
            //NodeArea.AddNode(6,10);
            //NodeArea.AddNode(7,10);
            // Loop through each node space.
            for (int x = 0; x < NodeArea.RangeX; x++)
            {
                for (int z = 0; z < NodeArea.RangeZ; z++)
                {
                    // If a node is closed, it might be a corner.
                    // Is it okay to assume that a node is not a corner if it's on the edge?
                    if (!NodeArea.IsOpen(x, z) && CheckEdge(x, z))
                    {
                        // Find which corner it is, and check '3' spaces out.
                        
                    }
                }
            }
        }

        private bool CheckEdge(int x, int z)
        {
            if ( (x >= cornerNodeSteps || x <= NodeArea.RangeX - cornerNodeSteps - 1) 
                 && (x >= cornerNodeSteps || x <= NodeArea.RangeX - cornerNodeSteps - 1) ) {
                return true;
            }
            return false;
        }
    }
}