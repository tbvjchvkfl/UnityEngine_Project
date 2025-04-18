using System.Collections;
using System.Collections.Generic;
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
    public void EnterState(BossCharacter boss)
    {
        boss.MovementDirection = Vector2.zero;
    }

    public void ExcuteState(BossCharacter boss)
    {
        Collider2D HitEnemy = Physics2D.OverlapBox(boss.SkillAttackSpace.bounds.center, boss.SkillAttackSpace.bounds.size, 0.0f, LayerMask.GetMask("Player"));
        if (HitEnemy)
        {
            float TargetDistance = Vector3.Distance(boss.TargetCharacter.transform.position, boss.transform.position);
            if (TargetDistance < 2.0f)
            {
                if (boss.bIsKickAttack)
                {
                    boss.ModifyingState(new BossAttackState());
                }
                else
                {
                    boss.ModifyingState(new BossComboAttackState());
                }
            }
            if(TargetDistance >= 2.0f)
            {
                if (boss.bIsSkillAttack)
                {
                    boss.ModifyingState(new BossSkillState());
                }
                else
                {
                    boss.ModifyingState(new BossMoveState());
                }
            }
        }
        else
        {
            boss.ModifyingState(new BossMoveState());
        }
    }

    public void ExitState(BossCharacter boss)
    {
    }
}

public class BossComboAttackState : IBossCharacterState
{
    BossCharacter Boss;

    public void EnterState(BossCharacter boss)
    {
        Boss = boss;
        boss.MovementDirection = Vector2.zero;
        boss.AnimationController.SetTrigger("Punch Attack");
        boss.StartCoroutine(PunchtoIdle());
    }
    public void ExcuteState(BossCharacter boss)
    {
    }
    public void ExitState(BossCharacter boss)
    {
    }

    IEnumerator PunchtoIdle()
    {
        yield return new WaitForSeconds(Boss.PunchAttack_A.length);
        yield return new WaitForSeconds(Boss.PunchAttack_B.length);
        yield return new WaitForSeconds(Boss.PunchAttack_C.length);
        Collider2D HitEnemy = Physics2D.OverlapBox(Boss.SkillAttackSpace.bounds.center, Boss.SkillAttackSpace.bounds.size, 0.0f, LayerMask.GetMask("Player"));
        if (HitEnemy)
        {
            float TargetDistance = Vector3.Distance(Boss.TargetCharacter.transform.position, Boss.transform.position);
            if (TargetDistance < 2.0f)
            {
                Boss.ModifyingState(new BossIdleState());
            }
            else
            {
                Boss.ModifyingState(new BossSkillState());
            }
        }
        else
        {
            Boss.ModifyingState(new BossIdleState());
        }
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
        boss.MovementDirection = Vector2.zero;
        boss.AnimationController.SetTrigger("Normal Attack");
        boss.StartCoroutine(NormalAttackTransition());
    }

    public void ExcuteState(BossCharacter boss)
    {
    }

    public void ExitState(BossCharacter boss)
    {
        boss.bIsKickAttack = false;
    }

    IEnumerator NormalAttackTransition()
    {
        yield return new WaitForSeconds(NormalAttack.length);
        Collider2D HitEnemy = Physics2D.OverlapBox(Boss.SkillAttackSpace.bounds.center, Boss.SkillAttackSpace.bounds.size, 0.0f, LayerMask.GetMask("Player"));
        if (HitEnemy)
        {
            float TargetDistance = Vector3.Distance(Boss.TargetCharacter.transform.position, Boss.transform.position);
            if (TargetDistance < 2.0f)
            {
                Boss.ModifyingState(new BossIdleState());
            }
            else
            {
                Boss.ModifyingState(new BossSkillState());
            }
        }
        else
        {
            Boss.ModifyingState(new BossIdleState());
        }
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
        boss.bIsPlatformMoving = false;
        boss.DestoryCannonBullet();
    }

    IEnumerator SkillAttackAfterDelay()
    {
        Boss.bIsCameraMoving = true;
        yield return new WaitForSeconds(0.5f);

        Boss.bIsPlatformMoving = true;
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
        boss.MovementDirection = Vector2.zero;
    }

