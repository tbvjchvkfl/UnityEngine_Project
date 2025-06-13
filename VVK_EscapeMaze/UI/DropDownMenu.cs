using UnityEngine;
using UnityEngine.UI;

public class DropDownMenu : MonoBehaviour
{
    public Text ButtonAText;
    public Text ButtonBText;
    public GameObject ButtonA;

    PickUpItem currentItem;
    ItemSlot currentItemSlot;

    public int currentSlotIndex { get; private set; }

    public delegate void OnUseItemNoticeDelegate(int slotIndex);
    public event OnUseItemNoticeDelegate OnUseItemNoticeEvent;

    public delegate void OnDestroyItemNoticeDelegate(int slotIndex);
    public event OnDestroyItemNoticeDelegate OnDestroyItemNoticeEvent;

    public void InitializeDropDownMenu(PickUpItem itemData, ItemSlot slot)
    {
        currentItem = itemData;
        currentItemSlot = slot;
        currentSlotIndex = currentItemSlot.SlotIndex;
        if (currentItem)
        {
            if (!ButtonA.activeSelf)
            {
                ButtonA.SetActive(true);
            }
            if (currentItem.itemType == ItemType.CONSUMABLE)
            {
                ButtonAText.text = "Use";
                ButtonBText.text = "Destroy";
            }
            else if (currentItem.itemType == ItemType.WEAPON || currentItem.itemType == ItemType.GEAR)
            {
                ButtonAText.text = "Equip";
                ButtonBText.text = "Destroy";
            }
            else if (currentItem.itemType == ItemType.MATERIAL)
            {
                ButtonA.SetActive(false);
                ButtonBText.text = "Destroy";
            }
        }
    }

    public void ModifyUIData(PickUpItem itemData, ItemSlot slot)
    {
        currentItem = itemData;
        currentItemSlot = slot;
        if (currentItem)
        {
            if (!ButtonA.activeSelf)
            {
                ButtonA.SetActive(true);
            }
            if (currentItem.itemType == ItemType.CONSUMABLE)
            {
                ButtonAText.text = "Use";
                ButtonBText.text = "Destroy";
            }
            else if (currentItem.itemType == ItemType.WEAPON || currentItem.itemType == ItemType.GEAR)
            {
                ButtonAText.text = "Equip";
                ButtonBText.text = "Destroy";
            }
            else if (currentItem.itemType == ItemType.MATERIAL)
            {
                ButtonA.SetActive(false);
                ButtonBText.text = "Destroy";
            }
        }
    }

    public void OnClickedButtonA()
    {
        OnUseItemNoticeEvent?.Invoke(currentSlotIndex);
        Destroy(this.gameObject);
    }

    public void OnClickedButtonB()
    {
        OnDestroyItemNoticeEvent?.Invoke(currentSlotIndex);
        Destroy(this.gameObject);
    }
}
