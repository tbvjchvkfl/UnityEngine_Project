using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    public Image ItemIcon;
    public Text ItemQuantity;
    public GameObject DropBox;

    public PickUpItem ItemData { get; private set; }
    public Button ItemButton { get; private set; }

    public int SlotIndex { get; private set; }

    public delegate void OnUsingItemSlotDelegate(int slotIndex);
    public event OnUsingItemSlotDelegate OnUsingItemSlot;
    public delegate void OnDestroyItemSlotDelegate(int slotIndex);
    public event OnDestroyItemSlotDelegate OnDestroyItemEvent;

    public delegate void OnCreateDropDownMenuDelegate(ItemSlot slotInfo);
    public event OnCreateDropDownMenuDelegate OnCreateDropDownMenuEvent;

    

    public void InitializeItem(int itemSlotIndex)
    {
        ItemButton = GetComponent<Button>();
        SlotIndex = itemSlotIndex;

        ItemIcon.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        ItemQuantity.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);

        ItemData = null;
    }

    public void SetitemData(PickUpItem item)
    {
        ItemData = item;
        if (ItemData)
        {
            ItemIcon.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            ItemQuantity.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);

            ItemIcon.sprite = ItemData.itemIcon;
            ItemQuantity.text = $"{ItemData.itemQuantity}";
        }
    }

    public void OnClickedItemButton()
    {
        OnCreateDropDownMenuEvent.Invoke(this);
    }

    public void UseItem(int currentIndex)
    {
        OnUsingItemSlot?.Invoke(currentIndex);
    }

    public void RemoveItem(int currentIndex)
    {
        OnDestroyItemEvent?.Invoke(currentIndex);
    }
}
