using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerInfo : MonoBehaviour
{

    // ====================================
    //          - Public Data-
    // ====================================
    [Header("Component")]
    public GameObject EnemyCharacter;
    public GameObject FGoalObj;
    public GameObject SGoalObj;
    public GameObject TGoalObj;
    public GameObject LGoalObj;

    [Header("Basic Data")]
    public float KnockBackDistance;
    public float StunTime;
    public float GoalUIHidingTime;

    public bool bIsStun { get; set; }
    public bool bIsHit { get; set; }
    public bool bIsDeath { get; set; }

    public int CurrentHP { get; private set; }
    public int PocketItem {  get; private set; }
    public float HideTime { get; private set; }
    public bool bIsHide {  get; private set; }

    [Header("Animation Data")]
    public AnimationClip HitAnim;
    public AnimationClip DeathAnim;

    [Header("Character UI")]
    public GameObject BulletPocketUI;
    public GameObject BulletItem_0;
    public GameObject BulletItem_1;
    public GameObject BulletItem_2;
    public GameObject GoalUI;
    public Image GoalUIIMG;

    // ====================================
    //          - Private Data-
    // ====================================
    Rigidbody2D CharacterBody;
    Animator AnimationController;
    Stack<GameObject> GoalObjects;

    void Awake()
    {
        CharacterBody = GetComponent<Rigidbody2D>();
        AnimationController = GetComponent<Animator>();
        GoalObjects = new Stack<GameObject>();
        HideTime = GoalUIHidingTime;
    }

    void Start()
    {
        InitGoalObjects();
        if (GameManager.Instance.bIsBossBattle)
        {
            GoalUIIMG.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        CheckStun();
        ShowAndHideBulletPocketUI();
        ModifyBulletUI();
        CheckGoalObjectLocation();
        DesiredGoalUIVisibility();
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
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            transform.position = new Vector3(-6.4f, 0.0f, transform.position.z);
        }
        else
        {
            transform.position = GameManager.Instance.PlayerLocation;
        }
        CurrentHP = GameManager.Instance.PlayerHP;
        
    }

    // UI 관련 함수
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

    void InitGoalObjects()
    {
        GoalObjects.Push(LGoalObj);
        GoalObjects.Push(TGoalObj);
        GoalObjects.Push(SGoalObj);
        GoalObjects.Push(FGoalObj);
    }

    void CheckGoalObjectLocation()
    {
        if (GoalObjects.Count != 0)
        {
            if (GoalObjects.Peek())
            {
                Vector2 TargetDirection = GoalObjects.Peek().transform.position - GoalUI.transform.position;
                float TargetDegree = Mathf.Atan2(TargetDirection.x, TargetDirection.y) * Mathf.Rad2Deg;
                GoalUI.transform.rotation = Quaternion.Euler(0.0f, 0.0f, -1.0f * TargetDegree);
            }
            else
            {
                GoalObjects.Pop();
            }
        }
    }

    void DesiredGoalUIVisibility()
    {
        if (gameObject.GetComponent<PlayerInput>().bIsView)
        {
            ShowGoalUI();
        }
        HideGoalUI();
    }

    void ShowGoalUI()
    {
        HideTime = GoalUIHidingTime;
        GoalUIIMG.color = new Color(0.5f, 0.5f, 0.5f, 1.0f);
    }

    void HideGoalUI()
    {
        HideTime -= Time.deltaTime;
        if (HideTime <= 0.0f)
        {
            HideTime = 0.0f;
            GoalUIIMG.color -= new Color(0.0f, 0.0f, 0.0f, Time.deltaTime);
            if (GoalUIIMG.color.a <= 0.0f)
            {
                GoalUIIMG.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
            }
        }
    }
}
