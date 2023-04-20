using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EasyAI;
using Project.Pickups;
using Project.Sensors;
using UnityEngine;

namespace Project
{
    /*
     * Collector Role State
     */
    public class SCapture : StateMachineBehaviour
    {
        private Soldier i;
        private Vector3 p;
        private bool preached;

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            i = animator.gameObject.GetComponent<Soldier>();
            p = i.Sense<RandomOffensivePositionSensor, Vector3>();
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            // if near p or c
            if (Vector3.Magnitude(animator.transform.position - p) <= 15)
            {
                preached = true;
            }
            
            // if no flag, get flag
            if (!preached)
                i.Navigate(p);
            else if (!i.CarryingFlag)
                i.Navigate(i.EnemyFlagPosition);

            // Set variables
            animator.SetInteger("Health", i.Health);
            animator.SetBool("Enemy", i.DetectedEnemies.Count(e => e.Visible) > 0);
            animator.SetBool("CarryingFlag", i.CarryingFlag);
        }

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    
        //}
    }
}