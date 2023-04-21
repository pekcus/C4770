using System.Collections;
using System.Collections.Generic;
using EasyAI;
using System.Linq;
using Project.Pickups;
using Project.Sensors;
using UnityEngine;

namespace Project
{
    /*
     * State for Attackers and Defenders when out of combat
     */
    public class SRoam : StateMachineBehaviour
    {
        private Soldier i;
        private bool def;
        private Vector3 p;
        private Vector3 q;
        private bool top;
        private HealthAmmoPickup a;

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            i = animator.gameObject.GetComponent<Soldier>();
            def = i.Role == Soldier.SoldierRole.Defender;
            q = i.Sense<RandomOffensivePositionSensor, Vector3>(); // first q only
            p = i.EnemyFlagPosition; // first p only
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (Vector3.Distance(i.transform.position, p) < 0.1f)
            {
                // reset p
                if (top && Vector3.Distance(i.transform.position, p) <= 0.1f)
                {
                    top = false;
                    // if attacker, get enemy flag
                    if (i.Role == Soldier.SoldierRole.Attacker)
                        p = i.EnemyFlagPosition;
                    // get pickup
                    else if ((a = i.Sense<NearestAmmoPickupSensor, HealthAmmoPickup>()) != null)
                        p = a.transform.position;
                }

                // reset q
                if (!top && Vector3.Distance(i.transform.position, q) <= 0.1f)
                {
                    top = true;
                    q = def
                        ? i.Sense<RandomDefensivePositionSensor, Vector3>()
                        : i.Sense<RandomOffensivePositionSensor, Vector3>();
                }
            }

            // go to p or q
            if (top)
                i.Navigate(p);
            else 
                i.Navigate(q);

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