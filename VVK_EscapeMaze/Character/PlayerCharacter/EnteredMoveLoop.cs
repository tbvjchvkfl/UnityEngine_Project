using UnityEngine;

public class EnteredMoveLoop : StateMachineBehaviour
{
    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float leanAngle = animator.GetComponent<CharacterAnimation>().CalculateRotation();
        animator.SetFloat("LeanAngle", leanAngle);
    }
}
