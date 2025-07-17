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

    public GameObject[] SkillNodeList = new GameObject[21];
    public GameObject[] SkillSlotList = new GameObject[2];

    PlayerCharacter playerCharacter;
    SkillSlot leftClickSkill;
    SkillSlot rightClickSkill;

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

            leftClickSkill = SkillSlotList[0].GetComponent<SkillSlot>();
            leftClickSkill.InitializeSkillSlot(owner, 0);

            rightClickSkill = SkillSlotList[1].GetComponent<SkillSlot>();
            rightClickSkill.InitializeSkillSlot(owner, 1);

            foreach (GameObject skillNode in SkillNodeList)
            {
                skillNode.GetComponent<SkillNode>().InitNodeData(owner);
            }
        }
    }
}
