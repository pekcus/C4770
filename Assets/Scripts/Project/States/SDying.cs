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
     * State for all Soldiers
     */
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
            if (!hp.Ready || hp == null)
                hp = i.Sense<NearestHealthPickupSensor, HealthAmmoPickup>();

            // get health
            if (hp != null)
                i.Navigate(hp.transform.position);

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