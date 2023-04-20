using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EasyAI;
using UnityEngine;

namespace Project
{
    /*
     * State for all Soldiers
     */
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

            // target what see, then what has flag, then what close
            target = i.DetectedEnemies.OrderBy(e => e.Visible).ThenBy(e => e.HasFlag)
                .ThenBy(e => Vector3.Distance(i.transform.position, e.Position)).First();

            
            if (i.DetectedEnemies.Count(e => e.Visible) > 2)
            {
                i.SetWeaponPriority(shotgun: 2, machineGun: 3, pistol: 4, rocketLauncher: 1, sniper: 5);
            }
            // if target far, snipe
            else if (Vector3.Distance(target.Position, i.transform.position) > 30)
            {
                i.SetWeaponPriority(shotgun: 5, machineGun: 4, pistol: 3, rocketLauncher: 2, sniper: 1);
            }
            else
            {
                i.SetWeaponPriority(shotgun: 3, machineGun: 2, pistol: 1, rocketLauncher: 5, sniper: 4);
            }

            i.SetTarget(new()
            {
                Enemy = target.Enemy,
                Position = target.Position,
                Visible = target.Visible
            });


            // if collector or security, go to flag... if no, chase enemy!
            i.Navigate(i.Role == Soldier.SoldierRole.Collector || i.Role == Soldier.SoldierRole.Security
                ? i.EnemyFlagPosition
                : target.Position);

            // Set variables
            animator.SetInteger("Health", i.Health);
            animator.SetInteger("Role", (int)i.Role);
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