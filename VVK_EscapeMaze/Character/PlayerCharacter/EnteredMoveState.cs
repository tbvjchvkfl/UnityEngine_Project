using UnityEngine;

public class EnteredMoveState : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float moveStartAngle = animator.GetComponent<CharacterAnimation>().CalculateDirection();
        
        if (moveStartAngle >= -45.0f && moveStartAngle <= 45.0f)
        {
            animator.SetFloat("MoveAngle", 0.0f);
        }
        else if (moveStartAngle < -45.0f && moveStartAngle >= -135.0f)
        {
            animator.SetFloat("MoveAngle", 0.25f);
        }
        else if (moveStartAngle < -135.0f && moveStartAngle <= 180.0f)
        {
            animator.SetFloat("MoveAngle", 0.5f);
        }
        else if (moveStartAngle > 45.0f && moveStartAngle <= 135.0f)
        {
            animator.SetFloat("MoveAngle", 0.75f);
        }
        else if(moveStartAngle > 135.0f && moveStartAngle <= 180.0f)
        {
            animator.SetFloat("MoveAngle", 1.0f);
        }
    }
}
