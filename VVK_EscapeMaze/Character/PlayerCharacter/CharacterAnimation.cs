using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    CharacterMovement characterMovement;
    CharacterAction characterAction;
    Animator animationController;

    bool bIsJump;
    bool bIsMove;
    bool bIsWalk;
    bool bIsSprint;

    void Awake()
    {
        SetEssentialData();
    }

    void Update()
    {
        SetMovementData();
    }

    void SetEssentialData()
    {
        characterMovement = GetComponent<CharacterMovement>();
        characterAction = GetComponent<CharacterAction>();
        animationController = GetComponent<Animator>();
    }

    void SetMovementData()
    {
        animationController.SetBool("InAir", characterMovement.bIsJump);
        animationController.SetBool("Move", characterMovement.bIsMove);
        animationController.SetBool("Walk", characterMovement.bIsWalk);
        //animationController.SetBool("Sprint", characterMovement.bIsSprint);
    }

    public float CalculateDirection()
    {
        float moveAngle = Vector3.SignedAngle(transform.forward, characterMovement.currentMoveDirection, Vector3.up);
        
        return moveAngle;
    }
}
