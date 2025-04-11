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

    // Movement State
    public bool bIsFalling { get; private set; }
    public bool bIsInAir { get; private set; }
    public bool bIsRolling { get; private set; }
    public bool bIsPowerJump { get; private set; }
    public bool bIsView { get; private set; }
    public bool bIsReadytoPowerJump { get; private set; }
    public bool bIsInteraction { get; private set; }
    public bool bIsSliding { get; private set; }
    public bool bIsInverseGravity { get; private set; }
    public bool bIsDuringGravity { get; private set; }
    public bool bIsStun { get; private set; }
    public bool bIsHit { get; private set; }
    public bool bIsDeath { get; private set; }
    public bool bIsBossStage { get; private set; }
    public bool bIsCanonControll { get; private set; }
    // ====================================
    //          - Private Data-
    // ====================================

    // Component
    Rigidbody2D CharacterBody;
    SpriteRenderer CharacterSprite;
    CapsuleCollider2D CharacterCapsule;
    Animator AnimationController;
    GameObject InteractionObj;
    PlayerInfo PlayerInfoData;
    RaycastHit2D InteractTrace;

    // Movement Data
    Vector2 MovementDirection;
    float CurrentSpeed;
    


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
        CancelCannonControll();
    }

    void FixedUpdate()
    {
        if (!bIsRolling && !bIsPowerJump && !bIsSliding && !bIsHit && !bIsDeath && !bIsStun)
        {
            ModifyMoveSpeedbyInteraction();
            if (bIsInverseGravity && CharacterBody.gravityScale < 0.0f)
            {
                CharacterBody.linearVelocity = new Vector2(MovementDirection.normalized.x * CurrentSpeed * -1.0f, CharacterBody.linearVelocity.y);
                return;
            }
            CharacterBody.linearVelocity = new Vector2(MovementDirection.normalized.x * CurrentSpeed, CharacterBody.linearVelocity.y);
        }
    }

    IEnumerator DoDash()
    {
        AnimationController.SetTrigger("Rolling");
        CharacterBody.linearVelocity = Vector2.zero;
        bIsRolling = true;
        float OriginGravityScale = CharacterBody.gravityScale;
        CharacterBody.gravityScale = 1.0f;
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("EnemyCharacter"), true);
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

        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("EnemyCharacter"), false);
        bIsRolling = false;
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

        bIsReadytoPowerJump = false;
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
        if (!HUD.Instance.bIsPause && !GameManager.Instance.bIsGameOver)
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
                }
                if (MovementDirection.x > 0)
                {
                    CharacterSprite.flipX = false;
                }
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
                bIsReadytoPowerJump = true;
            }
        }
        else
        {
            bIsInAir = true;
            bIsReadytoPowerJump = false;
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
            if (bIsView)
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
            if (InteractionObj && InteractionObj.GetComponent<PlatformControl>().bIsPullMax && MovementDirection == Vector2.left)
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

    public void OnMove(InputValue inputValue)
    {
        if (HUD.Instance.bIsPause)
        {
            MovementDirection = Vector2.zero;
            return;
        }
        if (!bIsRolling && !bIsPowerJump && !bIsSliding && !bIsDuringGravity && !bIsHit && !bIsDeath && !bIsStun && !bIsCanonControll)
        {
            MovementDirection = inputValue.Get<Vector2>();

            if (bIsInteraction)
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
    }

    public void OnJump()
    {
        if (bIsReadytoPowerJump)
        {
            StartCoroutine(DoPowerJump());
            return;
        }
        if (!bIsInAir && !bIsSliding && !bIsHit && !bIsDeath && !bIsStun && !bIsCanonControll && !bIsInteraction && !bIsPowerJump)
        {
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
    }

    public void OnDash()
    {
        if (!bIsInAir && !bIsSliding && !bIsHit && !bIsDeath && !bIsStun && !bIsCanonControll)
        {
            if (bIsInteraction)
            {
                bIsInteraction = false;
            }
            StartCoroutine(DoDash());
        }
    }

    public void OnView(InputValue inputValue)
    {
        if (!bIsDeath || !bIsCanonControll)
        {
            if (inputValue.isPressed)
            {
                bIsView = true;
            }
            if (!inputValue.isPressed)
            {
                bIsView = false;
            }
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
                }
                else
                {
                    bIsInteraction = true;
                    AnimationController.SetBool("Interaction", bIsInteraction);
                    InteractionObj = InteractTrace.collider.gameObject;
                    InteractionObj.GetComponent<PlatformControl>().bIsInteracting = true;
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

    public void OnEscape()
    {
        bool ShowingSettingMenu = HUD.Instance.PauseMenuInstance.bIsSettingMenu;
        bool ShowingControlMenu = HUD.Instance.PauseMenuInstance.bIsControllMenu;
        bool ShowingGraphicMenu = HUD.Instance.PauseMenuInstance.bIsGraphicMenu;
        bool ShowingSoundMenu = HUD.Instance.PauseMenuInstance.bIsSoundMenu;

        bool SelectScreenModeMenu = HUD.Instance.PauseMenuInstance.GraphicMenu.GetComponent<GraphicMenu>().bIsScreenMode;
        bool SelectResolutionSetMenu = HUD.Instance.PauseMenuInstance.GraphicMenu.GetComponent<GraphicMenu>().bIsResolutionSet;

        if (!HUD.Instance.GameOverUI.bIsGameOver)
        {
            if (ShowingSettingMenu && !ShowingControlMenu && !ShowingGraphicMenu && !ShowingSoundMenu)
            {
                HUD.Instance.PauseMenuInstance.HideSettingMenu();
            }
            else if (ShowingSettingMenu && ShowingControlMenu)
            {
                HUD.Instance.PauseMenuInstance.HideControlMenu();
            }
            else if (ShowingSettingMenu && ShowingGraphicMenu)
            {
                if (SelectScreenModeMenu || SelectResolutionSetMenu)
                {
                    HUD.Instance.PauseMenuInstance.GraphicMenu.GetComponent<GraphicMenu>().UnSelectedCancel();
                }
                else
                {
                    HUD.Instance.PauseMenuInstance.HideGraphicMenu();
                }
            }
            else if (ShowingSettingMenu && ShowingSoundMenu)
            {
                HUD.Instance.PauseMenuInstance.HideSoundMenu();
            }
            else
            {
                HUD.Instance.TogglePauseMenu();
            }
        }
    }

    void PlayWalkSound()
    {
        SoundManager.Instance.PlayWalkSound();
    }

    void PlayJumpSound()
    {
        SoundManager.Instance.PlayJumpSound();
    }

    void PlayLandingSound()
    {
        SoundManager.Instance.PlayLandingSound();
    }

    void CancelCannonControll()
    {
        if (bIsCanonControll && !BossCharacter.GetComponent<BossCharacter>().bIsCameraMoving)
        {
            bIsCanonControll = false;
            InteractionObj.GetComponent<CanonController>().OnUnPossesController();
            InteractionObj = null;
        }
    }
}
