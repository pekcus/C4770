﻿using EasyAI;
using UnityEngine;

namespace A1.Scripts
{
    public class BotActuator : Actuator
    {
        [Tooltip("How far away from the box must the agent be to pick it up.")] [Min(float.Epsilon)] [SerializeField]
        private float collectDistance = 1;

        /// <summary>
        /// Collect a box.
        /// </summary>
        /// <param name="agentAction">The action to perform.</param>
        /// <returns>True if the box was collect, false otherwise.</returns>
        public override bool Act(object agentAction)
        {
            // Cast the action into a transform. If it is anything else this actuator cannot use it so return false.
            // Passing a general transform may become hard in a multi-actuator setup.
            // In such cases making a unique class to wrap said data like transforms for each respective actuator is recommended.
            if (agentAction is not Transform tile)
            {
                return false;
            }

            // Return false if not close enough to pickup the box.
            if (Vector3.Distance(Agent.transform.position, tile.position) > collectDistance)
            {
                Log("Not close enough to touch the tile.");
                return false;
            }

            // Pickup (destroy) the box and return true indicating the action has been completed.
            Log("Hit the tile.");
            // Save the points as an int - floor.Hit() resets the floor state, so floor.State will be 0 inside the loop.
            MyFloor floor = tile.GetComponent<MyFloor>();
            int points = (int)floor.State;
            if (floor.Hit())
            {
                BotPerformance.hit += points;
            }

            return true;
        }
    }
}