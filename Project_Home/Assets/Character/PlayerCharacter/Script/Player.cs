using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    // Component
    Rigidbody2D rigidbody2;
    Animator animator;
    SpriteRenderer spriteRenderer;

    // Public Value
    [Header("Movement Value")]
    [Tooltip("캐릭터의 최대 속도")]
    [SerializeField] float MaxSpeed;
    [Tooltip("캐릭터의 최대 점프 강도")]
    [SerializeField] float MaxJumpPower;
    [Tooltip("캐릭터의 감속 정도")]
    [SerializeField] float DecreaseSpeed;

    // Private Value
    Vector2 MovementDirection;
    bool bIsJumping;
    bool bIsFalling;
    bool bIsAttack;
    

    // Functionary
    void Awake()
    {
        InitializedEssentialData();
        InitializedMovementData();
    }

    void InitializedEssentialData()
    {
        rigidbody2 = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        rigidbody2.freezeRotation = true;
    }

    void InitializedMovementData()
    {
        MaxSpeed = 5.0f;
        MaxJumpPower = 5.0f;
        DecreaseSpeed = 1.0f;
        MovementDirection = Vector2.zero;
        bIsJumping = false;
        bIsFalling = false;
        bIsAttack = false;
    }

    void Update()
    {
        CheckFalling();
        CheckMoving();

        /*if(Input.GetButtonUp("Horizontal"))
        {
            rigidbody2.linearVelocity = new Vector2(rigidbody2.linearVelocity.normalized.x * DecreaseSpeed, rigidbody2.linearVelocityY);
            animator.SetBool("bIsMove", false);
        }*/


    }

    void FixedUpdate()
    {
        // 이 함수는 유니티에서 사용하는 물리 기반 프레임마다 실행되는 듯.
        // 즉 RigidBody의 속성을 사용할 때에는 여기서 하면 되는 듯.

        // Movement Logic
        rigidbody2.linearVelocity = new Vector2(MovementDirection.normalized.x * MaxSpeed, rigidbody2.linearVelocityY);

        // Movment Acceleration Logic
        //rigidbody2.AddForce(MovementDirection.normalized, ForceMode2D.Impulse);
    }

    public void OnMove(InputValue inputValue)
    {
        MovementDirection = inputValue.Get<Vector2>();
        if(MovementDirection.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (MovementDirection.x > 0)
        {
            spriteRenderer.flipX = false;
        }
        
    }

    public void OnJump()
    {
        if(!bIsJumping && !bIsFalling)
        {
            rigidbody2.AddForce(Vector2.up * MaxJumpPower, ForceMode2D.Impulse);
        }
        else if (bIsJumping || bIsFalling)
        {
            // 천천히 떨어지는 기능 넣을 것.
            // 중력 값을 바꿔주거나 AddForce를 아래 방향으로 한 후 속도 조절하거나
        }
    }

    public void OnAttack()
    {
        if (!bIsAttack)
        {
            bIsAttack = true;
            animator.Play("Attack");
            //animator.SetBool("IsAttacking", bIsAttack);
        }
    }

    void CheckMoving()
    {
        if (MovementDirection != Vector2.zero)
        {
            animator.SetBool("IsMoving", true);
        }
        else
        {
            animator.SetBool("IsMoving", false);
        }
    }

    void CheckFalling()
    {
        if (rigidbody2.linearVelocity.y < 0)
        {
            bIsJumping = false;
            bIsFalling = true;
        }
        else if (rigidbody2.linearVelocityY == 0)
        {
            bIsJumping = false;
            bIsFalling = false;
        }
        else
        {
            bIsJumping = true;
            bIsFalling = false;
        }
        animator.SetBool("IsFalling", bIsFalling);
        animator.SetBool("IsJumping", bIsJumping);
    }

    void CheckFloor()
    {
        RaycastHit2D rayHit2D = Physics2D.Raycast(rigidbody2.position, Vector2.down, 0.25f, LayerMask.GetMask("Platform"));
        if (rayHit2D.collider)
        {
            animator.SetBool("bIsFalling", false);
            animator.SetBool("bIsJump", false);
        }
        else
        {
            animator.SetBool("bIsFalling", true);
        }
    }
}
