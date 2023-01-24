using System.Linq;
using EasyAI;
using UnityEngine;

namespace A1.Scripts
{
    /// <summary>
    /// Calculate how well the Bot is doing based on how many tiles it hit.
    /// </summary>
    [DisallowMultipleComponent]
    public class BotPerformance : PerformanceMeasure
    {
        /// <summary>
            /// The total tiles at the start of the scene.
            /// </summary>
            private int _totalTiles;
        
            /// <summary>
            /// The total number of tiles left in the scene.
            /// </summary>
            private static int TileCount => FindObjectsOfType<Transform>().Count(t => t.name.Contains("Floor") && t.GetComponent<MyFloor>().IsLit);

            /// <summary>
            /// The total number of tiles hit.
            /// </summary>
            public static int hit;
        
            /// <summary>
            /// Calculate the performance as a percentage of the number of tiles which have been collected.
            /// </summary>
            /// <returns>The percentage of tiles which have been collected.</returns>
            //public override float CalculatePerformance() => (_totalTiles - TileCount) / _totalTiles * 100;
            public override float CalculatePerformance() => hit;

            protected override void Start()
            {
                _totalTiles = TileCount;
                hit = 0;
                base.Start();
            }
    }
}