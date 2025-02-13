using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class Player : MonoBehaviour
{
    // Component
    Rigidbody2D rigidbody2;
    Animator animator;
    SpriteRenderer spriteRenderer;
    CapsuleCollider2D capsuleCollider2;

    // Modify In Editor Value
    [Header("Movement Value")]
    [Tooltip("캐릭터의 최대 속도")]
    [SerializeField] float MaxSpeed;
    [Tooltip("캐릭터의 최대 점프 강도")]
    [SerializeField] float MaxJumpPower;
    [SerializeField] Transform ReverseAttackPos;
    [SerializeField] Transform ForwardAttackPos;
    [SerializeField] Vector2 BoxSize;
    [SerializeField] float AttackRange;
    [SerializeField] float DashDistance;

    // Private Value
    Transform AttackPoint;
    public Vector2 MovementDirection;
    bool bIsJumping;
    bool bIsFalling;
    bool bIsDashing;

    [SerializeField] float DashingTime;
    

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
        capsuleCollider2 = GetComponent<CapsuleCollider2D>();

        rigidbody2.freezeRotation = true;
        AttackPoint = ForwardAttackPos;
    }

    void InitializedMovementData()
    {
        MaxSpeed = 5.0f;
        MaxJumpPower = 5.0f;
        MovementDirection = Vector2.zero;
        bIsJumping = false;
        bIsFalling = false;
        bIsDashing = false;
    }

    void Update()
    {
        if (bIsDashing)
        {
            return;
        }

        CheckFalling();
        CheckMoving();
    }

    void FixedUpdate()
    {
        // 이 함수는 유니티에서 사용하는 물리 기반 프레임마다 실행되는 듯.
        // 즉 RigidBody의 속성을 사용할 때에는 여기서 하면 되는 듯.
        if (bIsDashing)
        {
            return;
        }

        rigidbody2.linearVelocity = new Vector2(MovementDirection.normalized.x * MaxSpeed, rigidbody2.linearVelocityY);
    }
    
    public void OnMove(InputValue inputValue)
    {
        MovementDirection = inputValue.Get<Vector2>();
        if(MovementDirection.x < 0)
        {
            spriteRenderer.flipX = true;
            AttackPoint = ReverseAttackPos;
            capsuleCollider2.offset = new Vector2(0.07f, -0.04f);
            /*Vector3 localScale = transform.localScale;
            localScale.x *= -1.0f;
            transform.localScale = localScale;*/
        }
        else if (MovementDirection.x > 0)
        {
            spriteRenderer.flipX = false;
            AttackPoint = ForwardAttackPos;
            capsuleCollider2.offset = new Vector2(-0.08f, -0.04f);
            /*Vector3 localScale = transform.localScale;
            localScale.x *= 1.0f;
            transform.localScale = localScale;*/
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
        animator.SetTrigger("IsAttacking");
        Collider2D[] HitArray = Physics2D.OverlapBoxAll(AttackPoint.position, BoxSize, AttackRange);

        foreach (Collider2D HitResult in HitArray)
        {
            Debug.Log(HitResult.tag);
        }
    }

    /*private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(AttackPoint.position, BoxSize);
    }*/

    public void OnDash()
    {
        animator.SetTrigger("IsDashing");
        bIsDashing = true;
        StartCoroutine(DoDash());
    }

    private IEnumerator DoDash()
    {
        bIsDashing = true;
        var originalGravity = rigidbody2.gravityScale;
        rigidbody2.gravityScale = 0f;

        if (MovementDirection.x < 0)
        {
            rigidbody2.linearVelocity = new Vector2(transform.localScale.x * -DashDistance, 0);
        }
        else if (MovementDirection.x > 0)
        {
            rigidbody2.linearVelocity = new Vector2(transform.localScale.x * DashDistance, 0);
        }
        
        yield return new WaitForSeconds(DashingTime);

        rigidbody2.gravityScale = originalGravity;
        bIsDashing = false;
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
