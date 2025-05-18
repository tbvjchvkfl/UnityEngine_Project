using UnityEngine;

public class EnteredMoveState : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float moveStartAngle = animator.GetComponent<CharacterAnimation>().CalculateDirection();
        
        if (moveStartAngle >= -45.0f && moveStartAngle <= 45.0f)
        {
            Debug.Log("Forward");
            animator.SetFloat("Angle", 0.0f);
        }
        else if (moveStartAngle < -45.0f && moveStartAngle >= -135.0f)
        {
            Debug.Log("Turn Left 90");
            animator.SetFloat("Angle", 0.25f);
        }
        else if (moveStartAngle < -135.0f && moveStartAngle <= 180.0f)
        {
            Debug.Log("Turn Left 180");
            animator.SetFloat("Angle", 0.5f);
        }
        else if (moveStartAngle > 45.0f && moveStartAngle <= 135.0f)
        {
            Debug.Log("Turn Right 90");
            animator.SetFloat("Angle", 0.75f);
        }
        else if(moveStartAngle > 135.0f && moveStartAngle <= 180.0f)
        {
            Debug.Log("Turn Right 180");
            animator.SetFloat("Angle", 1.0f);
        }
        Debug.Log(moveStartAngle);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

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
