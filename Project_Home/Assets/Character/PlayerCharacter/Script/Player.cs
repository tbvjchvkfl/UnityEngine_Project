using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    // Component
    Rigidbody2D rigidbody2;
    Animator animator;
    SpriteRenderer spriteRenderer;
    CapsuleCollider2D capsuleCollider2;
    //CircleCollider2D circleCollider2;

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
    [SerializeField] float DashingTime;

    // Private Value
    Transform AttackPoint;
    public Vector2 MovementDirection;
    bool bIsJumping;
    bool bIsFalling;
    bool bIsDashing;
    bool bIsGround;
    bool bIsEdge;
    bool bIsInteraction;

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
        bIsGround = false;
        bIsEdge = false;
        bIsInteraction = false;
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
        if (bIsDashing || bIsEdge)
        {
            return;
        }
        rigidbody2.linearVelocity = new Vector2(MovementDirection.normalized.x * MaxSpeed, rigidbody2.linearVelocityY);
    }
    
    public void OnMove(InputValue inputValue)
    {
        if (bIsEdge)
        {
            return;
        }
        MovementDirection = inputValue.Get<Vector2>();
        if(MovementDirection.x < 0)
        {
            spriteRenderer.flipX = true;
            AttackPoint = ReverseAttackPos;
            capsuleCollider2.offset = new Vector2(0.07f, -0.04f);
        }
        else if (MovementDirection.x > 0)
        {
            spriteRenderer.flipX = false;
            AttackPoint = ForwardAttackPos;
            capsuleCollider2.offset = new Vector2(-0.08f, -0.04f);
        }
    }

    public void OnJump()
    {
        if(!bIsJumping && !bIsFalling && !bIsEdge)
        {
            bIsGround = false;
            bIsJumping = true;
            animator.SetBool("IsJumping", bIsJumping);
            rigidbody2.AddForce(Vector2.up * MaxJumpPower, ForceMode2D.Impulse);
        }
        else if (bIsJumping || bIsFalling)
        {
            DoEdging();
        }
        else if (bIsEdge)
        {
            rigidbody2.gravityScale = 1.0f;
            rigidbody2.AddForce(Vector2.up * 5.0f, ForceMode2D.Impulse);
            bIsEdge = false;
            animator.SetBool("IsEdging", bIsEdge);
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

    public void OnDash()
    {
        animator.SetTrigger("IsDashing");
        bIsDashing = true;
        StartCoroutine(DoDash());
    }

    public void OnInteraction()
    {
        RaycastHit2D HitRay = Physics2D.Raycast(transform.position, MovementDirection, 2.0f, LayerMask.GetMask("House"));

        if (bIsInteraction && HitRay.collider.tag == "Level1")
        {
            SceneManager.LoadScene("Level 1");
        }
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

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 7)
        {
            bIsInteraction = true;
            Debug.Log("EnableInteraction");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 7)
        {
            bIsInteraction = false;
            Debug.Log("DisableInteraction");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        bIsGround = collision.gameObject.layer == 6;
        if (collision.gameObject.layer == 6)
        {
            bIsGround = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            bIsGround = false;
        }
    }

    void CheckFalling()
    {
        if (rigidbody2.linearVelocityY < 0.0f && !bIsGround)
        {
            bIsFalling = true;
            bIsJumping = false;
        }
        else
        {
            bIsFalling = false;
        }
        animator.SetBool("IsFalling", bIsFalling);
        animator.SetBool("IsJumping", bIsJumping);
    }

    void DoEdging()
    {
        if(MovementDirection.x < 0)
        {
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2.position, Vector2.left, 0.25f, LayerMask.GetMask("Platform"));
            Debug.DrawRay(transform.position, Vector3.right, Color.red);
            if (hit.collider)
            {
                rigidbody2.gravityScale = 0.0f;
                rigidbody2.linearVelocityY = 0.0f;
                bIsEdge = true;
                bIsFalling = false;
                bIsJumping = false;
                animator.SetBool("IsEdging", bIsEdge);
                animator.SetBool("IsJumping", bIsJumping);
                animator.SetBool("IsFalling", bIsFalling);
            }
        }
        if(MovementDirection.x > 0)
        {
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2.position, Vector2.right, 0.25f, LayerMask.GetMask("Platform"));
            Debug.DrawRay(transform.position, Vector3.right, Color.green);
            if (hit.collider)
            {
                rigidbody2.gravityScale = 0.0f;
                rigidbody2.linearVelocityY = 0.0f;
                bIsEdge = true;
                bIsFalling = false;
                bIsJumping = false;
                animator.SetBool("IsEdging", bIsEdge);
                animator.SetBool("IsJumping", bIsJumping);
                animator.SetBool("IsFalling", bIsFalling);
            }
        }
    }
}