    void CheckTargetLoc(BossCharacter boss)
    {
        TargetLocation = new Vector3(boss.TargetCharacter.transform.position.x, boss.TargetCharacter.transform.position.y + AppearHeight, boss.TargetCharacter.transform.position.z);
        SkillNotice = GameObject.Instantiate<GameObject>(boss.SkillNotice);
        SkillNotice.transform.position = TargetLocation;
        if (boss.transform.position.x < boss.TargetCharacter.transform.position.x)
        {
            SkillNotice.GetComponent<SpriteRenderer>().flipX = false;
        }
        else
        {
            SkillNotice.GetComponent<SpriteRenderer>().flipX = true;
        }
    }
}

public class BossMoveState : IBossCharacterState
{
    public void EnterState(BossCharacter boss)
    {
        
    }

    public void ExcuteState(BossCharacter boss)
    {
        SetCharacterMovement(boss);
        Collider2D HitEnemy = Physics2D.OverlapBox(boss.SkillAttackSpace.bounds.center, boss.SkillAttackSpace.bounds.size, 0.0f, LayerMask.GetMask("Player"));
        if (HitEnemy)
        {
            float TargetDistance = Vector3.Distance(boss.TargetCharacter.transform.position, boss.transform.position);
            if (TargetDistance < 2.0f)
            {
                if (boss.bIsKickAttack)
                {
                    boss.ModifyingState(new BossAttackState());
                }
                else
                {
                    boss.ModifyingState(new BossComboAttackState());
                }
            }
            if (TargetDistance >= 2.0f)
            {
                if (boss.bIsSkillAttack)
                {
                    boss.ModifyingState(new BossSkillState());
                }
            }
        }
    }

    public void ExitState(BossCharacter boss)
    {

    }

    void SetCharacterMovement(BossCharacter boss)
    {
        if (boss.TargetCharacter.transform.position.x <= boss.transform.position.x)
        {
            boss.MovementDirection = Vector2.left;
        }
        else if (boss.TargetCharacter.transform.position.x > boss.transform.position.x)
        {
            boss.MovementDirection = Vector2.right;
        }
        boss.CharacterBody.linearVelocity = new Vector2(boss.MovementDirection.x * boss.MoveSpeed, boss.CharacterBody.linearVelocityY);
    }
}

public class BossDieState : IBossCharacterState
{
    public void EnterState(BossCharacter boss)
    {
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
    public BoxCollider2D MeleeAttackSpace;
    public BoxCollider2D SkillAttackSpace;
    public BoxCollider2D SkillAttackPoint;
    public GameObject SkillNotice;
    public GameObject Camera;
    public GameObject BattleObject;
    public GameObject LeftClampingWall;
    public GameObject RightClampingWall;

    public GameObject BulletSpawnPoint;
    public GameObject CannonBullet;


    [Header("Boss State Value")]
    public float KickAttackDelay;
    public float SkillDelayTime;
    public float SkillExcuteDelayTime;
    public float AppearHeight;
    public float SkillAfterExcuteDelayTime;
    public float PlatformMovingDistance;
    public float MoveSpeed;

    public float FirstPhaseMaxHP;
    public float SecondPhaseMaxHP;
    public float ThirdPhaseMaxHP;

    public float KnockBackPower;

    [Header("Component Value")]
    public float CameraZoomSpeed;
    public float WallMovingSpeed;
    public float LeftWallMoveDistance;
    public float RightWallMoveDistance;


    [Header("Animation Clip")]
    public AnimationClip PunchAttack_A;
    public AnimationClip PunchAttack_B;
    public AnimationClip PunchAttack_C;
    public AnimationClip KickAttack;
    public AnimationClip GroundSlashAttack;
    public AnimationClip SlamptoIdle;
    public AnimationClip DeathAnim;

    // Public Hide Value
    [HideInInspector] public Vector2 MovementDirection;

    [HideInInspector] public Animator AnimationController;
    [HideInInspector] public Rigidbody2D CharacterBody;

    [HideInInspector] public float CurrentHP;

    [HideInInspector] public bool bIsSkillAttack;
    [HideInInspector] public bool bIsKickAttack;
    [HideInInspector] public bool bIsCameraMoving;
    [HideInInspector] public bool bIsPlatformMoving;
    [HideInInspector] public bool bIsFirstPhase;
    [HideInInspector] public bool bIsMiddlePhase;
    [HideInInspector] public bool bIsLastPhase;
    [HideInInspector] public bool bIsMove;
    

    // Private Value
    IBossCharacterState BossState;

    SpriteRenderer SpriteRenderer;

    Vector3 InitBattleObjectLocation;
    Vector3 BattleObjTargetLocation;
    Vector3 InitLeftClampingWallLocation;
    Vector3 InitRightClampingWallLocation;
    Vector3 LeftClampingTargetLocation;
    Vector3 RightClampingTargetLocation;

    float SkillCooldown;
    float KickCooldown;

    List<GameObject> BulletPocket;

    void Awake()
    {
        AnimationController = GetComponent<Animator>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        CharacterBody = GetComponent<Rigidbody2D>();
        BulletPocket = new List<GameObject>();
        BossState = new BossIdleState();
        BossState.EnterState(this);

        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("EnemyCharacter"));

        InitBattleObjectLocation = BattleObject.transform.position;
        BattleObjTargetLocation = new Vector3(InitBattleObjectLocation.x, InitBattleObjectLocation.y + PlatformMovingDistance, InitBattleObjectLocation.z);

        InitLeftClampingWallLocation = LeftClampingWall.transform.position;
        LeftClampingTargetLocation = new Vector3(InitLeftClampingWallLocation.x + LeftWallMoveDistance, InitLeftClampingWallLocation.y, InitLeftClampingWallLocation.z);

        InitRightClampingWallLocation = RightClampingWall.transform.position;
        RightClampingTargetLocation = new Vector3(InitRightClampingWallLocation.x + RightWallMoveDistance, InitRightClampingWallLocation.y, InitRightClampingWallLocation.z);

        SkillCooldown = SkillDelayTime;
        KickCooldown = KickAttackDelay;

        CurrentHP = FirstPhaseMaxHP;
        bIsFirstPhase = true;
    }

