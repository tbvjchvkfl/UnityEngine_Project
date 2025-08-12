using UnityEngine;

public class EnteredMoveStart : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        CharacterAnimation characterAnim = animator.GetComponent<CharacterAnimation>();

        float movementAngle = characterAnim.CalculateDirection();

        if (characterAnim.AimStateIndex < 0.0f)
        {
            if (movementAngle >= -45.0f && movementAngle <= 45.0f)
            {
                animator.SetFloat("MoveAngle", 0.0f);
            }
            else if (movementAngle >= -135.0f && movementAngle < -45.0f)
            {
                animator.SetFloat("MoveAngle", -90.0f);
            }
            else if (movementAngle >= -180.0f && movementAngle < -135.0f)
            {
                animator.SetFloat("MoveAngle", -180.0f);
            }
            else if (movementAngle > 45.0f && movementAngle <= 135.0f)
            {
                animator.SetFloat("MoveAngle", 90.0f);
            }
            else if (movementAngle > 135.0f && movementAngle <= 180.0f)
            {
                animator.SetFloat("MoveAngle", 180.0f);
            }
        }
        else
        {
            float deltaAngle = Vector3.SignedAngle(animator.gameObject.transform.forward, animator.gameObject.GetComponentInParent<CharacterMovement>().currentMoveDirection, Vector3.up);

            //Debug.Log(deltaAngle);

            if (movementAngle >= -22.0f && movementAngle <= 22.0f)
            {
                animator.SetFloat("MoveAngle", 0.0f);
            }
            else if (movementAngle > 22.0f && movementAngle <= 67.0f)
            {
                animator.SetFloat("MoveAngle", 120.0f);
            }
            else if (movementAngle > 67.0f && movementAngle <= 112.0f)
            {
                animator.SetFloat("MoveAngle", 60.0f);
            }
            else if (movementAngle > 112.0f && movementAngle <= 157.0f)
            {
                animator.SetFloat("MoveAngle", 120.0f);
            }
            else if (movementAngle > 157.0f && movementAngle <= 180.0f)
            {
                animator.SetFloat("MoveAngle", 180.0f);
            }
            else if (movementAngle < -22.0f && movementAngle >= -67.0f)
            {
                animator.SetFloat("MoveAngle", -120.0f);
            }
            else if (movementAngle < -67.0f && movementAngle >= -112.0f)
            {
                animator.SetFloat("MoveAngle", -60.0f);
            }
            else if (movementAngle < -112.0f && movementAngle >= -157.0f)
            {
                animator.SetFloat("MoveAngle", -120.0f);
            }
            else if (movementAngle < -157.0f && movementAngle >= -180.0f)
            {
                animator.SetFloat("MoveAngle", -180.0f);
            }
        }
    }
}
