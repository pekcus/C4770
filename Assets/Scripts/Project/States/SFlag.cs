using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Project
{
    /*
     * State for Current Flag Carrier (regardless of Role)
     */
    public class SFlag : StateMachineBehaviour
    {
        private Soldier i;

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            i = animator.gameObject.GetComponent<Soldier>();
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            // rumn home!!!
            i.Navigate(i.BasePosition);
            
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
