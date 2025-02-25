using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [Header("GameObject")]
    public GameObject playerHPGuageIndex;
    public GameObject Player;
    public GameObject Boss;

    [Header("Script")]
    public HPGuage FirstHealthGuage;
    public HPGuage MiddleHealthGuage;
    public HPGuage LastHealthGuage;

    [Header("UI Position")]
    public Transform playerHPGuagePos;

    [Header("UI Component")]
    public Slider playerHealthController;
    public Slider bossFirstHPController;
    public Slider bossMiddleHPController;
    public Slider bossLastHPController;
    public HorizontalLayoutGroup playerHealthBar;

    bool FirstGuage;
    bool MiddleGuage;
    bool LastGuage;



    void Awake()
    {
        Invoke("InitEssentialData", 1.0f);
    }

    void InitEssentialData()
    {
        playerHealthController.maxValue = Player.gameObject.GetComponent<Player>().maxHP / 100.0f;
        playerHealthController.minValue = 0.0f;
        playerHealthController.value = Player.gameObject.GetComponent<Player>().curHP / 100.0f;

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
        if (playerHPGuageIndex)
        {
            for (int i = 0; i < 3; i++)
            {
                GameObject.Instantiate(playerHPGuageIndex, playerHPGuagePos);
            }
            ModifyPlayerImageSize(true);
        }
        Invoke(nameof(CallInvo), 1.0f);
    }

    void Update()
    {
        playerHealthController.value = Player.gameObject.GetComponent<Player>().curHP / 100.0f;
        FirstGuage = Boss.gameObject.GetComponent<Boss>().bIsFirstPhase;
        MiddleGuage = Boss.gameObject.GetComponent<Boss>().bIsMiddlePhase;
        LastGuage = Boss.gameObject.GetComponent<Boss>().bIsLastPhase;
        ControllBossHealthGuage();
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

    void ModifyPlayerImageSize(bool InModify)
    {
        playerHealthBar.childControlWidth = InModify;
        playerHealthBar.childControlHeight = InModify;
        playerHealthBar.childForceExpandWidth = InModify;
        playerHealthBar.childForceExpandHeight = InModify;
    }

    void CallInvo()
    {
        ModifyPlayerImageSize(false);
    }
}
