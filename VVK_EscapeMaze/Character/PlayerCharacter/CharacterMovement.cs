using Unity.Cinemachine;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [Header("Locomotion")]
    public float MaxMoveSpeed = 1.0f;

    // Component
    Camera mainCamera;
    CharacterController characterController;

    PlayerController playerController;
    CharacterAction characterAction;

    // Move State Value
    public float moveSpeed { get; private set; } = 0.0f;
    public float characterRotationRate { get; private set; } = 0.0f;
    public float GravityAcceleration { get; private set; } = 0.0f;
    
    public Vector3 currentMoveDirection { get; private set; } = Vector3.zero;
    public Vector3 lastInputDirection {  get; private set; } = Vector3.zero;

    public void InitEssentialData(Camera cam)
    {
        mainCamera = cam;
        playerController = GetComponent<PlayerController>();
        characterController = GetComponent<CharacterController>();
        characterAction = GetComponentInChildren<CharacterAction>();
        lastInputDirection = transform.forward;
    }

    void SetMoveDirection()
    {
        if (!playerController.bIsInventory)
        {
            Vector3 CameraForwardVector = mainCamera.transform.forward;
            CameraForwardVector.y = 0.0f;
            CameraForwardVector.Normalize();

            Vector3 CameraRightVector = mainCamera.transform.right;
            CameraRightVector.y = 0.0f;
            CameraRightVector.Normalize();

            Vector3 desiredMoveDirection = (CameraForwardVector * playerController.inputDirection.y + CameraRightVector * playerController.inputDirection.x).normalized;

            if (desiredMoveDirection.magnitude > 0.1f)
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
    }

    void SetRotateDirection(Vector3 moveDirection)
    {
        if (!playerController.bIsInventory)
        {
            Quaternion toRotation = Quaternion.identity;

            if (playerController.bIsEquip || playerController.bIsAim)
            {
                toRotation = Quaternion.LookRotation(mainCamera.transform.forward);
            }
            else if (moveDirection.magnitude > 0.1f)
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
        if(!characterAction.bIsSkillActivate)
        {
            float prevMoveSpeed = 0.0f;
            prevMoveSpeed = moveSpeed;
            moveSpeed = 0.0f;
            characterController.Move(DesiredDirection * MovePower);

            moveSpeed = prevMoveSpeed;
        }
    }

    public void Move(Vector2 moveDirection)
    {

        if (!characterAction.bIsSkillActivate && !playerController.bIsStep && !playerController.bIsDodge)
        {
            SetMoveDirection();

            Vector3 DesiredDirection = currentMoveDirection * moveSpeed * MaxMoveSpeed;
            DesiredDirection.y = SetGravity();

            characterController.Move(DesiredDirection * Time.fixedDeltaTime);
        }
        /*if (!characterAction.bIsSkillActivate && !playerController.bIsStep && !playerController.bIsDodge)
        {
            if (!playerController.bIsInventory)
            {
                Vector3 CameraForwardVector = mainCamera.transform.forward;
                CameraForwardVector.y = 0.0f;
                CameraForwardVector.Normalize();

                Vector3 CameraRightVector = mainCamera.transform.right;
                CameraRightVector.y = 0.0f;
                CameraRightVector.Normalize();

                movementDirection = new Vector3(moveDirection.x, 0.0f, mainCamera.transform.forward * moveDirection.y).normalized * moveSpeed * MaxMoveSpeed;
                if(movementDirection.magnitude > 0.1f)
                {
                    lastInputDirection = movementDirection;
                    currentMoveDirection = movementDirection;
                }
                else
                {
                    currentMoveDirection = Vector3.zero;
                }
            }
            movementDirection.y = SetGravity();
            characterController.Move(movementDirection * Time.fixedDeltaTime);
        }*/
    }

    /*public void Look(Vector2 mouseDelta)
    {
        if (CinemachineCamera && cinemachinPanTilt)
        {
            cinemachinPanTilt.PanAxis.Value += mouseDelta.x * CharacterRotationSpeedForLook;
            cinemachinPanTilt.TiltAxis.Value += mouseDelta.y * CharacterRotationSpeedForLook;

            cinemachinPanTilt.TiltAxis.Value = Mathf.Clamp(cinemachinPanTilt.TiltAxis.Value, -35.0f, 35.0f);

            characterLookDirection = new Vector3(cinemachinPanTilt.TiltAxis.Value, cinemachinPanTilt.PanAxis.Value, 0.0f);

            //mainCamera.transform.rotation = Quaternion.Euler(characterLookDirection.x, 0.0f, 0.0f);
            transform.rotation = Quaternion.Euler(0.0f, characterLookDirection.y, 0.0f);

            Ray ray = mainCamera.GetComponent<Camera>().ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0.0f));
            if (Physics.Raycast(ray, out RaycastHit hit, 200.0f, LayerMask.GetMask("Default")))
            {
                AimTarget.transform.position = hit.point;
            }
            else
            {
                AimTarget.transform.position = ray.GetPoint(100.0f);
            }
        }
    }*/
}