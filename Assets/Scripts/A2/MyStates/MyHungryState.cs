using A2;
using A2.Sensors;
using EasyAI;
using EasyAI.Navigation;
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
        {
            animator.SetBool("HasTarget", false);
            return;
        }
            
        microbe.StartHunting(target);
        agent.Move(target.transform, Steering.Behaviour.Pursue);
        if (microbe.Eat())
        {   // set isHungry to false to return to Mind state.
            // If one meal wasn't enough, it can return to Hungry from Mind.
            animator.SetBool("IsHungry", false);
            microbe.RemoveTargetMicrobe();
        }
        // Update variables
        animator.SetBool("HasTarget", microbe.HasTarget);
        animator.SetBool("IsAdult", microbe.IsAdult);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
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
