using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    [Header("UI Component")]
    public GameObject PlayerHealthBar;
    public GameObject PlayerSkillPoint;
    public GameObject PlayerEquipSkill;

    [Header("Owner Component")]
    public GameObject PlayerCharacter;


    public Text playerCurHP_Text;
    public Slider HPBar_Slider;

    SkillPoint_UI sp_UI;
    HPBar_UI hp_UI;


    float playerCurHP;
    float playerMaxHP;

    void Awake()
    {
        sp_UI = PlayerSkillPoint.GetComponent<SkillPoint_UI>();
        hp_UI = PlayerHealthBar.GetComponent<HPBar_UI>();
    }

    void Start()
    {
        sp_UI.InitSkillPointTray(PlayerCharacter);
        hp_UI.InitHPBar(PlayerCharacter);
    }

    void Update()
    {
        
    }
}
