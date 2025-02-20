using UnityEngine;
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
    private float ActivateDelay;
    private GameObject SpellObject;
    Rigidbody2D TargetCharacterRigid;

    public void EnterState(Boss boss)
    {
        ActivateDelay = boss.ActivateDelay;
        TargetCharacterRigid = boss.TargetCharacter.GetComponent<Rigidbody2D>();
        SpellObject = boss.SpellObject;
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
                ActivateDelay = boss.ActivateDelay;
                boss.animator.SetTrigger("IsSpelling");
                GameObject.Instantiate(SpellObject, boss.TargetCharacter.transform.position, boss.transform.rotation);
                boss.ModifyingState(new IdleState());
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

public class SpellState : IBossState
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
                boss.ModifyingState(new CastState());
            }
        }
        else
        {
            boss.ModifyingState(new IdleState());
        }
    }

    public void ExitState(Boss boss)
    {

    }
}

public class Boss : MonoBehaviour
{
    public GameObject SpellObject;
    public GameObject TargetCharacter;


    public Animator animator;
    public float AttackDelay;
    public float ActivateDelay;
    public float SpellCountTime;
    public float SpawnObjectLifeTime;

    public Rigidbody2D rigidbody2;
    public Rigidbody2D TargetRigidbody2;
    public BoxCollider2D boxCollider2;

    IBossState bossState;
    float SpellStartTime;

    private void Awake()
    {
        rigidbody2 = GetComponent<Rigidbody2D>();
        TargetRigidbody2 = TargetCharacter.GetComponent<Rigidbody2D>();
        boxCollider2 = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        bossState = new IdleState();
        bossState.EnterState(this);

        AttackDelay = 3.0f;
        ActivateDelay = 8.0f;
        SpellCountTime = 10.0f;
        SpawnObjectLifeTime = 1.5f;
        SpellStartTime = 0.0f;
    }

    void Start()
    {
        
    }

    void Update()
    {
        // 상태별 ChangeState호출
        //AutoTrasitionCheck();
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

    void AutoTrasitionCheck()
    {
        // 공격 범위 내에 있는가
        Collider2D HitEnemy = Physics2D.OverlapBox(boxCollider2.bounds.center, boxCollider2.bounds.size, 0.0f, LayerMask.GetMask("Player"));
        if (HitEnemy)
        {
            float TargetDistance = Vector3.Distance(TargetRigidbody2.transform.position, rigidbody2.transform.position);

            if (TargetDistance < 1.5f)
            {
                ModifyingState(new AttackState());
            }
            else
            {
                SpellStartTime -= Time.deltaTime;
                if (SpellStartTime <= 0.0f)
                {
                    ModifyingState(new CastState());
                    SpellStartTime = SpellCountTime;
                }
            }
        }
        else
        {
            ModifyingState(new IdleState());
        }
    }
/*
    IEnumerator Idle()
    {
        //Debug.Log("Idle");


        while (true)
        {
            //Debug.Log("Idle Looping");
            yield return null;
        }
    }

    IEnumerator Move()
    {


        while (true)
        {
            yield return null;
        }
    }

    IEnumerator Attack()
    {


        while (true)
        {
            Debug.Log("Attack");
            animator.SetTrigger("IsAttacking");
            yield return new WaitForSeconds(3.0f);
        }
    }

    IEnumerator Cast()
    {
        while (true)
        {
            Debug.Log("Cast");
            animator.SetBool("IsCasting", true);
            yield return null;
        }
    }*/
}
