using System;
using UnityEngine;

public class EnteredMoveStart : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float moveStartAngle = animator.gameObject.GetComponent<CharacterAnimation>().CalculateDirection();

        //Debug.Log("Angle : " + moveStartAngle);

        if (moveStartAngle >= -22.0f && moveStartAngle <= 22.0f)
        {
            animator.SetFloat("MoveAngle", 0.0f);
            return;
        }
        else if (moveStartAngle > 22.0f && moveStartAngle <= 67.0f)
        {
            animator.SetFloat("MoveAngle", 120.0f);
            return;
        }
        else if (moveStartAngle > 67.0f && moveStartAngle <= 112.0f)
        {
            animator.SetFloat("MoveAngle", 60.0f);
            return;
        }
        else if (moveStartAngle > 112.0f && moveStartAngle <= 157.0f)
        {
            animator.SetFloat("MoveAngle", 120.0f);
            return;
        }
        else if (moveStartAngle < -22.0f && moveStartAngle >= -67.0f)
        {
            animator.SetFloat("MoveAngle", -120.0f);
            return;
        }
        else if (moveStartAngle < -67.0f && moveStartAngle >= -112.0f)
        {
            animator.SetFloat("MoveAngle", -60.0f);
            return;
        }
        else if (moveStartAngle < -112.0f && moveStartAngle >= -157.0f)
        {
            animator.SetFloat("MoveAngle", -120.0f);
            return;
        }
        else if(moveStartAngle > 157.0f && moveStartAngle <= 180.0f)
        {
            animator.SetFloat("MoveAngle", 180.0f);
            return;
        }
        else if (moveStartAngle < -157.0f && moveStartAngle >= -180.0f)
        {
            animator.SetFloat("MoveAngle", -180.0f);
            return;
        }
    }
}
