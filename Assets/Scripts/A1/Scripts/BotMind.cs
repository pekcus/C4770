using EasyAI;
using UnityEngine;

namespace A1.Scripts
{
    /// <summary>
    /// The first state that bots enter, which send each bot to their respective state script.
    /// </summary>
    [CreateAssetMenu(menuName = "A1/Scripts/Bot", fileName = "Bot Mind")]
    public class BotMind : State
    {
        // Set up different bot minds.
        private int bot = 1;
        public override void Enter(Agent agent)
        {
            Sensor sensor = agent.Sensors[0];
            if (sensor.name.Contains("2"))
            {
                bot = 2;
            }
        }

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
            Transform tile;
            // temporary logic - this will be implemented in two different scripts!
            if(bot == 1)
                tile = agent.Sense<BotSensor, Transform>();
            else
                tile =  agent.Sense<BotSensor2, Transform>();

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