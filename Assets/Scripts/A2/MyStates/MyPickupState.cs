using System.Collections;
using System.Collections.Generic;
using A2;
using A2.Pickups;
using A2.Sensors;
using EasyAI;
using UnityEngine;

public class MyPickupState : StateMachineBehaviour
{
    private Agent agent;
    private Microbe microbe;
    private MicrobeBasePickup pickup;
    
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Set main variables
        agent = animator.gameObject.GetComponent<Agent>();
        microbe = agent.GetComponent<Microbe>();
        pickup = agent.Sense<NearestPickupSensor, MicrobeBasePickup>();
        if (pickup != null)
            microbe.SetPickup(pickup);
        animator.SetBool("HasPickup", microbe.HasPickup);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Move to the pickup.
        if (microbe.HasPickup)
        {
            pickup = microbe.Pickup;
            agent.Move(pickup.transform.position);
        }
        // Update variables
        animator.SetBool("HasPickup", microbe.HasPickup);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Update variables
        animator.SetBool("HasPickup", false);
        microbe.RemovePickup();
    }

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
