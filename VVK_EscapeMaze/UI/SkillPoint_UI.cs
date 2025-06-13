using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillPoint_UI : MonoBehaviour
{
    [Header("Skill Point UI Component")]
    public GameObject SP_Horizon;
    public GameObject SP_Item;
    public Slider SP_Slider;

    PlayerCharacter playerCharacter;

    public List<GameObject> SP_Inventory { get; private set; } = new List<GameObject>();
    public int SkillPoint { get; private set; }


    public void InitSkillPointTray(GameObject player)
    {
        InitProperty();

        if (player)
        {
            playerCharacter = player.GetComponent<PlayerCharacter>();

            for (int i = 0; i < 13; i++)
            {
                GameObject item = Instantiate(SP_Item, SP_Horizon.transform);
                item.GetComponent<SkillPointItem_UI>().InitSkillPointItem(i);
                SP_Inventory.Add(item);
            }
            StartCoroutine(OnChargeSkillPointGuage());
        }
    }

    void InitProperty()
    {
        SP_Slider.value = 0.0f;
        SkillPoint = 0;
    }

    public IEnumerator OnChargeSkillPointGuage()
    {
        while (!playerCharacter.bIsDead)
        {
            yield return new WaitForSeconds(0.1f);
            if (SP_Slider.value >= 1.0f)
            {
                if(SkillPoint >= playerCharacter.maxSkillPoint)
                {
                    yield return new WaitForSeconds(0.1f);
                }
                else
                {
                    SP_Slider.value = 0.0f;
                    foreach (GameObject item in SP_Inventory)
                    {
                        SkillPointItem_UI spItem = item.GetComponent<SkillPointItem_UI>();
                        if (!spItem.bIsActive)
                        {
                            spItem.RefreshItem(true);
                            SkillPoint++;
                            playerCharacter.SetSkillPoint(SkillPoint);
                            break;
                        }
                    }
                }
            }
            else
            {
                SP_Slider.value += Time.deltaTime;
            }
        }
    }
}
