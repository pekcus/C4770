using System.Collections;
using System.Collections.Generic;
using EasyAI;
using System.Linq;
using Project.Sensors;
using UnityEngine;

namespace Project
{
    public class SRoam : StateMachineBehaviour
    {
        private Soldier i;
        private Vector3 p;
        private bool def;
        
        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            i = animator.gameObject.GetComponent<Soldier>();
            def = i.Role == Soldier.SoldierRole.Defender;
            p = def ? i.Sense<RandomDefensivePositionSensor, Vector3>() : i.Sense<RandomOffensivePositionSensor, Vector3>();
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            // if flag gone, find flag
            if (Vector3.Magnitude(i.BasePosition - i.TeamFlagPosition) >= 1)
                p = i.TeamFlagPosition;
            // if attacker, get enemy flag
            if (i.Role == Soldier.SoldierRole.Attacker)
                p = i.EnemyFlagPosition;
            // if close to p, choose new p
            if (Vector3.Magnitude(animator.transform.position - p) < 0.1f)
                p = def ? i.Sense<RandomDefensivePositionSensor, Vector3>() : i.Sense<RandomOffensivePositionSensor, Vector3>();
            
            // go to p
            i.Navigate(p);
            
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
