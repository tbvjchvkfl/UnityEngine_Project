using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillSlot : MonoBehaviour, IDropHandler
{
    public Image EquipSkillIcon;

    PlayerCharacter playerCharacter;
    SkillNode skillNode;

    public void InitializeSkillSlot(GameObject owner)
    {
        if (owner)
        {
            playerCharacter = owner.GetComponent<PlayerCharacter>();
            EquipSkillIcon.color = Color.clear;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject pointerObj = eventData.pointerDrag;
        if (pointerObj)
        {
            skillNode = pointerObj.GetComponent<SkillNode>();

            if (skillNode.skillInfo.SkillID == 1 && skillNode.skillInfo.bIsActivity)
            {
                EquipSkillIcon.sprite = skillNode.SkillIcon.sprite;
                EquipSkillIcon.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            }
        }
    }

    public void EquipSkill()
    {

    }
}
