using System;
using EasyAI;
using UnityEngine;

namespace A2.States
{
    /// <summary>
    /// The global state which microbes are always in.
    /// </summary>
    [CreateAssetMenu(menuName = "A2/States/Microbe Mind", fileName = "Microbe Mind")]
    public class MicrobeMind : State
    {
        public override void Execute(Agent agent)
        {
            // TODO - Assignment 2 - Complete the mind of the microbes.
            Microbe mic = agent.GetComponent<Microbe>();
            if(mic.IsHungry)
                agent.SetState<MicrobeHungryState>();
            else if (mic.BeingHunted)
                agent.SetState<MicrobeHuntedState>();
            else
                agent.SetState<MicrobeRoamingState>();
        }
    }
}