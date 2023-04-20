using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Project.Sensors;
using UnityEngine;

namespace Project
{
    /*
     * State for Security/Support Role
     */
    public class SSecurity : StateMachineBehaviour
    {
        private Soldier i;
        private Soldier friend;

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            i = animator.gameObject.GetComponent<Soldier>();
            friend = i.GetTeam().First(f => f.Role == Soldier.SoldierRole.Collector);
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            // If friend die, find new friend
            if (friend != null && friend.Role == Soldier.SoldierRole.Dead)
                friend = i.GetTeam().OrderBy(f => f.CarryingFlag).ThenBy(f => f.Role == Soldier.SoldierRole.Collector)
                    .ThenBy(f => f.Role == Soldier.SoldierRole.Attacker).FirstOrDefault();
            // Note: if no flag carrier or collector found, choose an attacker as they have a chance of grabbing the flag

            // go to friend
            if (friend != null)
                i.Navigate(friend.transform.position);
            else
                i.Navigate(i.Sense<RandomOffensivePositionSensor, Vector3>());

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