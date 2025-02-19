using UnityEngine;
using System.Collections;

/*public enum CharacterState
{
    Idle,
    Move,
    Attack,
    Cast,
    Spell,
    Hit,
    Death
}*/

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
    }

    public void ExitState(Boss boss)
    {
    }
}

public class AttackState : IBossState
{
    private float AttackDelay;

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
    }

    public void ExitState(Boss boss)
    {
        if(AttackDelay != 0.0f)
        {
            AttackDelay = 0.0f;
        }
    }
}

public class CastState : IBossState
{
    public void EnterState(Boss boss)
    {

    }

    public void ExcuteState(Boss boss)
    {
    
    }

    public void ExitState(Boss boss)
    {
    
    }
}

public class SpellState : IBossState
{
    public void EnterState(Boss boss)
    {

    }

    public void ExcuteState(Boss boss)
    {

    }

    public void ExitState(Boss boss)
    {

    }
}

public class Boss : MonoBehaviour
{
    public GameObject SpellObject;
    public Rigidbody2D TargetCharacterRigidbody;
    public Animator animator;
    public float AttackDelay;

    Rigidbody2D rigidbody2;
    BoxCollider2D boxCollider2;

    private IBossState bossState;

    private void Awake()
    {
        rigidbody2 = GetComponent<Rigidbody2D>();
        boxCollider2 = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        bossState = new IdleState();
        bossState.EnterState(this);

        AttackDelay = 3.0f;
    }

    void Start()
    {

    }

    void Update()
    {
        // 상태별 ChangeState호출
        AutoTrasitionCheck();
        bossState.ExcuteState(this);
    }

    void ModifyingState(IBossState NewState)
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
            float TargetDistance = Vector3.Distance(TargetCharacterRigidbody.transform.position, rigidbody2.transform.position);
            if (TargetDistance < 1.5f)
            {
                ModifyingState(new AttackState());
            }
            else
            {
                ModifyingState(new CastState());
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
