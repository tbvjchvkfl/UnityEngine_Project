using System.Collections;
using System.Collections.Generic;
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

    public SkillBase skillInfo { get; private set; }
    public List<int> NextSkillNodeIndexList = new List<int>();

    CanvasGroup canvasGroup;
    PlayerCharacter playerCharacter;
    SkillTree skillTree;
    GameObject visibilityObj;
    Coroutine SliderCoroutine;

    bool bIsButtonPushed = false;
    bool bIsEnabledNextSkillLearn = false;
    int skillNodeIndex = 0;

    public void InitNodeData(GameObject player, SkillTree tree, int nodeIndex)
    {
        canvasGroup = GetComponent<CanvasGroup>();
        SliderCoroutine = null;
        skillTree = tree;
        skillNodeIndex = nodeIndex;

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

    public void OnPointerDown(PointerEventData eventData)
    {
        if (playerCharacter.technicalPoint >= skillInfo.NeedTechnicalPoint)
        {
            if (skillNodeIndex == 0)
            {
                PressedSkillButton();
            }
            else
            {
                if (bIsEnabledNextSkillLearn)
                {
                    PressedSkillButton();
                }
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

    void PressedSkillButton()
    {
        bIsButtonPushed = true;
        NeedTechnicalPointText.color = Color.clear;
        if (SliderCoroutine == null)
        {
            SliderCoroutine = StartCoroutine(OnIncreaseSkillCover());
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
                    foreach (int index in NextSkillNodeIndexList)
                    {
                        skillTree.SkillNodeList[index].GetComponent<SkillNode>().bIsEnabledNextSkillLearn = true;
                    }
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
