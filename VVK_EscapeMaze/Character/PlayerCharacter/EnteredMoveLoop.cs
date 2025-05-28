using UnityEngine;

public class EnteredMoveLoop : StateMachineBehaviour
{
    float MovementAngle = 0.0f;

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.gameObject.GetComponent<CharacterAnimation>().AimStateIndex < 1.0f)
        {
            float moveStrafeAngle = animator.gameObject.GetComponent<CharacterAnimation>().CalculateDirection();
            float TargetAngle = 0.0f;
            if (moveStrafeAngle >= -22.0f && moveStrafeAngle <= 22.0f)
            {
                TargetAngle = 0.0f;
            }
            else if (moveStrafeAngle > 22.0f && moveStrafeAngle <= 67.0f)
            {
                TargetAngle = 45.0f;
            }
            else if (moveStrafeAngle > 67.0f && moveStrafeAngle <= 112.0f)
            {
                TargetAngle = 90.0f;
            }
            else if (moveStrafeAngle > 112.0f && moveStrafeAngle <= 157.0f)
            {
                TargetAngle = 135.0f;
            }
            else if (moveStrafeAngle > 157.0f && moveStrafeAngle <= 180.0f)
            {
                TargetAngle = 180.0f;
            }
            else if (moveStrafeAngle < -22.0f && moveStrafeAngle >= -67.0f)
            {
                TargetAngle = -45.0f;
            }
            else if (moveStrafeAngle < -67.0f && moveStrafeAngle >= -112.0f)
            {
                TargetAngle = -90.0f;
            }
            else if (moveStrafeAngle < -112.0f && moveStrafeAngle >= -157.0f)
            {
                TargetAngle = -135.0f;
            }
            else if (moveStrafeAngle < -157.0f && moveStrafeAngle >= -180.0f)
            {
                TargetAngle = -180.0f;
            }


            if (TargetAngle == 180.0f && MovementAngle <= 90.0f || TargetAngle == -180.0f && MovementAngle>= -90.0f)
            {
                MovementAngle = 180.0f;
            }
            else if (TargetAngle == 0.0f && MovementAngle>= 90.0f || TargetAngle == 0.0f && MovementAngle<= -90.0f)
            {

            }
            MovementAngle = Mathf.MoveTowards(MovementAngle, TargetAngle, Time.deltaTime * 200.0f);
            animator.SetFloat("MoveAngle", MovementAngle);
        }
        else
        {
            float leanAngle = animator.GetComponent<CharacterAnimation>().CalculateRotation();
            animator.SetFloat("LeanAngle", leanAngle);
        }
    }
}
