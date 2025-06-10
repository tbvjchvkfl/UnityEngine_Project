using UnityEngine;
using UnityEngine.UI;

public class DropDownMenu : MonoBehaviour
{
    public Text ButtonAText;
    public Text ButtonBText;
    public GameObject ButtonA;
    public GameObject PlayerCharacter;

    PickUpItem currentItem;
    ItemSlot currentItemSlot;

    public int currentSlotIndex { get; private set; }

    public delegate void OnDestroyItemDelegate(int slotIndex);
    public event OnDestroyItemDelegate OnDestroyItemEvent;

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
            else if (currentItem.itemType == ItemType.EQUIPMENT)
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
            else if (currentItem.itemType == ItemType.EQUIPMENT)
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
        if (currentItem.itemType == ItemType.CONSUMABLE)
        {
            ButtonAText.text = "Use";
            ButtonBText.text = "Destroy";
        }
        else if (currentItem.itemType == ItemType.EQUIPMENT)
        {
            ButtonAText.text = "Equip";
            ButtonBText.text = "Destroy";
        }
        Destroy(this.gameObject);
    }

    public void OnClickedButtonB()
    {
        OnDestroyItemEvent?.Invoke(currentSlotIndex);

        Destroy(this.gameObject);
    }
}
