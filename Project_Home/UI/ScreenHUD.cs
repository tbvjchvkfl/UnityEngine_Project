using UnityEngine;
using UnityEngine.UI;
using static Unity.VisualScripting.Icons;

public class ScreenHUD : MonoBehaviour
{
    [Header("GameObject")]
    public GameObject Boss;

    [Header("Script")]
    public HPGuage FirstHealthGuage;
    public HPGuage MiddleHealthGuage;
    public HPGuage LastHealthGuage;

    [Header("UI Component")]
    public Slider bossFirstHPController;
    public Slider bossMiddleHPController;
    public Slider bossLastHPController;

    // Boss
    bool FirstGuage;
    bool MiddleGuage;
    bool LastGuage;

    void Awake()
    {
        bossFirstHPController.maxValue = Boss.gameObject.GetComponent<BossCharacter>().FirstPhaseMaxHP / Boss.gameObject.GetComponent<BossCharacter>().FirstPhaseMaxHP;
        bossMiddleHPController.maxValue = Boss.gameObject.GetComponent<BossCharacter>().SecondPhaseMaxHP / Boss.gameObject.GetComponent<BossCharacter>().SecondPhaseMaxHP;
        bossLastHPController.maxValue = Boss.gameObject.GetComponent<BossCharacter>().ThirdPhaseMaxHP / Boss.gameObject.GetComponent<BossCharacter>().ThirdPhaseMaxHP;
        bossFirstHPController.minValue = 0.0f;
        bossMiddleHPController.minValue = 0.0f;
        bossLastHPController.minValue = 0.0f;
        bossFirstHPController.value = bossFirstHPController.maxValue;
        bossMiddleHPController.value = bossMiddleHPController.maxValue;
        bossLastHPController.value = bossLastHPController.maxValue;
    }

    void Update()
    {
        if (Boss)
        {
            FirstGuage = Boss.gameObject.GetComponent<BossCharacter>().bIsFirstPhase;
            MiddleGuage = Boss.gameObject.GetComponent<BossCharacter>().bIsMiddlePhase;
            LastGuage = Boss.gameObject.GetComponent<BossCharacter>().bIsLastPhase;
            ControllBossHealthGuage();
        }
    }

    void ControllBossHealthGuage()
    {
        if (FirstGuage)
        {
            bossFirstHPController.value = Boss.gameObject.GetComponent<BossCharacter>().CurrentHP / Boss.gameObject.GetComponent<BossCharacter>().FirstPhaseMaxHP;
        }
        if (MiddleGuage)
        {
            bossFirstHPController.value = 0.0f;
            bossMiddleHPController.value = Boss.gameObject.GetComponent<BossCharacter>().CurrentHP / Boss.gameObject.GetComponent<BossCharacter>().SecondPhaseMaxHP;
            FirstHealthGuage.StopRotation = true;
        }
        if (LastGuage)
        {
            bossMiddleHPController.value = 0.0f;
            bossLastHPController.value = Boss.gameObject.GetComponent<BossCharacter>().CurrentHP / Boss.gameObject.GetComponent<BossCharacter>().ThirdPhaseMaxHP;
            MiddleHealthGuage.StopRotation = true;
        }
    }
}
