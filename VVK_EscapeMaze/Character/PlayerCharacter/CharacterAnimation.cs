using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    [Header("Object Component")]
    public Camera mainCamera;

    public GameObject Spine_Test;
    public GameObject Clavicle_L;
    public GameObject Clavicle_R;

    PCInputManager inputManager;
    CharacterMovement characterMovement;
    CharacterAction characterAction;
    Animator animationController;

    public float leaningAngle {  get; private set; }
    public float MoveStateIndex {  get; private set; }
    public float AimStateIndex {  get; private set; }

    void Awake()
    {
        SetEssentialData();
    }

    void Update()
    {
        SetMovementData();
    }

    void LateUpdate()
    {
        if(AimStateIndex == 0)
        {
            //Spine_Test.transform.localRotation = Quaternion.Euler(20.0f, 0.0f, 0.0f);
            //Clavicle_L.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
            //Clavicle_R.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        }
    }

    void SetEssentialData()
    {
        inputManager = GetComponent<PCInputManager>();
        characterMovement = GetComponent<CharacterMovement>();
        characterAction = GetComponent<CharacterAction>();
        animationController = GetComponent<Animator>();

        leaningAngle = 0.0f;
        MoveStateIndex = 2.0f;
        AimStateIndex = 1.0f;
    }

    void SetMovementData()
    {
        animationController.SetBool("InAir", inputManager.bIsJump);
        animationController.SetBool("Move", inputManager.inputDirection.magnitude > 0.1f);
        animationController.SetBool("Crouch", inputManager.bIsCrouch);
        animationController.SetBool("Aim", inputManager.bIsAim);
        animationController.SetFloat("Move State Index", SetMoveStateIndex());
        animationController.SetFloat("Aim State Index", SetAimStateIndex());

        characterMovement.SetRotationRate(animationController.GetFloat("Rotation Value For Script"));
        characterMovement.SetMoveSpeed(animationController.GetFloat("MoveSpeed For Script"));
    }

    float SetMoveStateIndex()
    {
        float CrouchTarget = 0.0f;
        float WalkTarget = 1.0f;
        float RunTarget = 2.0f;
        float SprintTarget = 3.0f;

        MoveStateIndex = inputManager.bIsCrouch ? Mathf.MoveTowards(MoveStateIndex, CrouchTarget, Time.deltaTime * 2.5f) :
                                 inputManager.bIsWalk ? Mathf.MoveTowards(MoveStateIndex, WalkTarget, Time.deltaTime) :
                                 inputManager.bIsSprint ? Mathf.MoveTowards(MoveStateIndex, SprintTarget, Time.deltaTime) :
                                                                Mathf.MoveTowards(MoveStateIndex, RunTarget, Time.deltaTime);
        return MoveStateIndex;
    }

    float SetAimStateIndex()
    {
        float AimTarget = 0.0f;
        float NormalTarget = 1.0f;

        float ReturnTarget = 1.0f;
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
        //float finalAngle = moveAngle * -1.0f;
        /*if((desiredMoveDirection.z < 0.0f && moveAngle > 0.0f) || 
            (desiredMoveDirection.z > 0.0f && moveAngle < 0.0f) ||
            (desiredMoveDirection.x < 0.0f && moveAngle > 0.0f)||
            (desiredMoveDirection.x > 0.0f && moveAngle < 0.0f))
        {
            finalAngle = moveAngle * -1.0f;
            Debug.Log("Reverse");
        }*/

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

    public void SetCrouchStateIndexInAnimClip()
    {
        if (inputManager.bIsCrouch && MoveStateIndex > 0.0f)
        {
            MoveStateIndex = 0.0f;
        }
    }
}
