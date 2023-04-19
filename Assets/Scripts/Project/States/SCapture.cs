using System.Collections;
using System.Collections.Generic;
using EasyAI;
using Project.Pickups;
using Project.Sensors;
using UnityEngine;

namespace Project
{
    public class SCapture : StateMachineBehaviour
    {
        private Agent a;
        private Soldier i;
        private Vector3 r;
        private bool rreached;
        
        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            a = animator.gameObject.GetComponent<Agent>();
            i = animator.gameObject.GetComponent<Soldier>();
            r = a.Sense<RandomOffensivePositionSensor, Vector3>();
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (i.DetectedEnemies.Count > 0)
            {
                animator.SetBool("Enemy", true);
                return;
            }

            if (Vector3.Magnitude(animator.transform.position - r) <= 10)
                rreached = true;
            if (i.CarryingFlag)
                a.Navigate(i.BasePosition);
            else if (!rreached)
                a.Navigate(r);
            else
                a.Navigate(i.EnemyFlagPosition);
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
