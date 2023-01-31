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
        /// The total number of tiles hit.
        /// </summary>
        public static int total;
        /// <summary>
        /// The current tile value.
        /// </summary>
        public static int hit;

        // todo: Consider if score should decrease if a tile disappears before it's hit?
        /// <summary>
        /// Calculate the performance as a percentage of the number of tiles which have been collected.
        /// </summary>
        /// <returns>The percentage of tiles which have been collected.</returns>
        //public override float CalculatePerformance() => (_totalTiles - TileCount) / _totalTiles * 100;
        public override float CalculatePerformance()
        {
            hit = MyBotManager.exp ? hit * hit : hit ;
            hit *= MyBotManager.Points;
            total += hit;
            hit = 0;
            return total;
        }

        protected override void Start()
        {
            hit = 0;
            base.Start();
        }
    }
}