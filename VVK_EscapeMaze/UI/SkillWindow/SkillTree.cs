using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[System.Serializable]
public class  NodeDataClass
{
    GameObject SkillNode;
    SkillBase SkillInfo;
}

public class SkillTree : MonoBehaviour
{
    public Text TechnicalPointText;
    public Text SkillNameText;
    public Text SkillDescriptionText;
    public GameObject ActionSkillSlot;
    public GameObject SpedcialSkillSlot;

    public GameObject[] SkillNodeList = new GameObject[21];

    PlayerCharacter playerCharacter;

    SkillSlot actionSkill;
    SkillSlot specialSkill;

    void Update()
    {
        if (EventSystem.current.currentSelectedGameObject)
        {

        }
        else
        {
            SkillNameText.text = "None";
            SkillDescriptionText.text = "None";
        }
    }

    public void InitializeSkillTreeUI(GameObject owner)
    {
        if (owner)
        {
            playerCharacter = owner.GetComponent<PlayerCharacter>();
            TechnicalPointText.text = $"TP : {playerCharacter.technicalPoint}";

            if(ActionSkillSlot && SpedcialSkillSlot)
            {
                actionSkill = ActionSkillSlot.GetComponent<SkillSlot>();
                actionSkill.InitializeSkillSlot(owner);

                specialSkill = SpedcialSkillSlot.GetComponent<SkillSlot>();
                specialSkill.InitializeSkillSlot(owner);
            }

            foreach (GameObject skillNode in SkillNodeList)
            {
                skillNode.GetComponent<SkillNode>().InitNodeData(owner);
            }
        }
    }
}
