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

    private void Awake()
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
        //SetGravityScale();
        CheckWall();
        CameraInteraction();
    }
    
    private void FixedUpdate()
    {
        if (bIsRolling || bIsPowerJump)
        {
            return;
        }
        CharacterBody.linearVelocity = new Vector2(MovementDirection.normalized.x * CurrentSpeed, CharacterBody.linearVelocity.y);
    }

    public void OnMove(InputValue inputValue)
    {
        if (bIsRolling || bIsPowerJump)
        {
            return;
        }

        MovementDirection = inputValue.Get<Vector2>();
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

    public void OnJump()
    {
        if (bIsInAir)
        {
            return;
        }
        if (IsReadytoPowerJump)
        {
            StartCoroutine(DoPowerJump());
            return;
        }
        //CharacterBody.gravityScale = 1.0f;
        float JumpValue = MaxJumpPower - CharacterBody.linearVelocity.y;
        CharacterBody.AddForce(Vector2.up * JumpValue, ForceMode2D.Impulse);
        AnimationController.SetTrigger("Jumping");
    }

    IEnumerator DoPowerJump()
    {
        bIsPowerJump = true;
        AnimationController.SetBool("ReadyPowerJump", true);

        yield return new WaitForSeconds(0.5f);

        CharacterBody.AddForce(Vector2.up * PowerJumpValue, ForceMode2D.Impulse);
        AnimationController.SetBool("ReadyPowerJump", false);
        AnimationController.SetTrigger("PowerJump");

        yield return new WaitForSeconds(1.5f);

        bIsPowerJump = false;
    }

    public void OnDash()
    {
        if (bIsInAir)
        {
            return;
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
        if (CheckGround || CheckPlatform)
        {
            bIsInAir = false;
            if (bIsFalling)
            {
                bIsFalling = false;
                AnimationController.SetTrigger("Landing");
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

    void SetGravityScale()
    {
        if (bIsInAir)
        {
            CharacterBody.gravityScale = 1.0f;
        }
        else
        {
            CharacterBody.gravityScale = 3.0f;
        }
    }
}
