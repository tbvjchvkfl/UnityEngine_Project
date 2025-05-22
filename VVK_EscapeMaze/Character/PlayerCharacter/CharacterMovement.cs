using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [Header("Locomotion")]
    public float maxWalkSpeed = 2.0f;
    public float maxRunSpeed = 5.0f;
    public float maxSprintSpeed = 10.0f;
    public float acceleration = 5.0f;
    public float deceleration = 10.0f;
    
    public float maxJumpPower = 2.0f;
    public float gravity = -9.81f;

    [Header("Component")]
    public GameObject CameraObj;


    // Component
    Camera mainCamera;
    PCInputManager inputManager;
    CharacterController characterController;
    

    // Move State Value
    public float moveSpeed { get; private set; }
    public float jumpSpeed { get; private set; }
    public float characterRotationRate {  get; private set; }

    public bool bIsJump { get; private set; }
    public bool bIsGround { get; private set; }
    public bool bIsMove { get; private set; }
    public bool bIsWalk { get; private set; }
    public bool bIsSprint { get; private set; }

    public Vector3 currentMoveDirection { get; private set; }
    public Vector3 lastMoveDirection { get; private set; }

    void Awake()
    {
        InitEssentialData();
    }

    void Update()
    {
        CheckMoveState();
    }

    void InitEssentialData()
    {
        mainCamera = CameraObj.GetComponent<Camera>();
        inputManager = GetComponent<PCInputManager>();
        characterController = GetComponent<CharacterController>();
        lastMoveDirection = transform.forward;
    }

    void CheckMoveState()
    {
        bIsWalk = inputManager.bIsWalk;
        bIsSprint = inputManager.bIsSprint;
        bIsGround = characterController.isGrounded;
    }

    void SetMoveDirection()
    {
        Vector3 CameraForwardVector = mainCamera.transform.forward;
        CameraForwardVector.y = 0.0f;
        CameraForwardVector.Normalize();

        Vector3 CameraRightVector = mainCamera.transform.right;
        CameraRightVector.y = 0.0f;
        CameraRightVector.Normalize();

        Vector3 desiredMoveDirection = (CameraForwardVector * inputManager.inputDirection.y + CameraRightVector * inputManager.inputDirection.x).normalized;

        // 감속 나아아아아아아중에 애니메이션 작업 전부 끝나고 테스트해볼 것
        //currentMoveDirection = Vector3.Lerp(currentMoveDirection, desiredMoveDirection, Time.fixedDeltaTime * 10.0f);
        /*float smoothTime = desiredMoveDirection.magnitude > 0.1f ? 0.05f : 0.15f;
        Vector3 vector = Vector3.zero;
        currentMoveDirection = Vector3.SmoothDamp(currentMoveDirection, desiredMoveDirection, ref vector, smoothTime);
        Debug.Log(vector);*/

        if(desiredMoveDirection.magnitude > 0.1f)
        {
            lastMoveDirection = desiredMoveDirection;
            currentMoveDirection = desiredMoveDirection;
        }
        else
        {
            currentMoveDirection = lastMoveDirection;
        }

        SetRotateDirection(currentMoveDirection);
    }

    void SetRotateDirection(Vector3 moveDirection)
    {
        if (moveDirection.magnitude > 0.1f)
        {
            Quaternion toRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, Time.fixedDeltaTime * characterRotationRate);
        }
    }

    void CalculateMoveSpeed()
    {
        if (inputManager.inputDirection.magnitude > 0.1f)
        {
            bIsMove = true;
            float speed = bIsSprint ? maxSprintSpeed : 
                          bIsWalk ? maxWalkSpeed : maxRunSpeed;

            moveSpeed = Mathf.MoveTowards(moveSpeed, speed, acceleration * Time.fixedDeltaTime);
        }
        else
        {
            bIsMove = false;
            moveSpeed = Mathf.MoveTowards(moveSpeed, 0.0f, deceleration * Time.fixedDeltaTime);
        }
    }

    void SetJumpSpeed()
    {
        if (characterController.isGrounded && inputManager.bIsJump)
        {
            if (jumpSpeed < 0.0f)
            {
                jumpSpeed = -2.0f;
            }
            if (inputManager.bIsJump)
            {
                jumpSpeed = Mathf.Sqrt(maxJumpPower * -2.0f * gravity);
            }
            bIsJump = true;
        }
        else
        {
            jumpSpeed += gravity * Time.deltaTime;
            bIsJump = false;
        }
    }

    public void SetRotationRate(float newrotationRate)
    {
        characterRotationRate = newrotationRate;
    }

    public void SetMoveSpeed(float newmoveSpeed)
    {
        moveSpeed = newmoveSpeed;
    }

    public void Move()
    {
        SetMoveDirection();
        CalculateMoveSpeed();
        SetJumpSpeed();
        
        Vector3 DesiredDirection = currentMoveDirection * moveSpeed;
        DesiredDirection.y = jumpSpeed;

        characterController.Move(DesiredDirection * Time.fixedDeltaTime);
    }
}
