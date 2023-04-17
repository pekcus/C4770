using System.Collections;
using System.Collections.Generic;
using EasyAI;
using Project.Pickups;
using UnityEngine;

namespace Project
{
    public class SCapture : StateMachineBehaviour
    {
        private Agent a;
        private Soldier i;
        private GameObject flag;
        
        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            a = animator.gameObject.GetComponent<Agent>();
            i = animator.gameObject.GetComponent<Soldier>();
            flag = GameObject.Find(i.RedTeam ? "BlueFlag" : "RedFlag");
            //flag = GameObject.Find("Flag").GetComponent<FlagPickup>().RedFlag;
            //animator.SetInteger("Role", (int)i.Role);
            
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            a.Move(flag.transform.position);
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
