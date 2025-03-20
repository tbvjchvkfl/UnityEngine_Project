using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public interface IBossCharacterState
{
    void EnterState(BossCharacter boss);
    void ExcuteState(BossCharacter boss);
    void ExitState(BossCharacter boss);
}

public class BossIdleState : IBossCharacterState
{
    float SkillDelayTime;
    float NormalAttackDelay;
    public void EnterState(BossCharacter boss)
    {
        NormalAttackDelay = boss.KickAttackDelay;
        SkillDelayTime = boss.SkillDelayTime;
    }

    public void ExcuteState(BossCharacter boss)
    {
        Collider2D HitEnemy = Physics2D.OverlapBox(boss.SkillAttackSpace.bounds.center, boss.SkillAttackSpace.bounds.size, 0.0f, LayerMask.GetMask("Player"));
        if (HitEnemy)
        {
            float TargetDistance = Vector3.Distance(boss.TargetCharacter.transform.position, boss.transform.position);
            if (TargetDistance < 2.0f)
            {
                NormalAttackDelay -= Time.deltaTime;
                if (NormalAttackDelay <= 0.0f)
                {
                    boss.ModifyingState(new BossAttackState());
                }
            }
            else
            {
                SkillDelayTime -= Time.deltaTime;
                if (SkillDelayTime <= 0)
                {
                    boss.ModifyingState(new BossSkillState());
                }
            }
        }
        else
        {
            //boss.ModifyingState(new MoveState());
        }
    }

    public void ExitState(BossCharacter boss)
    {
        NormalAttackDelay = 0.0f;
    }
}

public class BossAttackState : IBossCharacterState
{
    BossCharacter Boss;
    AnimationClip NormalAttack;
    public void EnterState(BossCharacter boss)
    {
        Boss = boss;
        NormalAttack = boss.KickAttack;
        boss.AnimationController.SetTrigger("Normal Attack");
        boss.StartCoroutine(NormalAttackTransition());
    }

    public void ExcuteState(BossCharacter boss)
    {
        
    }

    public void ExitState(BossCharacter boss)
    {
    }

    IEnumerator NormalAttackTransition()
    {
        yield return new WaitForSeconds(NormalAttack.length);
        Boss.ModifyingState(new BossIdleState());
    }
}

public class BossSkillState : IBossCharacterState
{
    Vector3 TargetLocation;
    
    public void EnterState(BossCharacter boss)
    {
        TargetLocation = boss.TargetCharacter.transform.position;
        boss.AnimationController.SetTrigger("Skill Attack");
        boss.ModifyingState(new BossIdleState());
    }

    public void ExcuteState(BossCharacter boss)
    {
        
    }

    public void ExitState(BossCharacter boss)
    {

    }
}

public class BossCharacter : MonoBehaviour
{
    [Header("GameObject")]
    public GameObject TargetCharacter;
    public BoxCollider2D SkillAttackSpace;
    public BoxCollider2D AttackForward;
    public BoxCollider2D AttackBackward;

    [Header("Boss State Value")]
    public float KickAttackDelay;
    public float SkillDelayTime;
    public float FirstPhaseMaxHP;
    public float MiddlePhaseMaxHP;
    public float LastPhaseMaxHP;
    public float CurrentHP;
    public float KnockBackPower;

    [Header("Animation Clip")]
    public AnimationClip PunchAttack;
    public AnimationClip KickAttack;
    public AnimationClip GroundSlashAttack;

    IBossCharacterState BossState;

    [HideInInspector] public Animator AnimationController;

    [HideInInspector] public bool bIsFirstPhase;
    [HideInInspector] public bool bIsMiddlePhase;
    [HideInInspector] public bool bIsLastPhase;

    SpriteRenderer SpriteRenderer;
    BoxCollider2D AttackPoint;

    void Awake()
    {
        AnimationController = GetComponent<Animator>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        BossState = new BossIdleState();
        BossState.EnterState(this);

        bIsFirstPhase = true;
        bIsMiddlePhase = false;
        bIsLastPhase = false;

        FirstPhaseMaxHP = 100.0f;
        MiddlePhaseMaxHP = 100.0f;
        LastPhaseMaxHP = 100.0f;

        CurrentHP = FirstPhaseMaxHP;
    }

    void Update()
    {
        CheckTargetPosition();
        BossState.ExcuteState(this);
    }

    public void ModifyingState(IBossCharacterState NewState)
    {
        if (BossState.ToString() == NewState.ToString())
        {
            return;
        }
        BossState.ExitState(this);
        BossState = NewState;
        BossState.EnterState(this);
    }

    void CheckTargetPosition()
    {
        if (TargetCharacter.gameObject.transform.position.x < transform.position.x)
        {
            SpriteRenderer.flipX = true;
            AttackPoint = AttackBackward;
        }
        else if (TargetCharacter.gameObject.transform.position.x > transform.position.x)
        {
            SpriteRenderer.flipX = false;
            AttackPoint = AttackForward;
        }
    }

    void AnimEventNormalAttack()
    {
        Collider2D AttackCollision = Physics2D.OverlapBox(AttackPoint.bounds.center, AttackPoint.bounds.size, 0.0f, LayerMask.GetMask("Player"));
        if (AttackCollision)
        {
            AttackCollision.gameObject.GetComponent<PlayerInfo>().TakeDamage(1);
        }
    }
}
