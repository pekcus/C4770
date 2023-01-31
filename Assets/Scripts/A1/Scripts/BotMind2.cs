using EasyAI;
using UnityEngine;

namespace A1.Scripts
{
    /// <summary>
    /// The state that Bot2 enters.
    /// Same logic as BotMind1, but uses BotSensor2!
    /// </summary>
    [CreateAssetMenu(menuName = "A1/Scripts/Bot", fileName = "Bot Mind 2")]
    public class BotMind2 : State
    {
        public override void Execute(Agent agent)
        {
            // TODO - Assignment 1 - Complete the mind of this agent along with any sensors and actuators you need.
            // If already moving towards a box, no need to think of anything new so simply return.
            // As mentioned in the actuator comments, you can probably see how passing transforms around will
            // become confusing in more complex agents and you can instead wrap data into of unique classes to pass.
            if (agent.HasAction<Transform>())
            {
                return;
            }

            // Sense the nearest tile.
            Transform tile = agent.Sense<BotSensor2, Transform>();

            // If there are no tiles left, do nothing.
            if (tile == null)
            {
                agent.Log("Touched all tiles.");
                return;
            }

            // Move towards the tile and try to hit it.
            agent.Log($"Touching {tile.name} next.");
            agent.Move(tile.position);
            agent.Act(tile);
        }
    }
}