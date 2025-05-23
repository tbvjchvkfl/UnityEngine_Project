using System;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    [Header("Object Component")]
    public Camera mainCamera;

    PCInputManager inputManager;
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
        inputManager = GetComponent<PCInputManager>();
        characterMovement = GetComponent<CharacterMovement>();
        characterAction = GetComponent<CharacterAction>();
        animationController = GetComponent<Animator>();
    }

    void SetMovementData()
    {
        animationController.SetBool("InAir", inputManager.bIsJump);
        animationController.SetBool("Move", inputManager.inputDirection.magnitude > 0.1f);
        animationController.SetFloat("State Index", SetMoveStateIndex());
        characterMovement.SetRotationRate(animationController.GetFloat("Rotation Value For Script"));
        characterMovement.SetMoveSpeed(animationController.GetFloat("MoveSpeed For Script"));
    }

    float SetMoveStateIndex()
    {
        float WalkTarget = 0.0f;
        float RunTarget = 1.0f;
        float SprintTarget = 2.0f;

        MoveStateIndex = inputManager.bIsWalk ? Mathf.MoveTowards(MoveStateIndex, WalkTarget, Time.deltaTime) :
                         inputManager.bIsSprint ? Mathf.MoveTowards(MoveStateIndex, SprintTarget, Time.deltaTime) :
                                                        Mathf.MoveTowards(MoveStateIndex, RunTarget, Time.deltaTime);

        return MoveStateIndex;
    }

    public float CalculateDirection()
    {
        float moveAngle = Vector3.SignedAngle(transform.forward, characterMovement.currentMoveDirection, Vector3.up);
        //float rotDirection = Vector3.SignedAngle(preMoveDirection, characterMovement.currentMoveDirection, Vector3.up);

        //preMoveDirection = characterMovement.currentMoveDirection;

        return moveAngle;
    }

    public float CalculateRotation()
    {
        float rotAngle = Vector3.SignedAngle(transform.forward, characterMovement.currentMoveDirection, Vector3.up);
        float targetLean = 0.5f;

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
