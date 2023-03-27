using A2.Pickups;
using A2.Sensors;
using EasyAI;
using UnityEngine;

namespace A2.States
{
    /// <summary>
    /// State for microbes that are seeking a pickup.
    /// </summary>
    [CreateAssetMenu(menuName = "A2/States/Microbe Seeking Pickup State", fileName = "Microbe Seeking Pickup State")]
    public class MicrobeSeekingPickupState : State
    {
        private MicrobeBasePickup target = null;
        public override void Enter(Agent agent)
        {
            // TODO - Assignment 2 - Complete this state. Have microbes look for pickups.
            agent.Log($"Looking for a pickup...");
            
        }
        
        public override void Execute(Agent agent)
        {
            // TODO - Assignment 2 - Complete this state. Have microbes look for pickups.
            target = agent.Sense<NearestPickupSensor, MicrobeBasePickup>();
            if (target == null)
                return;
            agent.Log($"Targeting pickup {target.name}.");
            agent.Move(target.transform.position);
            //agent.Act(tile);
        }
        
        public override void Exit(Agent agent)
        {
            // TODO - Assignment 2 - Complete this state. Have microbes look for pickups.
        }
    }
}