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
    public void EnterState(Boss boss)
    {

    }

    public void ExcuteState(Boss boss)
    {
        Collider2D HitEnemy = Physics2D.OverlapBox(boss.boxCollider2.bounds.center, boss.boxCollider2.bounds.size, 0.0f, LayerMask.GetMask("Player"));
        if (HitEnemy)
        {
            float TargetDistance = Vector3.Distance(boss.TargetRigidbody2.transform.position, boss.rigidbody2.transform.position);

            if (TargetDistance < 1.5f)
            {
                boss.ModifyingState(new AttackState());
            }
            else
            {
                // 여기서 CastState로 들어갈 쿨타임 정해주면 됨.
                boss.ModifyingState(new CastState());
            }
        }
        else
        {
            //boss.ModifyingState(new MoveState());
        }
    }

    public void ExitState(Boss boss)
    {
    }
}

public class AttackState : IBossState
{
    float AttackDelay;

    public void EnterState(Boss boss)
    {
        AttackDelay = 0.0f;
    }

    public void ExcuteState(Boss boss)
    {
        AttackDelay -= Time.deltaTime;
        if (AttackDelay <= 0.0f)
        {
            boss.animator.SetTrigger("IsAttacking");
            AttackDelay = boss.AttackDelay;
        }
        else
        {
            boss.ModifyingState(new IdleState());
        }
    }

    public void ExitState(Boss boss)
    {
        AttackDelay = 0.0f;
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
        Collider2D HitEnemy = Physics2D.OverlapBox(boss.boxCollider2.bounds.center, boss.boxCollider2.bounds.size, 0.0f, LayerMask.GetMask("Player"));
        if (HitEnemy)
        {
            ActivateDelay -= Time.deltaTime;
            if (ActivateDelay <= 0.0f)
            {
                float CurHealthPer = (CurHP / MaxHP) * 100.0f;
                if(CurHealthPer >= 70.0f)
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
                else if(30.0f < CurHealthPer && CurHealthPer < 70.0f)
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
                else if (CurHealthPer <= 30.0f)
                {

                }
            }
        }
        else
        {
            boss.ModifyingState(new IdleState());
        }
    }

    public void ExitState(Boss boss)
    {
        ActivateDelay = boss.ActivateDelay;
        boss.animator.SetBool("IsCasting", false);
    }
}

public class Boss : MonoBehaviour
{
    public GameObject TargetCharacter;
    public LightningPool lightningPool;

    public float AttackDelay;
    public float ActivateDelay;
    public float SpawnPos;
    public float MaxHP;
    public float CurrentHP;
    public float LightningInterval;

    [HideInInspector] public Animator animator;
    [HideInInspector] public Rigidbody2D rigidbody2;
    [HideInInspector] public Rigidbody2D TargetRigidbody2;
    [HideInInspector] public BoxCollider2D boxCollider2;

    IBossState bossState;
    

    void Awake()
    {
        rigidbody2 = GetComponent<Rigidbody2D>();
        TargetRigidbody2 = TargetCharacter.GetComponent<Rigidbody2D>();
        boxCollider2 = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
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
}
