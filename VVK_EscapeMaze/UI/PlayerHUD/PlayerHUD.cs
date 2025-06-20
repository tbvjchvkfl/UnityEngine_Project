using UnityEngine;

public class PlayerHUD : MonoBehaviour
{
    [Header("UI Component")]
    public GameObject PlayerHealthBarUIObj;
    public GameObject PlayerSkillPointUIObj;
    public GameObject PlayerEquipSkill;

    SkillPoint_UI sp_UI;
    HPBar_UI hp_UI;

    public void InitializePlayerHUD(GameObject owner)
    {
        if (owner)
        {
            if (PlayerHealthBarUIObj)
            {
                hp_UI = PlayerHealthBarUIObj.GetComponent<HPBar_UI>();
                hp_UI.InitHPBar(owner);
            }
            if (PlayerSkillPointUIObj)
            {
                sp_UI = PlayerSkillPointUIObj.GetComponent<SkillPoint_UI>();
                sp_UI.InitSkillPointTray(owner);
            }
        }
    }
}
