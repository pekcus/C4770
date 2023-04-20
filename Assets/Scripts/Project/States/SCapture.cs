using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EasyAI;
using Project.Pickups;
using Project.Sensors;
using UnityEngine;

namespace Project
{
    public class SCapture : StateMachineBehaviour
    {
        private Soldier i;
        private Vector3 p;
        private Vector3 c;
        private bool preached;
        private bool creached;
        
        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            i = animator.gameObject.GetComponent<Soldier>();
            p = i.Sense<RandomOffensivePositionSensor, Vector3>();
            c = i.Sense<RandomDefensivePositionSensor, Vector3>();
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (Vector3.Magnitude(animator.transform.position - p) <= 15)
            {
                preached = true;
                creached = false;
            }
            else if (Vector3.Magnitude(animator.transform.position - c) <= 15)
                creached = true;
            
            if (!preached)
                i.Navigate(p);
            else if (!i.CarryingFlag)
                i.Navigate(i.EnemyFlagPosition);
            else if (!creached)
                i.Navigate(c);
            else
                i.Navigate(i.BasePosition);
            
            // Set variables
            animator.SetInteger("Health", i.Health);
            animator.SetBool("Enemy", i.DetectedEnemies.Count(e => e.Visible) > 0);
        }

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    
        //}

        // OnStateMove is called right after Animator.OnAnimatorMove()
        //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    // Implement code that processes and affects root motion
        //}

        // OnStateIK is called right after Animator.OnAnimatorIK()
        //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    // Implement code that sets up animation IK (inverse kinematics)
        //}
    }
}
