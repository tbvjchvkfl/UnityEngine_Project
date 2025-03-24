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
    GameObject SkillNotice;
    BossCharacter Boss;
    AnimationClip SlamptoIdle;
    Vector3 TargetLocation;
    bool bIsSlampReady;
    float AppearHeight;
    float ExcuteDelayTime;
    float SkillAfterExcuteDelayTime;

    public void EnterState(BossCharacter boss)
    {
        SkillEssentialData(boss);
        CheckTargetLoc(boss);
    }

    public void ExcuteState(BossCharacter boss)
    {
        boss.AnimationController.SetBool("SlamReady", bIsSlampReady);
        if (SkillNotice)
        {
            ExcuteDelayTime -= Time.deltaTime;
            SkillNotice.GetComponent<SpriteRenderer>().color += new Color(0.0f, 0.0f, 0.0f, Time.deltaTime * 0.5f);
            if (ExcuteDelayTime <= 0.0f && SkillNotice.GetComponent<SpriteRenderer>().color.a >= 1.0f)
            {
                
                SkillNotice.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                GameObject.Destroy(SkillNotice);
                boss.transform.position = TargetLocation;
                boss.AnimationController.SetTrigger("Skill Attack");
                boss.StartCoroutine(SkillAttackAfterDelay());
            }
        }
    }

    public void ExitState(BossCharacter boss)
    {
        boss.bIsSkillAttack = false;
        boss.bIsCameraMoving = false;
    }

    IEnumerator SkillAttackAfterDelay()
    {
        // 여기서 레벨 무언가를 소환
        
        yield return new WaitForSeconds(SkillAfterExcuteDelayTime);
        bIsSlampReady = false;
        Boss.AnimationController.SetBool("SlamReady", bIsSlampReady);
        yield return new WaitForSeconds(SlamptoIdle.length);
        Boss.ModifyingState(new BossIdleState());
    }

    void SkillEssentialData(BossCharacter boss)
    {
        Boss = boss;
        SkillAfterExcuteDelayTime = boss.SkillAfterExcuteDelayTime;
        SlamptoIdle = boss.SlamptoIdle;
        bIsSlampReady = true;
        ExcuteDelayTime = boss.SkillExcuteDelayTime;
        AppearHeight = boss.AppearHeight;
        boss.bIsSkillAttack = true;
    }

    void CheckTargetLoc(BossCharacter boss)
    {
        TargetLocation = new Vector3(boss.TargetCharacter.transform.position.x, boss.TargetCharacter.transform.position.y + AppearHeight, boss.TargetCharacter.transform.position.z);
        SkillNotice = GameObject.Instantiate<GameObject>(boss.SkillNotice);
        SkillNotice.transform.position = TargetLocation;
        if (boss.transform.position.x < boss.TargetCharacter.transform.position.x)
        {
            SkillNotice.GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            SkillNotice.GetComponent<SpriteRenderer>().flipX = false;
        }
    }
}

public class BossCharacter : MonoBehaviour
{
    [Header("GameObject")]
    public GameObject TargetCharacter;
    public BoxCollider2D AttackForward;
    public BoxCollider2D AttackBackward;
    public BoxCollider2D SkillAttackSpace;
    public BoxCollider2D SkillAttackPoint;
    public GameObject SkillNotice;
    public GameObject Camera;
    public GameObject BattleObject;
    
    [Header("Boss State Value")]
    public float KickAttackDelay;
    public float SkillDelayTime;
    public float SkillExcuteDelayTime;
    public float AppearHeight;
    public float SkillAfterExcuteDelayTime;
    public float PlatformMovingDistance;

    public float FirstPhaseMaxHP;
    public float MiddlePhaseMaxHP;
    public float LastPhaseMaxHP;
    public float CurrentHP;
    public float KnockBackPower;

    [Header("Component Value")]
    public float CameraZoomSpeed;



    [Header("Animation Clip")]
    public AnimationClip PunchAttack;
    public AnimationClip KickAttack;
    public AnimationClip GroundSlashAttack;
    public AnimationClip SlamptoIdle;

    [HideInInspector] public Animator AnimationController;
    [HideInInspector] public Rigidbody2D CharacterBody;
    [HideInInspector] public bool bIsSkillAttack;
    [HideInInspector] public bool bIsSlamReady;
    [HideInInspector] public bool bIsFirstPhase;
    [HideInInspector] public bool bIsMiddlePhase;
    [HideInInspector] public bool bIsLastPhase;
    [HideInInspector] public bool bIsCameraMoving;

    IBossCharacterState BossState;

    SpriteRenderer SpriteRenderer;
    BoxCollider2D AttackPoint;
    Vector3 InitBattleObjectLocation;
    Vector3 BattleObjTargetLocation;

    void Awake()
    {
        AnimationController = GetComponent<Animator>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        CharacterBody = GetComponent<Rigidbody2D>();
        BossState = new BossIdleState();
        BossState.EnterState(this);

        InitBattleObjectLocation = BattleObject.transform.position;
        BattleObjTargetLocation = new Vector3(InitBattleObjectLocation.x, InitBattleObjectLocation.y + PlatformMovingDistance, InitBattleObjectLocation.z);

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
        CameraZoomSet();
        SetAttackPlace();
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
        if (bIsSkillAttack)
        {
            return;
        }
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

    void AnimEventSkillAttack()
    {
        bIsCameraMoving = true;
        Collider2D AttackCollision = Physics2D.OverlapBox(SkillAttackPoint.bounds.center, SkillAttackPoint.bounds.size, 0.0f, LayerMask.GetMask("Player"));
        if (AttackCollision)
        {
            AttackCollision.gameObject.GetComponent<PlayerInfo>().bIsStun = true;
            AttackCollision.gameObject.GetComponent<PlayerInfo>().TakeDamage(1);
        }
    }

    public void CameraZoomSet()
    {
        if (Camera && bIsCameraMoving)
        {
            Camera.GetComponent<Camera>().orthographicSize += Time.deltaTime * CameraZoomSpeed;
            if (Camera.GetComponent<Camera>().orthographicSize >= 10.0f)
            {
                Camera.GetComponent<Camera>().orthographicSize = 10.0f;
            }
        }
        if (Camera && !bIsCameraMoving)
        {
            Camera.GetComponent<Camera>().orthographicSize -= Time.deltaTime * CameraZoomSpeed;
            if (Camera.GetComponent<Camera>().orthographicSize <= 5.0f)
            {
                Camera.GetComponent<Camera>().orthographicSize = 5.0f;
            }
        }
    }

    void SetAttackPlace()
    {
        if (bIsCameraMoving)
        {
            BattleObject.transform.Translate(Vector3.up * 0.1f);
            if (BattleObject.transform.position.y >= BattleObjTargetLocation.y)
            {
                BattleObject.transform.position = BattleObjTargetLocation;
            }
        }
        else
        {
            BattleObject.transform.Translate(Vector3.down * 0.1f);
            if (BattleObject.transform.position.y <= InitBattleObjectLocation.y)
            {
                BattleObject.transform.position = InitBattleObjectLocation;
            }
        }
    }
}
