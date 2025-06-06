using System.Collections.Generic;
using UnityEngine;

public class InventoryPanel : MonoBehaviour
{
    public GameObject PanelItem;
    public GameObject PlayerCharacter;


    PlayerInventory InventoryCom;

    List<GameObject> itemList = new List<GameObject>();
    List<ItemSlot> itemSlotList = new List<ItemSlot>();

    void Awake()
    {
        
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void InitPanel()
    {
        InventoryCom = PlayerCharacter.GetComponent<PlayerInventory>();
        InventoryCom.OnRefreshInventory += RefreshGrid;
        Debug.Log("Init");
        RefreshGrid();
    }

    void RefreshGrid()
    {
        Debug.Log("Refresh Inventory");
        for (int i = 0; i < 10; i++)
        {
            GameObject item = Instantiate(PanelItem, transform);
            ItemSlot InventoryItem = item.GetComponent<ItemSlot>();
            itemList.Add(item);
            itemSlotList.Add(InventoryItem);
        }
        
    }
}
