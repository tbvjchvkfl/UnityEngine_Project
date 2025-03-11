using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerInput : MonoBehaviour
{
    // ====================================
    //          - Public Data-
    // ====================================

    [Header("Component")]
    public BoxCollider2D AttackForward;
    public BoxCollider2D AttackBackward;
    public Camera ViewCamera;
    public BoxCollider2D CheckingFloor;

    [Header("Player MovmentData")]
    public float MaxSpeed;
    public float InteractMoveSpeed;
    public float MaxJumpPower;
    public float PowerJumpValue;
    public float DashPower;


    [Header("Component Control Value")]
    public float MaxCameraSize;
    public float MinCameraSize;
    public float CameraZoomSpeed;
    public float CameraInterpSpeed;


    [Header("Animation Data")]
    public AnimationClip RollingAnimation;
    public AnimationClip DeathAnimation;
    public AnimationClip HitAnimation;
    

    // ====================================
    //          - Private Data-
    // ====================================

    // Component
    Rigidbody2D CharacterBody;
    SpriteRenderer CharacterSprite;
    CapsuleCollider2D CharacterCapsule;
    Animator AnimationController;
    BoxCollider2D AttackPoint;
    GameObject InteractionObj;

    // Movement Data
    Vector2 MovementDirection;
    float CurrentSpeed;

    // Movement State
    bool bIsFalling;
    bool bIsInAir;
    bool bIsRolling;
    bool bIsPowerJump;
    bool IsView;
    bool IsReadytoPowerJump;
    bool bIsInteraction;
    bool bIsSliding;
    bool bIsInteratingMoving;

    public CapsuleCollider2D GetCharacterCapsule()
    {
        return CharacterCapsule;
    }

    public bool SetInteractingMoving(bool Value)
    {
        return bIsInteratingMoving = Value;
    }

    public void OnMove(InputValue inputValue)
    {
        if (bIsRolling || bIsPowerJump || bIsSliding)
        {
            return;
        }

        MovementDirection = inputValue.Get<Vector2>();
        if (!bIsInteraction)
        {
            if (MovementDirection.x < 0)
            {
                CharacterSprite.flipX = true;
                AttackPoint = AttackBackward;
            }
            else if (MovementDirection.x > 0)
            {
                CharacterSprite.flipX = false;
                AttackPoint = AttackForward;
            }
        }
        else
        {
            if (!CharacterSprite.flipX)
            {
                if (MovementDirection.x < 0)
                {
                    AnimationController.SetBool("Pulling", true);
                    AnimationController.SetBool("Pushing", false);
                }
                else if (MovementDirection.x > 0)
                {
                    AnimationController.SetBool("Pulling", false);
                    AnimationController.SetBool("Pushing", true);
                }
                else
                {
                    AnimationController.SetBool("Pulling", false);
                    AnimationController.SetBool("Pushing", false);
                }
            }
            else
            {
                if (MovementDirection.x < 0)
                {
                    AnimationController.SetBool("Pulling", false);
                    AnimationController.SetBool("Pushing", true);
                }
                else if (MovementDirection.x > 0)
                {
                    AnimationController.SetBool("Pulling", true);
                    AnimationController.SetBool("Pushing", false);
                }
                else
                {
                    AnimationController.SetBool("Pulling", false);
                    AnimationController.SetBool("Pushing", false);
                }
            }
        }
    }

    public void OnJump()
    {
        if (bIsInAir || bIsSliding)
        {
            return;
        }
        if (IsReadytoPowerJump)
        {
            StartCoroutine(DoPowerJump());
            return;
        }
        if (bIsInteraction)
        {
            bIsInteraction = false;
        }
        float JumpValue = MaxJumpPower - CharacterBody.linearVelocity.y;
        CharacterBody.AddForce(Vector2.up * JumpValue, ForceMode2D.Impulse);
        AnimationController.SetTrigger("Jumping");
    }

    public void OnDash()
    {
        if (bIsInAir || bIsSliding)
        {
            return;
        }
        if (bIsInteraction)
        {
            bIsInteraction = false;
        }
        StartCoroutine(DoDash());
    }

    public void OnView(InputValue inputValue)
    {
        if (inputValue.isPressed)
        {
            IsView = true;
        }
        if (!inputValue.isPressed)
        {
            IsView = false;
        }
    }

    public void OnInteraction()
    {
        if (bIsInteraction)
        {
            bIsInteraction = false;
            AnimationController.SetBool("Interaction", bIsInteraction);
            InteractionObj.GetComponent<PlatformControl>().bIsInteracting = false;
            InteractionObj = null;
            SetInteractingMoving(false);
        }
        else
        {
            RaycastHit2D R_Trace = Physics2D.Raycast(CharacterCapsule.bounds.center, Vector2.right, CharacterCapsule.bounds.size.x / 2, LayerMask.GetMask("Interactable"));
            RaycastHit2D L_Trace = Physics2D.Raycast(CharacterCapsule.bounds.center, Vector2.left, CharacterCapsule.bounds.size.x / 2, LayerMask.GetMask("Interactable"));
            if (R_Trace)
            {
                bIsInteraction = true;
                AnimationController.SetBool("Interaction", bIsInteraction);
                InteractionObj = R_Trace.collider.gameObject;
                InteractionObj.GetComponent<PlatformControl>().bIsInteracting = true;
                SetInteractingMoving(true);
            }
            if (L_Trace)
            {
                bIsInteraction = true;
                AnimationController.SetBool("Interaction", bIsInteraction);
                InteractionObj = L_Trace.collider.gameObject;
                InteractionObj.GetComponent<PlatformControl>().bIsInteracting = true;
                SetInteractingMoving(true);
            }
        }
    }

    void Awake()
    {
        CharacterBody = GetComponent<Rigidbody2D>();
        CharacterSprite = GetComponent<SpriteRenderer>();
        CharacterCapsule = GetComponent<CapsuleCollider2D>();
        AnimationController = GetComponent<Animator>();

        CurrentSpeed = MaxSpeed;
    }

    void Update()
    {
        CheckGroundMoving();
        CheckInAir();
        CheckWall();
        CheckSliding();
        CameraInteraction();
    }

    void FixedUpdate()
    {
        if (bIsRolling || bIsPowerJump || bIsSliding)
        {
            return;
        }
        ModifyMoveSpeedbyInteraction();
        CharacterBody.linearVelocity = new Vector2(MovementDirection.normalized.x * CurrentSpeed, CharacterBody.linearVelocity.y);
    }

    IEnumerator DoDash()
    {
        AnimationController.SetTrigger("Rolling");
        CharacterBody.linearVelocity = Vector2.zero;
        bIsRolling = true;
        float OriginGravityScale = CharacterBody.gravityScale;
        CharacterBody.gravityScale = 0.0f;

        if (CharacterSprite.flipX)
        {
            CharacterBody.AddForce(Vector2.left * DashPower, ForceMode2D.Impulse);
        }
        else
        {
            CharacterBody.AddForce(Vector2.right * DashPower, ForceMode2D.Impulse);
        }

        yield return new WaitForSeconds(RollingAnimation.length - 0.09f);

        bIsRolling = false;
        CharacterBody.gravityScale = OriginGravityScale;
    }

    IEnumerator DoPowerJump()
    {
        CharacterBody.linearVelocity = Vector2.zero;
        bIsPowerJump = true;
        AnimationController.SetBool("ReadyPowerJump", true);

        yield return new WaitForSeconds(0.5f);

        CharacterBody.AddForce(Vector2.up * PowerJumpValue, ForceMode2D.Impulse);
        AnimationController.SetBool("ReadyPowerJump", false);
        AnimationController.SetTrigger("PowerJump");

        yield return new WaitForSeconds(1.5f);

        bIsPowerJump = false;
    }

    void CheckGroundMoving()
    {
        if (MovementDirection.x != 0.0f)
        {
            AnimationController.SetBool("Moving", true);
        }
        else
        {
            AnimationController.SetBool("Moving", false);
        }
    }

    void CheckInAir()
    {
        Collider2D CheckGround = Physics2D.OverlapBox(CheckingFloor.bounds.center, CheckingFloor.bounds.size, 0.0f, LayerMask.GetMask("Ground"));
        Collider2D CheckPlatform = Physics2D.OverlapBox(CheckingFloor.bounds.center, CheckingFloor.bounds.size, 0.0f, LayerMask.GetMask("Jump Platform"));
        if (CheckGround || CheckPlatform || bIsSliding)
        {
            bIsInAir = false;
            if (bIsFalling)
            {
                bIsFalling = false;
            }
            if (CheckPlatform)
            {
                IsReadytoPowerJump = true;
            }
        }
        else
        {
            bIsInAir = true;
            IsReadytoPowerJump = false;
            if (CharacterBody.linearVelocityY < 0.0f)
            {
                bIsFalling = true;
            }
        }
        AnimationController.SetBool("Landing", !bIsInAir);
        AnimationController.SetBool("Falling", bIsFalling);
    }

    void CheckWall()
    {
        RaycastHit2D RayTraceR = Physics2D.Raycast(CharacterCapsule.bounds.center, Vector2.right, CharacterCapsule.bounds.size.x / 2, LayerMask.GetMask("GroundObject"));
        RaycastHit2D RayTraceL = Physics2D.Raycast(CharacterCapsule.bounds.center, Vector2.left, CharacterCapsule.bounds.size.x / 2, LayerMask.GetMask("GroundObject"));
        if (RayTraceR || RayTraceL)
        {
            if (CharacterBody.linearVelocityX != 0.0f && bIsInAir)
            {
                Debug.Log("Push & Pull");
            }
        }
    }

    void CameraInteraction()
    {
        if (IsView)
        {
            if (MovementDirection.y > 0)
            {
                ViewCamera.orthographicSize += Time.deltaTime * CameraZoomSpeed;
                ViewCamera.orthographicSize = Mathf.Clamp(ViewCamera.orthographicSize, MinCameraSize, MaxCameraSize);
            }
            if (MovementDirection.y < 0)
            {
                ViewCamera.orthographicSize -= Time.deltaTime * CameraZoomSpeed;
                ViewCamera.orthographicSize = Mathf.Clamp(ViewCamera.orthographicSize, MinCameraSize, MaxCameraSize);
            }
            if (MovementDirection.y == 0)
            {
                ViewCamera.orthographicSize = Mathf.Lerp(ViewCamera.orthographicSize, 5.0f, Time.deltaTime * CameraInterpSpeed);
            }
        }
        else
        {
            ViewCamera.orthographicSize = Mathf.Lerp(ViewCamera.orthographicSize, 5.0f, Time.deltaTime * CameraInterpSpeed);
        }
    }

    void CheckSliding()
    {
        Collider2D CheckPlatform = Physics2D.OverlapBox(CheckingFloor.bounds.center, CheckingFloor.bounds.size, 0.0f, LayerMask.GetMask("RunWay"));
        if (CheckPlatform)
        {
            bIsSliding = true;
            MovementDirection = Vector2.zero;
            float TargetRotValue = CheckPlatform.gameObject.GetComponent<Transform>().eulerAngles.z;
            transform.rotation = Quaternion.Euler(0.0f, 0.0f, TargetRotValue);
            if (TargetRotValue <= 180.0f)
            {
                CharacterSprite.flipX = true;
            }
            else
            {
                CharacterSprite.flipX = false;
            }
        }
        else
        {
            bIsSliding = false;
            transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        }
        AnimationController.SetBool("Sliding", bIsSliding);
    }

    void ModifyMoveSpeedbyInteraction()
    {
        if (bIsInteraction)
        {
            if (InteractionObj && InteractionObj.GetComponent<PlatformControl>().GetPullMax() && MovementDirection == Vector2.left)
            {
                CurrentSpeed = 0.0f;
            }
            else
            {
                CurrentSpeed = InteractMoveSpeed;
            }
        }
        else
        {
            CurrentSpeed = MaxSpeed;
        }
    }
}
