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


    [Header("Player MovmentData")]
    public float MaxSpeed;
    public float MaxJumpPower;
    public float DashPower;
    public float AttackDamage;
    public float MaxHP;
    public float CurrentHP;

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
    bool bIsGround;
    bool bIsInteraction;
    bool bIsHit;
    bool bIsDeath;

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
        CheckWall();
    }

    private void FixedUpdate()
    {
        if (bIsRolling)
        {
            return;
        }
        CharacterBody.linearVelocity = new Vector2(MovementDirection.normalized.x * CurrentSpeed, CharacterBody.linearVelocityY);
    }

    public void OnMove(InputValue inputValue)
    {
        if (bIsRolling)
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
        CharacterBody.AddForce(Vector2.up * MaxJumpPower, ForceMode2D.Impulse);
        AnimationController.SetTrigger("Jumping");
    }

    public void OnDash()
    {
        if (bIsInAir)
        {
            return;
        }
        StartCoroutine(DoDash());
    }

    IEnumerator DoDash()
    {
        AnimationController.SetTrigger("Rolling");
        CharacterBody.linearVelocity = Vector2.zero;
        bIsRolling = true;
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
        CharacterBody.gravityScale = 1.0f;
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
        RaycastHit2D RayTrace = Physics2D.Raycast(CharacterCapsule.bounds.center, Vector2.down, CharacterCapsule.bounds.size.y / 2, LayerMask.GetMask("Ground"));
        if (RayTrace)
        {
            bIsInAir = false;
            if (bIsFalling)
            {
                bIsFalling = false;
                bIsGround = true;
                AnimationController.SetTrigger("Landing");
            }
        }
        else
        {
            bIsInAir = true;
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
}
