using System.Collections;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{

    // ====================================
    //          - Public Data-
    // ====================================
    [Header("Component")]
    public GameObject EnemyCharacter;

    [Header("Basic Data")]
    public float KnockBackDistance;
    public float StunTime;
    public int CurrentHP;

    [HideInInspector] public bool bIsStun;
    [HideInInspector] public bool bIsHit;
    [HideInInspector] public bool bIsDeath;

    [Header("Animation Data")]
    public AnimationClip HitAnim;
    public AnimationClip DeathAnim;

    [Header("Script")]
    public PlayerHUD PlayerCharacterHUD;

    // ====================================
    //          - Private Data-
    // ====================================
    Rigidbody2D CharacterBody;
    Animator AnimationController;


    void Awake()
    {
        CharacterBody = GetComponent<Rigidbody2D>();
        AnimationController = GetComponent<Animator>();
    }

    void Start()
    {
        GetGameManagerData();
    }

    void Update()
    {
        CheckStun();
    }

    public void TakeDamage(int Damage)
    {
        CurrentHP -= Damage;
        GameManager.Instance.SetPlayerHP(Damage);
        bIsHit = true;

        if (CurrentHP <= 0)
        {
            AnimationController.SetTrigger("Die");
        }
        else
        {
            if (transform.position.x < EnemyCharacter.transform.position.x)
            {
                CharacterBody.AddForce(new Vector2(-1 * KnockBackDistance, 0.35f * KnockBackDistance), ForceMode2D.Impulse);
            }
            else if (transform.position.x > EnemyCharacter.transform.position.x)
            {
                CharacterBody.AddForce(new Vector2(1 * KnockBackDistance, 0.35f * KnockBackDistance), ForceMode2D.Impulse);
            }
            AnimationController.SetTrigger("Hit");
            Invoke("ReturnHitStateValue", HitAnim.length);
        }
    }

    void ReturnHitStateValue()
    {
        bIsHit = false;
    }

    void CheckStun()
    {
        if (bIsStun)
        {
            StunTime -= Time.deltaTime;
            if (StunTime <= 0.0f)
            {
                bIsStun = false;
            }
        }
        AnimationController.SetBool("Stun", bIsStun);
    }

    public void GetGameManagerData()
    {
        CurrentHP = GameManager.Instance.PlayerHP;
    }
}
