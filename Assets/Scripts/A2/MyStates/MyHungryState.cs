using System.Collections;
using System.Collections.Generic;
using A2;
using A2.Sensors;
using EasyAI;
using UnityEngine;

public class MyHungryState : StateMachineBehaviour
{
    private Agent agent;
    private Microbe microbe;
    private Microbe target;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Set main variables
        agent = animator.gameObject.GetComponent<Agent>();
        microbe = agent.GetComponent<Microbe>();
        target = agent.Sense<NearestPreySensor, Microbe>();
        animator.SetBool("IsHungry", microbe.IsHungry);
        animator.SetBool("HasTarget", microbe.HasTarget);
    }

    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Get prey via sensor, hunt prey
        if (target == null)
            return;
        microbe.StartHunting(target);
        agent.Move(target.transform.position);
        if (microbe.Eat())
            animator.SetBool("IsHungry", false);
        // set isHungry to false to return to Roaming state.
        // If one meal wasn't enough, it can return to Hungry from Roaming.
        animator.SetBool("HasTarget", microbe.HasTarget);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Update variable
        microbe.RemoveTargetMicrobe();
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
