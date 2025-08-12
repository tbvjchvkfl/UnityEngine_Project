using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    public BoxCollider SkillRangeBox;

    PlayerController playerController;
    CharacterMovement characterMovement;
    CharacterAction characterAction;
    Animator animationController;
    Camera mainCamera;

    Coroutine SkillCoroutine;

    public float leaningAngle {  get; private set; }
    public float MoveStateIndex {  get; private set; }
    public float AimStateIndex {  get; private set; }
    public float SpeedValue { get; private set; }
    bool bIsInitSuccess = false;

    void Update()
    {
        if (bIsInitSuccess)
        {
            SetMovementData();
        }
    }

    public void SetEssentialData(Camera cam)
    {
        playerController = GetComponent<PlayerController>();
        characterMovement = GetComponent<CharacterMovement>();
        characterAction = GetComponent<CharacterAction>();
        animationController = GetComponent<Animator>();
        mainCamera = cam;
        bIsInitSuccess = true;

        leaningAngle = 0.0f;
        MoveStateIndex = 0.0f;
        AimStateIndex = 0.0f;
    }

    void SetMovementData()
    {
        animationController.SetBool("Step", playerController.bIsStep);
        animationController.SetBool("Dodge", playerController.bIsDodge);
        animationController.SetBool("Move", playerController.inputDirection.magnitude > 0.1f);
        animationController.SetBool("Aim", playerController.bIsAim);
        animationController.SetBool("Attack", playerController.bIsNormalAttack);
        animationController.SetBool("Hit", playerController.bIsHit);
        animationController.SetBool("Equip", playerController.bIsEquip);
        animationController.SetBool("Skill Ready", playerController.bIsSkillReady);
        animationController.SetBool("Skill Activate", characterAction.bIsSkillActivate);
        animationController.SetFloat("Skill Index", characterAction.SkillIndex);

        animationController.SetFloat("Move State Index", SetMoveStateIndex());
        animationController.SetFloat("Aim State Index", SetAimStateIndex());

        characterMovement.SetRotationRate(animationController.GetFloat("Rotation Value For Script"));
        characterMovement.SetMoveSpeed(animationController.GetFloat("MoveSpeed For Script"));
    }

    float SetMoveStateIndex()
    {
        float WalkTarget = 0.0f;
        float RunTarget = 1.0f;
        
        MoveStateIndex = playerController.bIsSprint ? Mathf.MoveTowards(MoveStateIndex, RunTarget, Time.deltaTime * 2.5f) : Mathf.MoveTowards(MoveStateIndex, WalkTarget, Time.deltaTime * 2.5f);
        return MoveStateIndex;
    }

    float SetAimStateIndex()
    {
        float AimTarget = -1.0f;
        float NormalTarget = 0.0f;

        float ReturnTarget = 0.0f;
        if (playerController.bIsAim)
        {
            ReturnTarget = AimTarget;
            float LayerWeight = Mathf.MoveTowards(animationController.GetLayerWeight(1), 1.0f, Time.deltaTime * 5.0f);
            animationController.SetLayerWeight(1, LayerWeight);
        }
        else
        {
            ReturnTarget = NormalTarget;
            float LayerWeight = Mathf.MoveTowards(animationController.GetLayerWeight(1), 0.0f, Time.deltaTime * 5.0f);
            animationController.SetLayerWeight(1, LayerWeight);
        }

        AimStateIndex = Mathf.MoveTowards(AimStateIndex, ReturnTarget, Time.deltaTime * 2.5f);
        return AimStateIndex;
    }

    public float CalculateDirection()
    {
        Vector3 CameraForwardVector = mainCamera.transform.forward;
        CameraForwardVector.y = 0.0f;
        CameraForwardVector.Normalize();

        Vector3 CameraRightVector = mainCamera.transform.right;
        CameraRightVector.y = 0.0f;
        CameraRightVector.Normalize();

        Vector3 desiredMoveDirection = (CameraForwardVector * playerController.inputDirection.y + CameraRightVector * playerController.inputDirection.x).normalized;

        float moveAngle = Vector3.SignedAngle(transform.forward, desiredMoveDirection, Vector3.up);

        if (Mathf.Abs(moveAngle - 180.0f) < 1.0f)
            moveAngle = -180.0f;
        return moveAngle;
    }

    public float CalculateRotation()
    {
        float rotAngle = Vector3.SignedAngle(transform.forward, characterMovement.currentMoveDirection, Vector3.up);
        float targetLean = 0.0f;

        // 왼쪽
        if (rotAngle < -10.0f)
        {
            targetLean = -1.0f;
        }
        // 오른쪽
        else if (rotAngle > 10.0f)
        {
            targetLean = 1.0f;
        }
        // 가운데
        else
        {
            targetLean = 0.0f;
        }

        leaningAngle = Mathf.MoveTowards(leaningAngle, targetLean, Time.deltaTime);
        return leaningAngle;
    }

    public void SetDodgeDirection()
    {
        float dodgeAngle = CalculateDirection();

        if (dodgeAngle >= -45.0f && dodgeAngle <= 45.0f)
        {
            animationController.SetFloat("DodgeAngle", 0.0f);
        }
        else if (dodgeAngle < -45.0f && dodgeAngle >= -135.0f)
        {
            animationController.SetFloat("DodgeAngle", -90.0f);
        }
        else if (dodgeAngle < -135.0f && dodgeAngle >= -180.0f)
        {
            animationController.SetFloat("DodgeAngle", -180.0f);
        }
        else if (dodgeAngle > 45.0f && dodgeAngle <= 135.0f)
        {
            animationController.SetFloat("DodgeAngle", 90.0f);
        }
        else if(dodgeAngle > 135.0f && dodgeAngle <= 180.0f)
        {
            animationController.SetFloat("DodgeAngle", 180.0f);
        }
    }

    // Animation Notify ver.
    public void SkillBegin()
    {
        if (SkillCoroutine == null)
        {
            SkillCoroutine = StartCoroutine(SkillActivate());
        }
    }

    IEnumerator SkillActivate()
    {
        while (characterAction.bIsSkillActivate && !playerController.bIsHit && !playerController.bIsDead)
        {
            Collider[] HitList = Physics.OverlapBox(SkillRangeBox.bounds.center, SkillRangeBox.bounds.extents, Quaternion.identity, ~0);
            foreach (Collider hit in HitList)
            {
                if (hit.gameObject.CompareTag("Enemy"))
                {
                    EnemyStateMachine EnemySM = hit.gameObject.GetComponent<EnemyStateMachine>();
                    if (EnemySM)
                    {
                        EnemySM.OnTakeDamage(2.0f);
                        Debug.Log("Enemy Damage");
                    }
                }
            }
            yield return null;
        }
        if (playerController.bIsHit || playerController.bIsDead)
        {
            SkillCoroutine = null;
            characterAction.bIsSkillActivate = false;
        }
    }

    public void SkillEnd()
    {
        if (SkillCoroutine != null)
        {
            StopCoroutine(SkillCoroutine);
            SkillCoroutine = null;
        }
        characterAction.bIsSkillActivate = false;
    }
}