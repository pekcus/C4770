using A2;
using EasyAI;
using EasyAI.Navigation;
using UnityEngine;

public class MyHuntedState : StateMachineBehaviour
{
    private Agent agent;
    private Microbe microbe;
    private Microbe hunter;
    
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Set main variables
        agent = animator.gameObject.GetComponent<Agent>();
        microbe = agent.GetComponent<Microbe>();
        hunter = microbe.Hunter;
        animator.SetBool("BeingHunted", microbe.BeingHunted);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (microbe != null && microbe.Hunter != null)
        {
            hunter = microbe.Hunter;
            agent.Move(hunter.transform, Steering.Behaviour.Evade);
        }
        // Update variable
        animator.SetBool("BeingHunted", microbe.BeingHunted);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       // P E R I S H </3
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
