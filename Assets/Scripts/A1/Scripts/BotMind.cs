using EasyAI;
using UnityEngine;

namespace A1.Scripts
{
    /// <summary>
    /// The global state which the cleaner is always in.
    /// </summary>
    [CreateAssetMenu(menuName = "A1/States/Cleaner Mind", fileName = "Cleaner Mind")]
    public class BotMind : State
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

            // Sense the nearest box.
            Transform tile = agent.Sense<BotSensor, Transform>();

            // If there are no boxes left, do nothing.
            if (tile == null)
            {
                agent.Log("Touched all tiles.");
                return;
            }

            // Move towards the box and try to pick it up.
            agent.Log($"Touching {tile.name} next.");
            agent.Move(tile.position);
            agent.Act(tile);
        }
    }
}