    void Update()
    {
        CheckAttackCooldown();
        CheckCharacterMove();
        CheckTargetPosition();
        CameraZoomSet();
        SetAttackPlace();
        SetClampingWallLoc();
        BossState.ExcuteState(this);
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
        }
        else if (TargetCharacter.gameObject.transform.position.x > transform.position.x)
        {
            SpriteRenderer.flipX = false;
        }
    }

    void CheckCharacterMove()
    {
        if (MovementDirection != Vector2.zero)
        {
            bIsMove = true;
        }
        else
        {
            bIsMove = false;
        }
        AnimationController.SetBool("Moving", bIsMove);
    }

    void AnimEventNormalAttack()
    {
        Collider2D AttackCollision = Physics2D.OverlapBox(MeleeAttackSpace.bounds.center, MeleeAttackSpace.bounds.size, 0.0f, LayerMask.GetMask("Player"));
        if (AttackCollision && !GameManager.Instance.PCInput.bIsRolling)
        {
            AttackCollision.gameObject.GetComponent<PlayerInfo>().TakeDamage(1);
        }
    }

    void AnimEventSkillAttack()
    {
        SpawnCannonBullet();
        StartCoroutine(CameraShake());
        Collider2D AttackCollision = Physics2D.OverlapBox(SkillAttackPoint.bounds.center, SkillAttackPoint.bounds.size, 0.0f, LayerMask.GetMask("Player"));
        if (AttackCollision && !GameManager.Instance.PCInput.bIsRolling)
        {
            AttackCollision.gameObject.GetComponent<PlayerInfo>().bIsStun = true;
            AttackCollision.gameObject.GetComponent<PlayerInfo>().TakeDamage(1);
        }
    }

    IEnumerator CameraShake()
    {
        Vector3 CameraInitLocation = Camera.transform.position;
        InvokeRepeating("CameraShaking", 0.0f, Time.deltaTime);
        yield return new WaitForSeconds(0.3f);
        CancelInvoke();
        Camera.transform.position = CameraInitLocation;
    }

    void CameraShaking()
    {
        float RandomValue = Random.Range(-0.1f, 0.1f);
        Vector3 NewLocation = new Vector3(Camera.transform.position.x + RandomValue, Camera.transform.position.y + RandomValue, Camera.transform.position.z);
        Camera.transform.position = NewLocation;
    }

    void SetAttackPlace()
    {
        if (bIsPlatformMoving)
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

    void SetClampingWallLoc()
    {
        if (bIsCameraMoving)
        {
            LeftClampingWall.transform.Translate(Vector3.left * WallMovingSpeed);
            if (LeftClampingWall.transform.position.x <= LeftClampingTargetLocation.x)
            {
                LeftClampingWall.transform.position = LeftClampingTargetLocation;
            }
            RightClampingWall.transform.Translate(Vector3.right * WallMovingSpeed);
            if (RightClampingWall.transform.position.x >= RightClampingTargetLocation.x)
            {
                RightClampingWall.transform.position = RightClampingTargetLocation;
            }
        }
        else
        {
            LeftClampingWall.transform.Translate(Vector3.right * WallMovingSpeed);
            if (LeftClampingWall.transform.position.x >= InitLeftClampingWallLocation.x)
            {
                LeftClampingWall.transform.position = InitLeftClampingWallLocation;
            }
            RightClampingWall.transform.Translate(Vector3.left * WallMovingSpeed);
            if (RightClampingWall.transform.position.x <= InitRightClampingWallLocation.x)
            {
                RightClampingWall.transform.position = InitRightClampingWallLocation;
            }
        }
    }

    IEnumerator CharacterShake()
    {
        Vector3 BackupCurrentPos = transform.position;
        InvokeRepeating("CharacterShaking", 0.0f, Time.deltaTime);
        yield return new WaitForSeconds(0.3f);
        CancelInvoke();
        transform.position = BackupCurrentPos;
    }

    void CharacterShaking()
    {
        float RandomValue = Random.Range(-0.1f, 0.1f);
        Vector3 NewLocation = new Vector3(transform.position.x + RandomValue, transform.position.y, transform.position.z);
        transform.position = NewLocation;
    }

    void CheckAttackCooldown()
    {
        if (!bIsSkillAttack)
        {
            SkillCooldown -= Time.deltaTime;
        }
        if (!bIsKickAttack)
        {
            KickCooldown -= Time.deltaTime;
        }
        if(SkillCooldown <= 0.0f)
        {
            bIsSkillAttack = true;
            SkillCooldown = SkillDelayTime;
        }
        if (KickCooldown <= 0.0f)
        {
            bIsKickAttack = true;
            KickCooldown = KickAttackDelay;
        }
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

    public void TakeDamage(float Damage)
    {
        CurrentHP -= Damage;
        if (CurrentHP <= 0.0f && bIsFirstPhase)
        {
            bIsFirstPhase = false;
            bIsMiddlePhase = true;
            CurrentHP = SecondPhaseMaxHP;
        }
        else if (CurrentHP <= 0.0f && !bIsFirstPhase && bIsMiddlePhase)
        {
            bIsMiddlePhase = false;
            bIsLastPhase = true;
            CurrentHP = ThirdPhaseMaxHP;
        }
        else if (CurrentHP <= 0.0f && !bIsFirstPhase && !bIsMiddlePhase && bIsLastPhase)
        {
            CurrentHP = 0.0f;
            ModifyingState(new BossDieState());
            AnimationController.SetTrigger("Die");
            StartCoroutine(ShowGameClearUI());
        }
        else
        {
            StartCoroutine(CharacterShake());
        }
    }

    IEnumerator ShowGameClearUI()
    {
        yield return new WaitForSeconds(DeathAnim.length);
        HUD.Instance.ShowGameClearUI();
    }

    public void SpawnCannonBullet()
    {
        for (int i = 0; i < GameManager.Instance.PCInfo.CurrentHP; i++)
        {
            GameObject Bullet = GameObject.Instantiate(CannonBullet);
            BulletPocket.Add(Bullet);
            float RandomValue = Random.Range(-6.0f, 6.0f);
            Bullet.transform.position = new Vector3(BulletSpawnPoint.transform.position.x + RandomValue, BulletSpawnPoint.transform.position.y, BulletSpawnPoint.transform.position.z);
        }
    }

    public void DestoryCannonBullet()
    {
        foreach (var PocItem in BulletPocket)
        {
            if (PocItem)
            {
                Destroy(PocItem.gameObject);
            }
        }
        BulletPocket.Clear();
    }
}



