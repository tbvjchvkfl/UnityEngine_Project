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

    [Header("UI Position")]
    public Transform playerHPGuagePos;
    public Transform bossHPGuagePos;

    [Header("UI Component")]
    public Slider playerHealthController;
    public Slider bossHealthController;
    public HorizontalLayoutGroup playerHealthBar;

    void Awake()
    {
        playerHealthController.maxValue = Player.gameObject.GetComponent<Player>().maxHP / 100.0f;
        playerHealthController.minValue = 0.0f;
        playerHealthController.value = Player.gameObject.GetComponent<Player>().curHP / 100.0f;

        

        /*bossHealthController.maxValue = Boss.gameObject.GetComponent<Boss>().MaxHP / 100.0f;
        bossHealthController.minValue = 0.0f;
        bossHealthController.value = Boss.gameObject.GetComponent<Boss>().CurrentHP / 100.0f;*/
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
        //bossHealthController.value = Boss.gameObject.GetComponent<Boss>().CurrentHP / 100.0f;
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
