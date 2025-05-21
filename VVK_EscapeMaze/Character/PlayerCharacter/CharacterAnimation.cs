using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    public Camera mainCamera;

    CharacterMovement characterMovement;
    CharacterAction characterAction;
    Animator animationController;

    float leaningAngle = 0.5f;
    float MoveStateIndex = 1.0f;


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
        animationController.SetBool("Ground", characterMovement.bIsGround);
        animationController.SetBool("Sprint", characterMovement.bIsSprint);

        SetAnimDataMoveStateIndex();
    }

    void SetAnimDataMoveStateIndex()
    {
        float WalkTarget = 0.0f;
        float RunTarget = 1.0f;
        MoveStateIndex = characterMovement.bIsWalk? WalkTarget : RunTarget;
        animationController.SetFloat("State Index", MoveStateIndex);
    }

    public float CalculateDirection()
    {
        float moveAngle = Vector3.SignedAngle(transform.forward, characterMovement.currentMoveDirection, Vector3.up);
        
        return moveAngle;
    }

    public float CalculateRotation()
    {
        float rotAngle = Vector3.SignedAngle(transform.forward, characterMovement.currentMoveDirection, Vector3.up);
        float targetLean = 0.5f;

        //Debug.Log(rotAngle);

        // 왼쪽
        if (rotAngle < -10.0f)
        {
            targetLean = 0.0f;
        }
        // 오른쪽
        else if (rotAngle > 10.0f)
        {
            targetLean = 1.0f;
        }
        // 가운데
        else
        {
            targetLean = 0.5f;
        }

        leaningAngle = Mathf.MoveTowards(leaningAngle, targetLean, Time.deltaTime);
        return leaningAngle;
    }
}
