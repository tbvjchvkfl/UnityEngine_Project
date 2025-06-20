using UnityEngine;
using UnityEngine.UI;

public class SkillPointItem_UI : MonoBehaviour
{
    [Header("Skill Point Item UI Component")]
    public Image SPImage;


    public int SPItemIndex { get; private set; }
    
    public bool bIsActive { get; private set; }


    public void InitSkillPointItem(int index)
    {
        SPItemIndex = index;
        bIsActive = false;
        SPImage.enabled = false;
    }

    public void RefreshItem(bool ItemActive)
    {
        bIsActive = ItemActive;
        if (bIsActive)
        {
            SPImage.enabled = true;
        }
    }
}
