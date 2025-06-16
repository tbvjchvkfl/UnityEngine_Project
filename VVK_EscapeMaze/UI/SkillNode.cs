using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillNode : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public Slider ActiveSlider;
    public Image SkillIcon;
    public Text NeedSkillPointText;
    public SkillBase SkillData;
    public GameObject VisibiltiyObject;

    Button skillActiveButton;
    CanvasGroup canvasGroup;

    PlayerCharacter playerCharacter;
    public SkillBase skillInfo {  get; private set; }
    GameObject visibilityObj;

    Coroutine SliderCoroutine;

    bool bIsButtonPushed = false;
    

    public void InitNodeData(GameObject player)
    {
        skillActiveButton = GetComponent<Button>();
        canvasGroup = GetComponent<CanvasGroup>();
        SliderCoroutine = null;

        if (player)
        {
            playerCharacter = player.GetComponent<PlayerCharacter>();
        }

        if (SkillData)
        {
            skillInfo = SkillData.CopyData();

            ActiveSlider.minValue = 0.0f;
            ActiveSlider.maxValue = 2.0f;
            SkillIcon.sprite = skillInfo.SkillIcon;
            NeedSkillPointText.text = $"TP : {skillInfo.NeedTechnicalPoint}";
        }
    }

    /*void CopySkillData(SkillBase Data)
    {
        if (Data)
        {
            skillInfo = Data.CopyData();
        }
    }*/

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

    public void OnDrag(PointerEventData eventData)
    {
        if (visibilityObj)
        {
            visibilityObj.transform.position = Input.mousePosition + new Vector3(50.0f, -50.0f, 0.0f);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (VisibiltiyObject && !visibilityObj)
        {
            visibilityObj = Instantiate(VisibiltiyObject, transform.parent.parent.parent.parent);
            visibilityObj.transform.position = Input.mousePosition + new Vector3(50.0f, -50.0f, 0.0f);
            visibilityObj.GetComponent<Visibility>().InitVisibility(SkillIcon.sprite);
            canvasGroup.blocksRaycasts = false;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        bIsButtonPushed = false;
        canvasGroup.blocksRaycasts = true;

        if (SliderCoroutine != null)
        {
            StopCoroutine(SliderCoroutine);
            SliderCoroutine = null;
            ActiveSlider.value = 0.0f;
        }

        if (visibilityObj)
        {
            Destroy(visibilityObj);
            visibilityObj = null;
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
                    playerCharacter.ApplyCharacterStat(skillInfo.IncreaseAttackRate, skillInfo.IncreaseArmorRate, skillInfo.IncreaseAimRate);
                }
            }
            yield return new WaitForSeconds(0.1f);
        }
        SliderCoroutine = null;
    }
}
