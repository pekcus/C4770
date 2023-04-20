using System.Collections;
using System.Collections.Generic;
using EasyAI;
using System.Linq;
using Project.Pickups;
using Project.Sensors;
using UnityEngine;

namespace Project
{
    public class SDying : StateMachineBehaviour
    {
        private Soldier i;
        private HealthAmmoPickup hp;
        
        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            i = animator.gameObject.GetComponent<Soldier>();
            hp = i.Sense<NearestHealthPickupSensor, HealthAmmoPickup>();
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (!hp.Ready)
                hp = i.Sense<NearestHealthPickupSensor, HealthAmmoPickup>();
                
            // try run home if have flag, or get health
            i.Navigate(i.CarryingFlag ? i.BasePosition : hp.transform.position);
            
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