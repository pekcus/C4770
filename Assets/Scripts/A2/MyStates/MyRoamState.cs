using System;
using System.Collections;
using System.Collections.Generic;
using A2;
using A2.Pickups;
using A2.Sensors;
using EasyAI;
using UnityEngine;
using Random = UnityEngine.Random;

public class MyRoamState : StateMachineBehaviour
{
    private Agent agent;
    private Microbe microbe;
    private MicrobeBasePickup pickup;
    private Vector3 p; 
    
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Set main variables
        agent = animator.gameObject.GetComponent<Agent>();
        microbe = agent.GetComponent<Microbe>();
        animator.SetBool("IsAdult", microbe.IsAdult);
        animator.SetBool("IsHungry", microbe.IsHungry);
        // Sense nearest pickup. If it's nearby, might as well try to grab it.
        pickup = agent.Sense<NearestPickupSensor, MicrobeBasePickup>();
        if ((agent.transform.position - pickup.transform.position).magnitude <= 8f)
        {
            microbe.SetPickup(pickup);
            animator.SetBool("HasPickup", true);
            p = pickup.transform.position;
        }
        // Small random movement - choose nearby position to move to
        p = agent.transform.position;
        p.x += Random.Range(-10f, 10f);
        p.z += Random.Range(-10f, 10f);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.Move(p);
        // if agent has arrived to random spot, set a new random spot
        if ((agent.transform.position - p).magnitude <= 0.2f)
        {
            p.x += Random.Range(-10f, 10f);
            p.z += Random.Range(-10f, 10f);
        }
        // Update variables
        animator.SetBool("IsAdult", microbe.IsAdult);
        animator.SetBool("IsHungry", microbe.IsHungry);
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
