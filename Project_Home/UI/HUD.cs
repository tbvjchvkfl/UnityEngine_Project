using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [Header("GameObject")]
    public GameObject Player;
    public GameObject Boss;
    public GameObject PlayerHealthGuageFirst;
    public GameObject PlayerHealthGuageSecond;

    [Header("Script")]
    public HPGuage FirstHealthGuage;
    public HPGuage MiddleHealthGuage;
    public HPGuage LastHealthGuage;

    [Header("UI Component")]
    public Slider playerHealthController;
    public Slider bossFirstHPController;
    public Slider bossMiddleHPController;
    public Slider bossLastHPController;

    [Header("UI Parameter")]
    public float PlayerHealthGuageScrollSpeed;


    // Player
    RectTransform PlayerFHG;
    RectTransform PlayerSHG;

    bool DoOnceF;
    bool DoOnceS;

    // Boss
    bool FirstGuage;
    bool MiddleGuage;
    bool LastGuage;


    void Awake()
    {
        playerHealthController.maxValue = Player.gameObject.GetComponent<Player>().maxHP / 100.0f;
        playerHealthController.minValue = 0.0f;
        playerHealthController.value = Player.gameObject.GetComponent<Player>().curHP / 100.0f;

        PlayerFHG = PlayerHealthGuageFirst.GetComponent<RectTransform>();
        PlayerSHG = PlayerHealthGuageSecond.GetComponent<RectTransform>();
        DoOnceF = true;
        DoOnceS = true;

        Invoke("InitEssentialData", 1.0f);
    }

    void InitEssentialData()
    {
        bossFirstHPController.maxValue = Boss.gameObject.GetComponent<Boss>().FirstPhaseMaxHP / 100.0f;
        bossMiddleHPController.maxValue = Boss.gameObject.GetComponent<Boss>().MiddlePhaseMaxHP / 100.0f;
        bossLastHPController.maxValue = Boss.gameObject.GetComponent<Boss>().LastPhaseMaxHP / 100.0f;
        bossFirstHPController.minValue = 0.0f;
        bossMiddleHPController.minValue = 0.0f;
        bossLastHPController.minValue = 0.0f;
        bossFirstHPController.value = Boss.gameObject.GetComponent<Boss>().CurrentHP / 100.0f;
        bossMiddleHPController.value = Boss.gameObject.GetComponent<Boss>().CurrentHP / 100.0f;
        bossLastHPController.value = Boss.gameObject.GetComponent<Boss>().CurrentHP / 100.0f;
    }

    void Start()
    {
        
    }

    void Update()
    {
        if (Player)
        {
            playerHealthController.value = Player.gameObject.GetComponent<Player>().curHP / 100.0f;
            PlayerFHG.transform.Translate(Vector3.left * Time.deltaTime * PlayerHealthGuageScrollSpeed);
            PlayerSHG.transform.Translate(Vector3.left * Time.deltaTime * PlayerHealthGuageScrollSpeed);
            ResetHGPosition();
        }
        if (Boss)
        {
            FirstGuage = Boss.gameObject.GetComponent<Boss>().bIsFirstPhase;
            MiddleGuage = Boss.gameObject.GetComponent<Boss>().bIsMiddlePhase;
            LastGuage = Boss.gameObject.GetComponent<Boss>().bIsLastPhase;
            ControllBossHealthGuage();
        }
    }

    void ResetHGPosition()
    {
        if (DoOnceF)
        {
            if (PlayerSHG.anchoredPosition.x <= 0.0f)
            {
                PlayerFHG.anchoredPosition = new Vector2(370.0f, 0.0f);
                DoOnceF = false;
                DoOnceS = true;
            }
        }
        if (DoOnceS)
        {
            if (PlayerFHG.anchoredPosition.x <= 0.0f)
            {
                PlayerSHG.anchoredPosition = new Vector2(370.0f, 0.0f);
                DoOnceF = true;
                DoOnceS = false;
            }
        }
    }

    void ControllBossHealthGuage()
    {
        if (FirstGuage)
        {
            bossFirstHPController.value = Boss.gameObject.GetComponent<Boss>().CurrentHP / 100.0f;
        }
        if (MiddleGuage)
        {
            bossMiddleHPController.value = Boss.gameObject.GetComponent<Boss>().CurrentHP / 100.0f;
            FirstHealthGuage.StopRotation = true;
        }
        if (LastGuage)
        {
            bossLastHPController.value = Boss.gameObject.GetComponent<Boss>().CurrentHP / 100.0f;
            MiddleHealthGuage.StopRotation = true;
        }
    }
}
