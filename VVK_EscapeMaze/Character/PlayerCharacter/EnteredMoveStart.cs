using UnityEngine;

public class EnteredMoveStart : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.gameObject.GetComponent<CharacterAnimation>().AimStateIndex < 1.0f)
        {
            float moveStrafeAngle = animator.gameObject.GetComponent<CharacterAnimation>().CalculateDirection();
            if (moveStrafeAngle >= -45.0f && moveStrafeAngle <= 45.0f)
            {
                animator.SetFloat("MoveAngle", 0.0f);
            }
            else if (moveStrafeAngle >= -135.0f && moveStrafeAngle < -45.0f)
            {
                animator.SetFloat("MoveAngle", -90.0f);
            }
            else if (moveStrafeAngle >= -180.0f && moveStrafeAngle < -135.0f)
            {
                animator.SetFloat("MoveAngle", -180.0f);
            }
            else if (moveStrafeAngle > 45.0f && moveStrafeAngle <= 135.0f)
            {
                animator.SetFloat("MoveAngle", 90.0f);
            }
            else if (moveStrafeAngle > 135.0f && moveStrafeAngle <= 180.0f)
            {
                animator.SetFloat("MoveAngle", 180.0f);
            }
        }
        else
        {
            float moveStartAngle = animator.gameObject.GetComponent<CharacterAnimation>().CalculateDirection();

            if (moveStartAngle >= -22.0f && moveStartAngle <= 22.0f)
            {
                animator.SetFloat("MoveAngle", 0.0f);
            }
            else if (moveStartAngle > 22.0f && moveStartAngle <= 67.0f)
            {
                animator.SetFloat("MoveAngle", 120.0f);
            }
            else if (moveStartAngle > 67.0f && moveStartAngle <= 112.0f)
            {
                animator.SetFloat("MoveAngle", 60.0f);
            }
            else if (moveStartAngle > 112.0f && moveStartAngle <= 157.0f)
            {
                animator.SetFloat("MoveAngle", 120.0f);
            }
            else if (moveStartAngle < -22.0f && moveStartAngle >= -67.0f)
            {
                animator.SetFloat("MoveAngle", -120.0f);
            }
            else if (moveStartAngle < -67.0f && moveStartAngle >= -112.0f)
            {
                animator.SetFloat("MoveAngle", -60.0f);
            }
            else if (moveStartAngle < -112.0f && moveStartAngle >= -157.0f)
            {
                animator.SetFloat("MoveAngle", -120.0f);
            }
            else if (moveStartAngle > 157.0f && moveStartAngle <= 180.0f)
            {
                animator.SetFloat("MoveAngle", 180.0f);
            }
            else if (moveStartAngle < -157.0f && moveStartAngle >= -180.0f)
            {
                animator.SetFloat("MoveAngle", -180.0f);
            }
        }
    }
}
