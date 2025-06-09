using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public GameObject PoolObject;

    public int InventorySize { get; private set; }
    public List<GameObject> ItemList { get; private set; }

    // Delegate for Refreshing Inventory
    public delegate void RefreshInventoryDelegate();
    public event RefreshInventoryDelegate OnRefreshInventory;

    PickupItemPool itemPool;

    void Awake()
    {
        InitializeInventory();
    }

    void Update()
    {
    }

    void InitializeInventory()
    {
        InventorySize = 10;
        ItemList = new List<GameObject>();
        if (PoolObject)
        {
            itemPool = PoolObject.GetComponent<PickupItemPool>();
        }
    }

    void AddItem(GameObject ItemObject)
    {
        if (ItemList.Count < InventorySize)
        {
            PickUpItem NewItem = ItemObject.GetComponent<PickUpItem>();
            int RemainingQuantity = NewItem.itemQuantity;

            foreach (GameObject Item in ItemList)
            {
                PickUpItem ListItem = Item.GetComponent<PickUpItem>();
                if (ListItem.itemID == NewItem.itemID && ListItem.itemQuantity < ListItem.itemMaxQuantity)
                {
                    ListItem.SetItemQuantity(RemainingQuantity, out RemainingQuantity);

                    if (RemainingQuantity <= 0)
                    {
                        break;
                    }
                }
            }

            while (RemainingQuantity > 0 && CheckInventorySpace())
            {
                int QuantityforAdd = Mathf.Min(RemainingQuantity, NewItem.itemMaxQuantity);
                GameObject NewItemObj = itemPool.UseItemPool();

                if (NewItemObj)
                {
                    PickUpItem NewPickUpItem = NewItemObj.GetComponent<PickUpItem>();
                    NewPickUpItem.InitializePickUpItem(NewItem);
                    NewPickUpItem.ModifyItemQuantity(QuantityforAdd);
                    ItemList.Add(NewItemObj);
                }

                RemainingQuantity -= QuantityforAdd;
            }

            OnRefreshInventory?.Invoke();
            itemPool.ReturnItemPool(ItemObject);
        }
    }

    bool CheckInventorySpace()
    {
        if (ItemList.Count < InventorySize)
        {
            return true;
        }
        else
        {
            Debug.Log("Inventory is Full");
            return false;
        }
    }

    public void CheckItem(GameObject item)
    {
        if (item)
        {
            AddItem(item);
        }
    }
}
