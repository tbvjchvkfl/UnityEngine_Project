using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class InventoryPanel : MonoBehaviour
{
    public GameObject PanelItem;
    public int InventorySize;

    PlayerInventory InventoryCom;
    List<GameObject> itemList = new List<GameObject>();

    
    public void InitPanel(PlayerInventory InventoryComponent)
    {
        itemList.Clear();

        InventoryCom = InventoryComponent;
        InventoryCom.OnRefreshInventory += RefreshGrid;

        for (int i = 0; i < InventorySize; i++)
        {
            GameObject item = Instantiate(PanelItem, transform);
            ItemSlot itemSlot = item.GetComponent<ItemSlot>();
            itemSlot.InitializeItem(i);
            itemSlot.OnDestroyItemEvent += RemoveAtListItem;
            itemList.Add(item);
        }
        RefreshGrid();
    }

    void RefreshGrid()
    {
        for (int i = 0; i < InventoryCom.ItemList.Count; i++)
        {
            itemList[i].GetComponent<ItemSlot>().SetitemData(InventoryCom.ItemList[i].GetComponent<PickUpItem>());
        }
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
                itemSlot.OnDestroyItemEvent -= RemoveAtListItem;
            }
            Destroy(item);
        }
    }
}
