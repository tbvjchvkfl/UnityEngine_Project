using UnityEngine;
using UnityEngine.UI;

public class Visibility : MonoBehaviour
{
    public Image SkillIcon;

    public void InitVisibility(Sprite Icon)
    {
        if (Icon)
        {
            SkillIcon.sprite = Icon;
        }
    }
}
