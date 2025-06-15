using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillNode : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Slider ActiveSlider;
    public Image SkillIcon;
    public Text NeedSkillPointText;
    public SkillBase SkillData;

    Button skillActiveButton;
    PlayerCharacter playerCharacter;
    SkillBase skillInfo;
    Coroutine SliderCoroutine;
    bool bIsButtonPushed = false;


    public void InitNodeData(GameObject player)
    {
        skillActiveButton = GetComponent<Button>();
        SliderCoroutine = null;
        if (player)
        {
            playerCharacter = player.GetComponent<PlayerCharacter>();
        }
        if (SkillData)
        {
            skillInfo = SkillData;
            ActiveSlider.minValue = 0.0f;
            ActiveSlider.maxValue = 2.0f;
            SkillIcon.sprite = skillInfo.SkillIcon;
            NeedSkillPointText.text = $"TP : {skillInfo.NeedTechnicalPoint}";
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (playerCharacter.technicalPoint >= skillInfo.NeedTechnicalPoint)
        {
            bIsButtonPushed = true;
            if (SliderCoroutine == null)
            {
                SliderCoroutine = StartCoroutine(OnIncreaseSkillCover());
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        bIsButtonPushed = false;
        if (SliderCoroutine != null)
        {
            StopCoroutine(SliderCoroutine);
            SliderCoroutine = null;
            ActiveSlider.value = 0.0f;
        }
    }

    IEnumerator OnIncreaseSkillCover()
    {
        while (!skillInfo.bIsActivity)
        {
            if (bIsButtonPushed)
            {
                ActiveSlider.value += Time.deltaTime;
                if (ActiveSlider.value >= ActiveSlider.maxValue)
                {
                    ActiveSlider.value = ActiveSlider.minValue;
                    ActiveSlider.interactable = false;
                    skillInfo.bIsActivity = true;
                }
            }
            yield return new WaitForSeconds(0.1f);
        }
        SliderCoroutine = null;
    }
}
