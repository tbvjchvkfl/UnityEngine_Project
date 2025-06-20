using NUnit.Framework.Internal;
using UnityEngine;
using UnityEngine.UI;

public class EquipSlot : MonoBehaviour
{
    public Image SlotIcon;
    public Text SlotItemName;
    public Text SlotItemRate;

    PlayerCharacter playerCharacter;

    PickUpItem currentEquipItem;

    public delegate void OnReturnItemInventoryDelegate(PickUpItem item);
    public event OnReturnItemInventoryDelegate OnReturnItemInventoryEvent;

    public void InitializeEquipSlot()
    {
        currentEquipItem = null;
        if (SlotIcon)
        {
            SlotIcon.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        }
    }

    public void SetItemData(PickUpItem itemData)
    {
        if (currentEquipItem)
        {
            currentEquipItem.ModifyItemQuantity(1);
            OnReturnItemInventoryEvent?.Invoke(currentEquipItem);
        }
        if (itemData && SlotIcon)
        {
            currentEquipItem = itemData;
            SlotIcon.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            SlotIcon.sprite = itemData.itemIcon;
            SlotItemName.text = itemData.itemName;
            SlotItemRate.text = $"{itemData.equipItemRate}";
        }
    }
}
