using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    public Image ItemIcon;
    public Text ItemQuantity;
    public GameObject DropBox;

    Button ItemButton;
    PickUpItem ItemData;
    DropDownMenu DropDownUI;
    GameObject currentDropBoxUI;

    public int SlotIndex { get; private set; }

    public delegate void OnDestroyItemSlotDelegate(int itemSlot);
    public event OnDestroyItemSlotDelegate OnDestroyItemEvent;

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape) && currentDropBoxUI)
        {
            Destroy(currentDropBoxUI);
        }
    }

    public void InitializeItem(int itemSlotIndex)
    {
        SlotIndex = itemSlotIndex;
        ItemButton = GetComponent<Button>();

        ItemIcon.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        ItemQuantity.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
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
        if (ItemData && DropBox)
        {
            foreach(Transform child in transform.parent.parent)
            {
                if(child.name == "DropDown(Clone)")
                {
                    DropDownUI.OnDestroyItemEvent -= RemoveItem;
                    Destroy(child.gameObject);
                }
            }
            GameObject DropBoxUI = Instantiate(DropBox, transform.parent.parent);
            currentDropBoxUI = DropBoxUI;
            DropBoxUI.transform.position = Input.mousePosition + new Vector3(200.0f, -50.0f, 0.0f);
            DropDownUI = DropBoxUI.GetComponent<DropDownMenu>();
            DropDownUI.InitializeDropDownMenu(ItemData, this);
            DropDownUI.OnDestroyItemEvent += RemoveItem;
        }
    }

    void RemoveItem(int currentIndex)
    {
        ItemIcon.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        ItemQuantity.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        ItemQuantity.text = "";
        OnDestroyItemEvent?.Invoke(SlotIndex);
    }
}
