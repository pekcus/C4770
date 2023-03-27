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
                        // Check all 4 corners, placing a Node wherever it is open.
                        
                        
                    }
                }
            }
        }

        /// <summary>
        /// Helper function to make sure that the given coordinates are at least cNS spaces away from an edge.
        /// </summary>
        /// <param name="x"> x-coordinate </param>
        /// <param name="z"> z-coordinate </param>
        /// <returns> true if the coordinates are at least cNS away from an edge </returns>
        private bool CheckEdge(int x, int z)
        {
            if ( (x >= cornerNodeSteps && x <= NodeArea.RangeX - cornerNodeSteps - 1) 
                 && (x >= cornerNodeSteps && x <= NodeArea.RangeX - cornerNodeSteps - 1) ) {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Helper function to check that the corner is free for cNS spaces.
        /// </summary>
        /// <param name="x"> x-coordinate </param>
        /// <param name="z"> z-coordinate </param>
        /// <param name="corner"> the corner being checked </param>
        /// <returns> true if the corner is free for cNS </returns>
        private bool CheckCorner(int x, int z, int corner)
        {
            switch (corner)
            {
                // top right corner
                case 1:
                {
                    if (NodeArea.IsOpen(x-1, z+1) && NodeArea.IsOpen(x-1, z) && NodeArea.IsOpen(x, z+1))
                        if (NodeArea.IsOpen(x - 2, z + 2) && NodeArea.IsOpen(x - 2, z) && NodeArea.IsOpen(x, z + 2))
                            return true;
                    return false;
                }
            }
        }

        /// <summary>
        /// Helper function to check all 4 corners.
        /// </summary>
        /// <param name="x"> x-coordinate </param>
        /// <param name="z"> z-coordinate </param>
        private void CheckCorners(int x, int z)
        {
            
        }
    }
}