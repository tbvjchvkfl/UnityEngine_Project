using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Unity.VisualScripting;

public interface IBossState
{
    void EnterState(Boss boss);
    void ExcuteState(Boss boss);
    void ExitState(Boss boss);
}

public class IdleState : IBossState
{
    float CastCooldown;
    float AttackDelay;
    public void EnterState(Boss boss)
    {
        CastCooldown = boss.CastCooldown;
        AttackDelay = boss.AttackDelay;
    }

    public void ExcuteState(Boss boss)
    {
        Collider2D HitEnemy = Physics2D.OverlapBox(boss.boxCollider2.bounds.center, boss.boxCollider2.bounds.size, 0.0f, LayerMask.GetMask("Player"));
        if (HitEnemy)
        {
            float TargetDistance = Vector3.Distance(boss.TargetRigidbody2.transform.position, boss.rigidbody2.transform.position);

            if (TargetDistance < 1.5f)
            {
                AttackDelay -= Time.deltaTime;
                if (AttackDelay <= 0.0f)
                {
                    boss.ModifyingState(new AttackState());
                }
            }
            else
            {
                CastCooldown -= Time.deltaTime;
                if (CastCooldown <= 0.0f)
                {
                    boss.ModifyingState(new CastState());
                }
            }
        }
        else
        {
            //boss.ModifyingState(new MoveState());
        }
    }

    public void ExitState(Boss boss)
    {
        AttackDelay = 0.0f;
        CastCooldown = 0.0f;
    }
}

public class AttackState : IBossState
{
    public void EnterState(Boss boss)
    {
    }

    public void ExcuteState(Boss boss)
    {
        boss.animator.SetTrigger("IsAttacking");
        boss.ModifyingState(new IdleState());
    }

    public void ExitState(Boss boss)
    {
    }
}

public class CastState : IBossState
{
    float ActivateDelay;
    GameObject SpellObject;
    float SpawnPos;
    float MaxHP;
    float CurHP;
    float LightningInterval;

    public void EnterState(Boss boss)
    {
        ActivateDelay = boss.ActivateDelay;
        SpawnPos = boss.SpawnPos;
        MaxHP = boss.MaxHP;
        CurHP = boss.CurrentHP;
        LightningInterval = boss.LightningInterval;

        boss.animator.SetBool("IsCasting", true);
    }

    public void ExcuteState(Boss boss)
    {
        CastingAction(boss);
    }

    public void ExitState(Boss boss)
    {
        ActivateDelay = boss.ActivateDelay;
        boss.animator.SetBool("IsCasting", false);
    }

    void CastingAction(Boss boss)
    {
        Collider2D HitEnemy = Physics2D.OverlapBox(boss.boxCollider2.bounds.center, boss.boxCollider2.bounds.size, 0.0f, LayerMask.GetMask("Player"));
        if (HitEnemy)
        {
            ActivateDelay -= Time.deltaTime;
            if (ActivateDelay <= 0.0f)
            {
                float CurHealthPer = (CurHP / MaxHP) * 100.0f;
                SpellAction(boss, CurHealthPer);
            }
        }
        else
        {
            boss.ModifyingState(new IdleState());
        }
    }

    void SpellAction(Boss boss, float HP)
    {
        if (HP >= 70.0f)
        {
            SpellObject = boss.lightningPool.UsingPool();
            if (SpellObject)
            {
                ActivateDelay = boss.ActivateDelay;
                boss.animator.SetTrigger("IsSpelling");
                SpellObject.transform.position = new Vector3(boss.TargetCharacter.transform.position.x, boss.TargetCharacter.transform.position.y * SpawnPos, boss.TargetCharacter.transform.position.z);
                boss.ModifyingState(new IdleState());
            }
        }
        else if (30.0f < HP && HP < 70.0f)
        {
            for (int i = 0; i < 5; i++)
            {
                SpellObject = boss.lightningPool.UsingPool();
                if (SpellObject)
                {
                    ActivateDelay = boss.ActivateDelay;
                    boss.animator.SetTrigger("IsSpelling");
                    SpellObject.transform.position = new Vector3(boss.transform.position.x * LightningInterval * i, boss.TargetCharacter.transform.position.y * SpawnPos, boss.TargetCharacter.transform.position.z);
                }
            }
        }
        else if (HP <= 30.0f)
        {

        }
    }
}

public class Boss : MonoBehaviour
{
    public GameObject TargetCharacter;
    public LightningPool lightningPool;

    public float AttackDelay;
    public float ActivateDelay;
    public float CastCooldown;
    public float SpawnPos;
    public float MaxHP;
    public float CurrentHP;
    public float LightningInterval;

    [HideInInspector] public Animator animator;
    [HideInInspector] public Rigidbody2D rigidbody2;
    [HideInInspector] public Rigidbody2D TargetRigidbody2;
    [HideInInspector] public BoxCollider2D boxCollider2;

    IBossState bossState;
    SpriteRenderer spriteRenderer;
    CapsuleCollider2D capsuleCollider2;
    CapsuleCollider2D TargetCapsule;

    void Awake()
    {
        rigidbody2 = GetComponent<Rigidbody2D>();
        TargetRigidbody2 = TargetCharacter.GetComponent<Rigidbody2D>();
        TargetCapsule = TargetCharacter.GetComponent<CapsuleCollider2D>();
        boxCollider2 = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        capsuleCollider2 = GetComponent<CapsuleCollider2D>();
        bossState = new IdleState();
        bossState.EnterState(this);

        AttackDelay = 3.0f;
        ActivateDelay = 5.0f;
        SpawnPos = 0.4f;
        MaxHP = 100.0f;
        CurrentHP = MaxHP;
    }

    void Start()
    {
    }

    void Update()
    {
        // 상태별 ChangeState호출
        CheckTargetPosition();
        bossState.ExcuteState(this);
    }

    public void ModifyingState(IBossState NewState)
    {
        if (bossState.ToString() == NewState.ToString())
        {
            return;
        }
        bossState.ExitState(this);
        bossState = NewState;
        bossState.EnterState(this);
    }

    void CheckTargetPosition()
    {
        if (TargetCapsule.transform.position.x > capsuleCollider2.transform.position.x)
        {
            spriteRenderer.flipX = true;
            capsuleCollider2.offset = new Vector2(-0.34f, -0.2f);
        }
        else
        {
            spriteRenderer.flipX = false;
            capsuleCollider2.offset = new Vector2(0.34f, -0.2f);
        }
    }

    public void TakeDamage(float damage)
    {
        if (bossState.ToString() == "IdleState")
        {
            CurrentHP -= damage;
            if (CurrentHP <= 0)
            {
                CurrentHP = 0.0f;
                animator.SetTrigger("IsDeath");
                gameObject.SetActive(false);
            }
            else
            {
                animator.SetTrigger("IsHurting");
                if (!spriteRenderer.flipX)
                {
                    rigidbody2.AddForce(Vector2.right * 1001.0f, ForceMode2D.Impulse);
                }
                else
                {
                    rigidbody2.AddForce(Vector2.left * 1001.0f, ForceMode2D.Impulse);
                }
            }
        }
    }


}
