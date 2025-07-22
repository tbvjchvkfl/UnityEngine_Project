using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    [Header("Object Component")]
    public Camera mainCamera;

    public GameObject head_Bone;
    public GameObject spine_Bone;

    public BoxCollider SkillRangeBox;

    PCInputManager inputManager;
    CharacterMovement characterMovement;
    CharacterAction characterAction;
    Animator animationController;
    Coroutine SkillCoroutine;

    public float leaningAngle {  get; private set; }
    public float MoveStateIndex {  get; private set; }
    public float AimStateIndex {  get; private set; }
    public float SpeedValue { get; private set; }

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
        inputManager = GetComponentInParent<PCInputManager>();
        characterMovement = GetComponentInParent<CharacterMovement>();
        characterAction = GetComponentInParent<CharacterAction>();
        animationController = GetComponent<Animator>();

        leaningAngle = 0.0f;
        MoveStateIndex = 0.0f;
        AimStateIndex = 0.0f;
    }

    void SetMovementData()
    {
        animationController.SetBool("Step", inputManager.bIsStep);
        animationController.SetBool("Dodge", inputManager.bIsDodge);
        animationController.SetBool("Move", inputManager.inputDirection.magnitude > 0.1f);
        animationController.SetBool("Aim", inputManager.bIsAim);
        animationController.SetBool("Attack", inputManager.bIsNormalAttack);
        animationController.SetBool("Hit", inputManager.bIsHit);
        animationController.SetBool("Equip", inputManager.bIsEquip);
        animationController.SetBool("Skill Ready", inputManager.bIsSkillReady);
        animationController.SetBool("Skill Activate", characterAction.bIsSkillActivate);
        animationController.SetFloat("Skill Index", characterAction.SkillIndex);

        animationController.SetFloat("Move State Index", SetMoveStateIndex());
        animationController.SetFloat("Aim State Index", SetAimStateIndex());

        characterMovement.SetRotationRate(animationController.GetFloat("Rotation Value For Script"));
        characterMovement.SetMoveSpeed(SetMoveSpeedValue());
    }

    float SetMoveSpeedValue()
    {
        if (inputManager.bIsEquip)
        {
            if(inputManager.inputDirection.magnitude > 0.1f)
            {
                if (MoveStateIndex < 1.0f)
                {
                    SpeedValue = 2.0f;
                }
                else
                {
                    SpeedValue = 5.0f;
                }
            }
            else
            {
                SpeedValue = 0.0f;
            }
        }
        else
        {
            SpeedValue = animationController.GetFloat("MoveSpeed For Script");
        }

        return SpeedValue;
    }

    float SetMoveStateIndex()
    {
        float WalkTarget = 0.0f;
        float RunTarget = 1.0f;
        
        MoveStateIndex = inputManager.bIsSprint ? Mathf.MoveTowards(MoveStateIndex, RunTarget, Time.deltaTime * 2.5f) : Mathf.MoveTowards(MoveStateIndex, WalkTarget, Time.deltaTime * 2.5f);
        return MoveStateIndex;
    }

    float SetAimStateIndex()
    {
        float AimTarget = -1.0f;
        float NormalTarget = 0.0f;

        float ReturnTarget = 0.0f;
        if (inputManager.bIsAim)
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

        Vector3 desiredMoveDirection = (CameraForwardVector * inputManager.inputDirection.y + CameraRightVector * inputManager.inputDirection.x).normalized;

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
        while (characterAction.bIsSkillActivate)
        {
            Collider[] HitList = Physics.OverlapBox(SkillRangeBox.bounds.center, SkillRangeBox.bounds.extents / 2, Quaternion.identity, ~0);
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