using System.Collections;
using System.Collections.Generic;
using A2;
using A2.Sensors;
using EasyAI;
using UnityEngine;

public class MyMateState : StateMachineBehaviour
{
    private Agent agent;
    private Microbe microbe;
    private Microbe mate;
    
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Set main variables
        agent = animator.gameObject.GetComponent<Agent>();
        microbe = agent.GetComponent<Microbe>();
        mate = agent.Sense<NearestMateSensor, Microbe>();
        // Successful attraction will set a target.
        // Unsuccessful attraction leads agent back to Roaming state.
        microbe.AttractMate(mate);
        animator.SetBool("HasTarget", microbe.HasTarget);
        animator.SetBool("DidMate", microbe.DidMate);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (microbe.HasTarget)
        {
            // Move to mate target and try to mate.
            agent.Move(mate.transform.position);
            microbe.Mate();
        }

        // Update variables
        animator.SetBool("DidMate", microbe.DidMate);
        if (microbe.DidMate)
            microbe.RemoveTargetMicrobe();
        animator.SetBool("HasTarget", microbe.HasTarget);
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
