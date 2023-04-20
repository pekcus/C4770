using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EasyAI;
using UnityEngine;

namespace Project
{
    public class SAttack : StateMachineBehaviour
    {
        private Soldier i;
        private Soldier.EnemyMemory target;
        
        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            i = animator.gameObject.GetComponent<Soldier>();
            //animator.SetInteger("Role", (int)i.Role);
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            // Steven's very kindly provided attacking template :)
            if (i.DetectedEnemies.Count == 0)
            {
                animator.SetBool("Enemy", false);
                i.NoTarget();
                return;
            }
            // kill what see, then what has flag, then what close
            target = i.DetectedEnemies.OrderBy(e => e.Visible).ThenBy(e => e.HasFlag).ThenBy(e => Vector3.Distance(i.transform.position, e.Position)).First();

            i.SetTarget(new()
            {
                Enemy = target.Enemy,
                Position = target.Position,
                Visible = target.Visible
            });
            
            // todo: switch gun based off of enemies and stuff
            
            // If have flag, go home... if collector or security, go to flag... if no, chase enemy!
            if (i.CarryingFlag)
                i.Navigate(i.BasePosition);
            else
                i.Navigate(i.Role == Soldier.SoldierRole.Collector || i.Role == Soldier.SoldierRole.Security ? i.EnemyFlagPosition : target.Position);
            
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
