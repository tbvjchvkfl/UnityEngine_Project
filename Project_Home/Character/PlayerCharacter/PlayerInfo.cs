using System.Collections;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{

    // ====================================
    //          - Public Data-
    // ====================================
    [Header("Component")]
    public GameObject EnemyCharacter;
    public GameObject BulletPocketUI;
    public GameObject BulletItem_0;
    public GameObject BulletItem_1;
    public GameObject BulletItem_2;

    [Header("Basic Data")]
    public float KnockBackDistance;
    public float StunTime;
    public int CurrentHP { get; private set; }
    public int PocketItem {  get; private set; }

    [HideInInspector] public bool bIsStun;
    [HideInInspector] public bool bIsHit;
    [HideInInspector] public bool bIsDeath;

    [Header("Animation Data")]
    public AnimationClip HitAnim;
    public AnimationClip DeathAnim;

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
        ShowAndHideBulletPocketUI();
        ModifyBulletUI();
    }

    public void TakeDamage(int Damage)
    {
        CurrentHP -= Damage;
        GameManager.Instance.SetPlayerHP(Damage);
        bIsHit = true;

        if (CurrentHP <= 0)
        {
            CurrentHP = 0;
            AnimationController.SetTrigger("Die");
            if (GameManager.Instance.bIsGameOver)
            {
                StartCoroutine(ShowGameOverUI());
            }
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

    IEnumerator ShowGameOverUI()
    {
        yield return new WaitForSeconds(DeathAnim.length);
        HUD.Instance.ShowGameOverUI();
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

    public void RefreshPocketItem()
    {
        PocketItem = 0;
    }

    public void AddPocketItem()
    {
        PocketItem++;
    }

    public void RemovePocketItem()
    {
        PocketItem--;
    }

    void ModifyBulletUI()
    {
        if (PocketItem == 0)
        {
            BulletItem_0.SetActive(false);
            BulletItem_1.SetActive(false);
            BulletItem_2.SetActive(false);
        }
        if (PocketItem == 1)
        {
            BulletItem_0.SetActive(true);
            BulletItem_1.SetActive(false);
            BulletItem_2.SetActive(false);
        }
        else if (PocketItem == 2)
        {
            BulletItem_0.SetActive(true);
            BulletItem_1.SetActive(true);
            BulletItem_2.SetActive(false);
        }
        else if (PocketItem == 3)
        {
            BulletItem_0.SetActive(true);
            BulletItem_1.SetActive(true);
            BulletItem_2.SetActive(true);
        }
    }

    void ShowAndHideBulletPocketUI()
    {
        if (EnemyCharacter)
        {
            if (EnemyCharacter.GetComponent<BossCharacter>().bIsCameraMoving)
            {
                BulletPocketUI.SetActive(true);
            }
            else
            {
                BulletPocketUI.SetActive(false);
                BulletItem_0.SetActive(false);
                BulletItem_1.SetActive(false);
                BulletItem_2.SetActive(false);
            }
        }
    }
}
