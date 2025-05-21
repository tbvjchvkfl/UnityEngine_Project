using UnityEngine;

public class EnteredMoveStart : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float moveStartAngle = animator.GetComponent<CharacterAnimation>().CalculateDirection();
        
        if (moveStartAngle >= -22.0f && moveStartAngle <= 22.0f)
        {
            animator.SetFloat("MoveAngle", 0.0f);// 전방
        }
        else if (moveStartAngle > 22.0f && moveStartAngle <= 67.0f)
        {
            animator.SetFloat("MoveAngle", 120.0f); // 오른쪽 45
        }
        else if (moveStartAngle > 67.0f && moveStartAngle <= 112.0f)
        {
            animator.SetFloat("MoveAngle", 60.0f);// 오른쪽 90
        }
        else if (moveStartAngle > 112.0f && moveStartAngle <= 157.0f)
        {
            animator.SetFloat("MoveAngle", 120.0f); //오른쪽 135
        }
        else if (moveStartAngle > 157.0f && moveStartAngle <= 180.0f)
        {
            animator.SetFloat("MoveAngle", 180.0f); // 오른쪽 180
        }
        else if(moveStartAngle < -22.0f && moveStartAngle >= -67.0f)
        {
            animator.SetFloat("MoveAngle", -120.0f);// 왼쪽 45
        }
        else if (moveStartAngle < -67.0f && moveStartAngle >= -112.0f)
        {
            animator.SetFloat("MoveAngle", -60.0f);// 왼쪽 90
        }
        else if (moveStartAngle < -112.0f && moveStartAngle >= -157.0f)
        {
            animator.SetFloat("MoveAngle", -120.0f);// 왼쪽 135
        }
        else if (moveStartAngle < -157.0f && moveStartAngle >= -180.0f)
        {
            animator.SetFloat("MoveAngle", -180.0f);// 왼쪽 180
        }
    }
}
