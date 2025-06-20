using System.Collections.Generic;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemInventory : MonoBehaviour
{
    public GameObject ItemPanel;
    public GameObject ItemSlotObject;
    public GameObject EquipInventoryObject;
    public GameObject DropDownMenuObject;
    public Text ItemNameText;
    public Text ItemDescriptionText;
    public int InventorySize;

    PlayerCharacter playerCharacter;
    PlayerInventory InventoryCom;
    EquipInventory equipInventory;

    GameObject dropDownUIObject;
    DropDownMenu dropDownMenu;

    List<GameObject> itemList = new List<GameObject>();

    void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == ItemSlotObject && EventSystem.current.currentSelectedGameObject.GetComponent<ItemSlot>().ItemData)
        {
            ItemNameText.text = $"{EventSystem.current.currentSelectedGameObject.GetComponent<ItemSlot>().ItemData.itemName}";
            ItemDescriptionText.text = $"{EventSystem.current.currentSelectedGameObject.GetComponent<ItemSlot>().ItemData.itemDescription}";
        }
        else
        {
            ItemNameText.text = "None";
            ItemDescriptionText.text = "None";
        }

        if (Input.GetKey(KeyCode.Escape) && dropDownUIObject)
        {
            Destroy(dropDownUIObject);
            dropDownUIObject = null;
        }
    }

    public void InitPanel(GameObject OwnerCharacter)
    {
        itemList.Clear();

        if (OwnerCharacter)
        {
            playerCharacter = OwnerCharacter.GetComponent<PlayerCharacter>();
            InventoryCom = OwnerCharacter.GetComponent<PlayerInventory>();
            InventoryCom.OnRefreshInventory += RefreshGrid;
        }
        if (EquipInventoryObject)
        {
            equipInventory = EquipInventoryObject.GetComponent<EquipInventory>();
        }

        for (int i = 0; i < InventorySize; i++)
        {
            GameObject SlotObject = Instantiate(ItemSlotObject, ItemPanel.transform);
            ItemSlot itemSlot = SlotObject.GetComponent<ItemSlot>();
            itemSlot.InitializeItem(i);
            itemSlot.OnCreateDropDownMenuEvent += CreateDropDownMenu;
            itemSlot.OnUsingItemSlot += UseAtListItem;
            itemSlot.OnDestroyItemEvent += RemoveAtListItem;
            itemList.Add(SlotObject);
        }
        RefreshGrid();

        EventSystem.current.SetSelectedGameObject(itemList[0].GetComponent<ItemSlot>().ItemButton.gameObject);
    }

    void RefreshGrid()
    {
        for (int i = 0; i < itemList.Count; i++)
        {
            itemList[i].GetComponent<ItemSlot>().InitializeItem(i);
        }
        for (int i = 0; i < InventoryCom.ItemList.Count; i++)
        {
            itemList[i].GetComponent<ItemSlot>().SetitemData(InventoryCom.ItemList[i]);
        }
    }

    void UseAtListItem(int currentSlotIndex)
    {
        InventoryCom.UseItem(currentSlotIndex);
        RefreshGrid();
    }

    void RemoveAtListItem(int currentSlotIndex)
    {
        InventoryCom.ItemList.RemoveAt(currentSlotIndex);

        for (int i = 0; i < itemList.Count; i++)
        {
            itemList[i].GetComponent<ItemSlot>().InitializeItem(i);
        }
        RefreshGrid();
    }

    void CreateDropDownMenu(ItemSlot Slot)
    {
        if (Slot && Slot.ItemData)
        {
            if (!dropDownUIObject)
            {
                dropDownUIObject = Instantiate(DropDownMenuObject, transform);
            }
            dropDownUIObject.transform.position = Input.mousePosition + new Vector3(200.0f, -50.0f, 0.0f);
            dropDownMenu = dropDownUIObject.GetComponent<DropDownMenu>();
            dropDownMenu.InitializeDropDownMenu(Slot.ItemData, Slot);
            dropDownMenu.OnUseItemNoticeEvent += Slot.UseItem;
            dropDownMenu.OnDestroyItemNoticeEvent += Slot.RemoveItem;
        }
        else
        {
            if (dropDownUIObject)
            {
                dropDownMenu.OnUseItemNoticeEvent -= Slot.UseItem;
                dropDownMenu.OnDestroyItemNoticeEvent -= Slot.RemoveItem;
                Destroy(dropDownUIObject);
                dropDownUIObject = null;
            }
        }
    }

    void OnDestroy()
    {
        if (InventoryCom)
        {
            InventoryCom.OnRefreshInventory -= RefreshGrid;
        }
        foreach (GameObject item in itemList)
        {
            ItemSlot itemSlot = item.GetComponent<ItemSlot>();
            if (itemSlot)
            {
                itemSlot.OnCreateDropDownMenuEvent -= CreateDropDownMenu;
                itemSlot.OnUsingItemSlot -= UseAtListItem;
                itemSlot.OnDestroyItemEvent -= RemoveAtListItem;
            }
            Destroy(item);
        }
    }
}

