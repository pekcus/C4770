using EasyAI;
using UnityEngine;

namespace A1.Scripts
{
    /// <summary>
    /// The first state that bots enter, which send each bot to their respective state script.
    /// </summary>
    //[CreateAssetMenu(menuName = "A1/Scripts/Bot", fileName = "Bot Mind")]
    public class BotMind : State
    {
        public override void Enter(Agent agent)
        {
            if (agent.Sense<BotSensor1, Transform>() != null)
            {
                // Agent has a BotSensor1 component, it must be the first Bot.
                agent.SetState<BotMind1>();
            }
            else
            {
                // Agent does not have a BotSensor1 component, it must be the second Bot.
                agent.SetState<BotMind2>();    
            }
        }
    }
}