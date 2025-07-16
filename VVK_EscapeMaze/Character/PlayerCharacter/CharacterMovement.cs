using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [Header("Locomotion")]
    public float maxMoveSpeed = 1.0f;
    public float maxJumpPower = 2.0f;

    [Header("Object Component")]
    public GameObject CameraObj;


    // Component
    Camera mainCamera;
    PCInputManager inputManager;
    CharacterController characterController;
    

    // Move State Value
    public float moveSpeed { get; private set; }
    public float characterRotationRate {  get; private set; }
    public float GravityAcceleration { get; private set; } = 0.0f;

    public Vector3 currentMoveDirection { get; private set; }
    public Vector3 lastInputDirection {  get; private set; }

    public void InitEssentialData()
    {
        mainCamera = CameraObj.GetComponent<Camera>();
        inputManager = GetComponent<PCInputManager>();
        characterController = GetComponent<CharacterController>();
        lastInputDirection = transform.forward;
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
            lastInputDirection = desiredMoveDirection;
            currentMoveDirection = desiredMoveDirection;
        }
        else
        {
            currentMoveDirection = Vector3.zero;
        }

        SetRotateDirection(currentMoveDirection);
    }

    void SetRotateDirection(Vector3 moveDirection)
    {
        Quaternion toRotation = Quaternion.identity;

        if (inputManager.bIsEquip || inputManager.bIsAim)
        {
            toRotation = Quaternion.LookRotation(mainCamera.transform.forward);
        }
        else if(moveDirection.magnitude > 0.1f)
        {
            toRotation = Quaternion.LookRotation(moveDirection);
        }
        else
        {
            return;
        }
        toRotation.x = 0.0f;
        toRotation.z = 0.0f;

        transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, Time.fixedDeltaTime * characterRotationRate);
    }

    float SetGravity()
    {
        if (!characterController.isGrounded)
        {
            GravityAcceleration += -9.81f * Time.deltaTime;
        }
        else
        {
            GravityAcceleration = -2.0f;
        }
        return GravityAcceleration;
    }

    public void SetRotationRate(float newrotationRate)
    {
        characterRotationRate = newrotationRate;
    }

    public void SetMoveSpeed(float newmoveSpeed)
    {
        moveSpeed = newmoveSpeed;
    }

    public void StepAndDodgeMovement(Vector3 DesiredDirection, float MovePower)
    {
        float prevMoveSpeed = 0.0f;
        prevMoveSpeed = moveSpeed;
        moveSpeed = 0.0f;
        characterController.Move(DesiredDirection * MovePower);

        moveSpeed = prevMoveSpeed;
    }

    public void Move()
    {
        SetMoveDirection();

        Vector3 DesiredDirection = currentMoveDirection * moveSpeed * maxMoveSpeed;
        DesiredDirection.y = SetGravity();

        characterController.Move(DesiredDirection * Time.fixedDeltaTime);
    }
}
