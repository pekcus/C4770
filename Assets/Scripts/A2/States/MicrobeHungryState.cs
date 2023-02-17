using A2.Sensors;
using EasyAI;
using UnityEngine;

namespace A2.States
{
    /// <summary>
    /// State for microbes that are hungry and wanting to seek food.
    /// </summary>
    [CreateAssetMenu(menuName = "A2/States/Microbe Hungry State", fileName = "Microbe Hungry State")]
    public class MicrobeHungryState : State
    {
        private Microbe target = null;
        public override void Enter(Agent agent)
        {
            // TODO - Assignment 2 - Complete this state. Have microbes search for other microbes to eat.
            agent.Log($"Feeling hungry, looking for a microbe to hunt...");
        }
        
        public override void Execute(Agent agent)
        {
            // TODO - Assignment 2 - Complete this state. Have microbes search for other microbes to eat.
            target = agent.Sense<NearestPreySensor, Microbe>();
            if (target == null)
                return;
            agent.Log($"Hunting microbe {target.name}.");
            agent.Move(target.transform.position);
        }
        
        public override void Exit(Agent agent)
        {
            // TODO - Assignment 2 - Complete this state. Have microbes search for other microbes to eat.
        }
    }
}