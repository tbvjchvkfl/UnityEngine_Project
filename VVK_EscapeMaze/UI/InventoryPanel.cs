using System.Collections.Generic;
using UnityEngine;

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
            item.GetComponent<ItemSlot>().InitializeItem();
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
}
