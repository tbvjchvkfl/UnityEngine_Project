using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class Player : MonoBehaviour
{
    // Component
    public Rigidbody2D rigidbody2;
    Animator animator;
    SpriteRenderer spriteRenderer;
    CapsuleCollider2D capsuleCollider2;

    // Modify In Editor Value
    public BoxCollider2D ForwardAttackPos;
    public BoxCollider2D BackwardAttackPos;

    [Header("Movement Value")]
    [Tooltip("캐릭터의 최대 속도")]
    [SerializeField] float MaxSpeed;
    [Tooltip("캐릭터의 최대 점프 강도")]
    [SerializeField] float MaxJumpPower;

    public float DashDistance;
    public float DashingTime;
    public float AttackDamage;
    public float maxHP;

    [Header("Anim Clip")]
    public AnimationClip deathclip;
    public AnimationClip hurtclip;

    // Private Value
    BoxCollider2D AttackPoint;
    Vector2 MovementDirection;
    bool bIsJumping;
    bool bIsFalling;
    bool bIsDashing;
    bool bIsGround;
    bool bIsEdge;
    bool bIsInteraction;
    bool EnableLaddering;
    bool bIsHurt;
    bool bIsDeath;

    [HideInInspector] public float curHP;


    // Functionary
    void Awake()
    {
        InitializedEssentialData();
        InitializedMovementData();
    }

    void Update()
    {
        if (bIsDashing || bIsHurt || bIsDeath)
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
        if (bIsDashing || bIsEdge || bIsHurt || bIsDeath)
        {
            if (bIsHurt)
            {
                if (rigidbody2.linearVelocity == Vector2.zero)
                {
                    bIsHurt = false;
                    animator.SetBool("IsHurting", bIsHurt);
                }
            }
            return;
        }
        if(EnableLaddering)
        {
            rigidbody2.gravityScale = 0.0f;
            capsuleCollider2.isTrigger = true;
            rigidbody2.linearVelocity = new Vector2(rigidbody2.linearVelocityX, MovementDirection.normalized.y * MaxSpeed);
            
            return;
        }
        rigidbody2.linearVelocity = new Vector2(MovementDirection.normalized.x * MaxSpeed, rigidbody2.linearVelocityY);
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
        EnableLaddering = false;
        maxHP = 100.0f;
        curHP = maxHP;
    }

    public void OnMove(InputValue inputValue)
    {
        if (bIsEdge || bIsHurt || bIsDeath)
        {
            return;
        }
        MovementDirection = inputValue.Get<Vector2>();
        if(MovementDirection.x < 0)
        {
            spriteRenderer.flipX = true;
            AttackPoint = BackwardAttackPos;
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
        if (bIsHurt)
        {
            return;
        }
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
        if(bIsHurt)
        {
            return;
        }
        animator.SetTrigger("IsAttacking");
        Collider2D hitManager = Physics2D.OverlapBox(AttackPoint.bounds.center, AttackPoint.bounds.size, 0.0f, LayerMask.GetMask("Enemy"));
        if(hitManager)
        {
            Boss boss = hitManager.gameObject.GetComponent<Boss>();
            if (boss)
            {
                boss.TakeDamage(AttackDamage);
            }
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
        if (bIsInteraction)
        {
            if (HitRay.collider.tag == "Level1")
            {
                SceneManager.LoadScene("Level 1");
            }
            if (HitRay.collider.tag == "Ladder")
            {
                EnableLaddering = true;
                animator.SetBool("IsLadding", EnableLaddering);
            }
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
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 7)
        {
            bIsInteraction = false;
            EnableLaddering = false;
            animator.SetBool("IsLadding", EnableLaddering);
            rigidbody2.gravityScale = 1.0f;
            capsuleCollider2.isTrigger = false;
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

    public void TakeDamage(float damage, float knockbackdistance)
    {
        curHP -= damage;
        if (curHP <= 0.0f)
        {
            curHP = 0.0f;
            StartCoroutine(Death());
        }
        else
        {
            //StartCoroutine(Hurt(knockbackdistance));
            bIsHurt = true;
            //animator.SetTrigger("IsHurting");
            animator.SetBool("IsHurting", bIsHurt);
            if (spriteRenderer.flipX)
            {
                rigidbody2.AddForce(new Vector2(1, 1) * knockbackdistance, ForceMode2D.Impulse);
            }
            else
            {
                rigidbody2.AddForce(new Vector2(-1, 1) * knockbackdistance, ForceMode2D.Impulse);
            }
        }
    }

    IEnumerator Hurt(float distance)
    {
        bIsHurt = true;
        animator.SetTrigger("IsHurting");
        if (spriteRenderer.flipX)
        {
            rigidbody2.AddForce(new Vector2(1, 1) * distance, ForceMode2D.Impulse);
        }
        else
        {
            rigidbody2.AddForce(new Vector2(-1, 1) * distance, ForceMode2D.Impulse);
        }
        yield return hurtclip.length;
        if(rigidbody2.linearVelocity == Vector2.zero)
        {
            bIsHurt = false;
            StopCoroutine(Hurt(distance));
        }
    }

    IEnumerator Death()
    {
        animator.SetTrigger("IsDeath");
        yield return deathclip.length;
        this.gameObject.SetActive(false);
        StopCoroutine(Death());
    }
}
