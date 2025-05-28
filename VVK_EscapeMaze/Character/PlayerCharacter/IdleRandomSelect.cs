using UnityEngine;

public class IdleRandomSelect : StateMachineBehaviour
{
    bool animationChangeFlag = false;

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.normalizedTime % 1.0f < 0.1f)
        {
            if(!animationChangeFlag)
            {
                int RandNum = Random.Range(0, 6);

                animator.SetFloat("Idle Random Index", RandNum);
                animationChangeFlag = true;
            }
        }
        else
        {
            animationChangeFlag = false;
        }
    }
}
