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
    public GameObject BossCharacter;

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
    GameObject CanonObj;
    PlayerInfo PlayerInfoData;
    RaycastHit2D InteractTrace;

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
    bool bIsInverseGravity;
    bool bIsDuringGravity;
    bool bIsElevatorMove;
    bool bIsStun;
    bool bIsHit;
    bool bIsDeath;
    bool bIsBossStage;
    bool bIsCanonControll;

    public CapsuleCollider2D GetCharacterCapsule()
    {
        return CharacterCapsule;
    }

    public Rigidbody2D GetPlayerRigid()
    {
        return CharacterBody;
    }

    public bool SetInteractingMoving(bool Value)
    {
        return bIsInteratingMoving = Value;
    }

    public void OnMove(InputValue inputValue)
    {
        if (bIsRolling || bIsPowerJump || bIsSliding || bIsDuringGravity || bIsHit || bIsDeath || bIsStun || bIsCanonControll)
        {
            return;
        }

        MovementDirection = inputValue.Get<Vector2>();

        if(bIsInteraction)
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
        }
    }

    public void OnJump()
    {
        if (bIsInAir || bIsSliding || bIsHit || bIsDeath || bIsStun || bIsCanonControll || bIsInteraction || bIsPowerJump)
        {
            return;
        }
        if (IsReadytoPowerJump)
        {
            StartCoroutine(DoPowerJump());
            return;
        }
        
        if (bIsInverseGravity)
        {
            float JumpValue = MaxJumpPower + CharacterBody.linearVelocity.y;
            CharacterBody.AddForce(Vector2.down * JumpValue, ForceMode2D.Impulse);
        }
        else
        {
            float JumpValue = MaxJumpPower - CharacterBody.linearVelocity.y;
            CharacterBody.AddForce(Vector2.up * JumpValue, ForceMode2D.Impulse);
        }
        AnimationController.SetTrigger("Jumping");
    }

    public void OnDash()
    {
        if (bIsInAir || bIsSliding || bIsHit || bIsDeath || bIsStun || bIsCanonControll)
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
        if (bIsDeath || bIsCanonControll)
        {
            return;
        }
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
        StartTrace();
        if (InteractTrace)
        {
            if (InteractTrace.collider.gameObject.tag == "PullingObj")
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
                    bIsInteraction = true;
                    AnimationController.SetBool("Interaction", bIsInteraction);
                    InteractionObj = InteractTrace.collider.gameObject;
                    InteractionObj.GetComponent<PlatformControl>().bIsInteracting = true;
                    SetInteractingMoving(true);
                }
            }
            if (InteractTrace.collider.gameObject.tag == "GravityObj")
            {
                if (bIsInverseGravity)
                {
                    AnimationController.SetTrigger("GravityInteraction");
                    InteractionObj.GetComponent<GravityControl>().ReturnOriginGravity();
                    bIsInverseGravity = InteractionObj.GetComponent<GravityControl>().bIsInverseGravity;
                    InteractionObj = null;
                }
                else
                {
                    InteractionObj = InteractTrace.collider.gameObject;
                    InteractionObj.GetComponent<GravityControl>().StartReverseGravity();
                    bIsInverseGravity = InteractionObj.GetComponent<GravityControl>().bIsInverseGravity;
                    AnimationController.SetTrigger("GravityInteraction");
                    StartCoroutine(DuringGravity());
                }
            }
            if (InteractTrace.collider.gameObject.tag == "ElevatorObj")
            {
                if (InteractTrace.collider.gameObject.GetComponentInParent<MovingElevator>().bIsInteraction)
                {
                    InteractTrace.collider.gameObject.GetComponentInParent<MovingElevator>().bIsInteraction = false;
                }
                else
                {
                    InteractTrace.collider.gameObject.GetComponentInParent<MovingElevator>().bIsInteraction = true;
                }
                AnimationController.SetTrigger("GravityInteraction");
            }
            if (InteractTrace.collider.gameObject.tag == "CannonObj")
            {
                if (bIsCanonControll)
                {
                    InteractionObj.GetComponent<CanonController>().OnUnPossesController();
                    bIsCanonControll = InteractionObj.GetComponent<CanonController>().bIsControlled;
                    InteractionObj = null;
                }
                else
                {
                    InteractionObj = InteractTrace.collider.gameObject;
                    InteractionObj.GetComponent<CanonController>().OnPossesController();
                    bIsCanonControll = InteractionObj.GetComponent<CanonController>().bIsControlled;
                }
            }
            if (InteractTrace.collider.gameObject.tag == "Portal")
            {
                InteractTrace.collider.gameObject.GetComponent<Portal>().MovetoNextStage();
            }
        }
    }

    void Awake()
    {
        CharacterBody = GetComponent<Rigidbody2D>();
        CharacterSprite = GetComponent<SpriteRenderer>();
        CharacterCapsule = GetComponent<CapsuleCollider2D>();
        AnimationController = GetComponent<Animator>();
        PlayerInfoData = GetComponent<PlayerInfo>();

        CurrentSpeed = MaxSpeed;
    }

    void Update()
    {
        ApplyPlayerInfoValue();
        CheckGroundMoving();
        CheckInAir();
        CheckWall();
        CheckSliding();
        CameraInteraction();
        CheckBossStage();
        SetMovingDirection();
        if (bIsCanonControll && !BossCharacter.GetComponent<BossCharacter>().bIsCameraMoving)
        {
            bIsCanonControll = false;
            InteractionObj.GetComponent<CanonController>().OnUnPossesController();
            InteractionObj = null;
        }
    }

    void FixedUpdate()
    {
        if (bIsRolling || bIsPowerJump || bIsSliding || bIsHit || bIsDeath || bIsStun)
        {
            return;
        }
        ModifyMoveSpeedbyInteraction();
        if (bIsInverseGravity && CharacterBody.gravityScale < 0.0f)
        {
            CharacterBody.linearVelocity = new Vector2(MovementDirection.normalized.x * CurrentSpeed * -1.0f, CharacterBody.linearVelocity.y);
            return;
        }
        CharacterBody.linearVelocity = new Vector2(MovementDirection.normalized.x * CurrentSpeed, CharacterBody.linearVelocity.y);
    }

    IEnumerator DoDash()
    {
        AnimationController.SetTrigger("Rolling");
        CharacterBody.linearVelocity = Vector2.zero;
        bIsRolling = true;
        float OriginGravityScale = CharacterBody.gravityScale;
        CharacterBody.gravityScale = 0.0f;
        CharacterCapsule.enabled = false;
        MovementDirection = Vector2.zero;
        AnimationController.SetBool("Moving", false);
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
        CharacterCapsule.enabled = true;
        CharacterBody.gravityScale = OriginGravityScale;
    }

    IEnumerator DoPowerJump()
    {
        CharacterBody.linearVelocity = Vector2.zero;
        AnimationController.SetBool("Moving", false);
        MovementDirection = Vector2.zero;
        bIsPowerJump = true;
        AnimationController.SetBool("ReadyPowerJump", true);
        yield return new WaitForSeconds(0.5f);

        IsReadytoPowerJump = false;
        CharacterBody.AddForce(Vector2.up * PowerJumpValue, ForceMode2D.Impulse);
        AnimationController.SetBool("ReadyPowerJump", false);
        AnimationController.SetBool("PowerJump", true);
        yield return new WaitForSeconds(1.5f);

        AnimationController.SetBool("PowerJump", false);
        bIsPowerJump = false;
    }

    IEnumerator DuringGravity()
    {
        bIsDuringGravity = true;
        MovementDirection = Vector2.zero;
        yield return new WaitForSeconds(10.0f);
        bIsDuringGravity = false;
    }

    void StartTrace()
    {
        RaycastHit2D R_Trace = Physics2D.Raycast(CharacterCapsule.bounds.center, Vector2.right, CharacterCapsule.bounds.size.x / 2, LayerMask.GetMask("Interactable"));
        RaycastHit2D L_Trace = Physics2D.Raycast(CharacterCapsule.bounds.center, Vector2.left, CharacterCapsule.bounds.size.x / 2, LayerMask.GetMask("Interactable"));
        if (!CharacterSprite.flipX)
        {
            InteractTrace = R_Trace;
        }
        else
        {
            InteractTrace = L_Trace;
        }
    }

    void SetMovingDirection()
    {
        if (bIsHit)
        {
            MovementDirection = Vector3.zero;
            AnimationController.SetBool("Moving", false);
        }
        if (bIsInteraction)
        {
            CharacterSprite.flipX = false;
        }
        else
        {
            if (MovementDirection.x < 0)
            {
                CharacterSprite.flipX = true;
                AttackPoint = AttackBackward;
            }
            if (MovementDirection.x > 0)
            {
                CharacterSprite.flipX = false;
                AttackPoint = AttackForward;
            }
        }
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
            if (CharacterBody.linearVelocityY < 0.0f || CharacterBody.gravityScale < 3.0f)
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
        if (!bIsBossStage)
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
            if (bIsInverseGravity || CharacterBody.gravityScale != 3.0f)
            {
                return;
            }
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

    void ApplyPlayerInfoValue()
    {
        bIsStun = PlayerInfoData.bIsStun;
        bIsHit = PlayerInfoData.bIsHit;
        bIsDeath = PlayerInfoData.bIsDeath;
    }

    void CheckBossStage()
    {
        if (BossCharacter)
        {
            bIsBossStage = true;
        }
        else
        {
            bIsBossStage = false;
        }
    }
}
