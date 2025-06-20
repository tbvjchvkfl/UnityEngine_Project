using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillNode : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public Image SkillIcon;
    public Image ActiveGuage;
    public Text NeedTechnicalPointText;
    public SkillBase SkillData;
    public GameObject VisibiltiyObject;

    CanvasGroup canvasGroup;

    PlayerCharacter playerCharacter;
    public SkillBase skillInfo {  get; private set; }
    GameObject visibilityObj;

    Coroutine SliderCoroutine;

    bool bIsButtonPushed = false;
    

    public void InitNodeData(GameObject player)
    {
        canvasGroup = GetComponent<CanvasGroup>();
        SliderCoroutine = null;

        if (player)
        {
            playerCharacter = player.GetComponent<PlayerCharacter>();
        }

        if (SkillData)
        {
            skillInfo = SkillData.CopyData();

            SkillIcon.sprite = skillInfo.SkillIcon;
            NeedTechnicalPointText.text = $"TP : {skillInfo.NeedTechnicalPoint}";
        }
        if (ActiveGuage)
        {
            ActiveGuage.fillAmount = 0.0f;
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
            NeedTechnicalPointText.color = Color.clear;
            if (SliderCoroutine == null)
            {
                SliderCoroutine = StartCoroutine(OnIncreaseSkillCover());
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        bIsButtonPushed = false;
        NeedTechnicalPointText.color = new Color(0.8f, 0.8f, 0.8f, 1.0f);
        if (SliderCoroutine != null)
        {
            StopCoroutine(SliderCoroutine);
            SliderCoroutine = null;
            ActiveGuage.fillAmount = 0.0f;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (visibilityObj && skillInfo.bIsActivity)
        {
            visibilityObj.transform.position = Input.mousePosition + new Vector3(50.0f, -50.0f, 0.0f);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (VisibiltiyObject && !visibilityObj && skillInfo.bIsActivity)
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
            ActiveGuage.fillAmount = 0.0f;
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
                ActiveGuage.fillAmount += Time.deltaTime;
                if (ActiveGuage.fillAmount >= 1.0f)
                {
                    ActiveGuage.fillAmount = 0.0f;
                    ActiveGuage.enabled = false;
                    skillInfo.bIsActivity = true;
                    GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                    SkillIcon.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                    playerCharacter.ApplyCharacterStat(skillInfo.NeedTechnicalPoint, skillInfo.IncreaseAttackRate, skillInfo.IncreaseArmorRate, skillInfo.IncreaseAimRate);
                }
            }
            yield return null;
        }
        SliderCoroutine = null;
    }
}
