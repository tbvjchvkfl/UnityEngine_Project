using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [Header("Locomotion")]
    public float maxMoveSpeed = 1.0f;

    public float maxJumpPower = 2.0f;
    public float gravity = -9.81f;

    [Header("Object Component")]
    public GameObject CameraObj;


    // Component
    Camera mainCamera;
    PCInputManager inputManager;
    CharacterController characterController;
    

    // Move State Value
    public float moveSpeed { get; private set; }
    public float jumpSpeed { get; private set; }
    public float characterRotationRate {  get; private set; }

    public Vector3 currentMoveDirection { get; private set; }
    public Vector3 lastMoveDirection { get; private set; }

    void Awake()
    {
        InitEssentialData();
    }

    void Update()
    {
    }

    void InitEssentialData()
    {
        mainCamera = CameraObj.GetComponent<Camera>();
        inputManager = GetComponent<PCInputManager>();
        characterController = GetComponent<CharacterController>();
        lastMoveDirection = transform.forward;
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
            //bIsJump = true;
        }
        else
        {
            jumpSpeed += gravity * Time.deltaTime;
            //bIsJump = false;
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
        SetJumpSpeed();
        
        Vector3 DesiredDirection = currentMoveDirection * moveSpeed * maxMoveSpeed;
        DesiredDirection.y = jumpSpeed;

        characterController.Move(DesiredDirection * Time.fixedDeltaTime);
    }
}
