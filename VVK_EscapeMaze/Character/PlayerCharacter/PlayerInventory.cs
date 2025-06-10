using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public GameObject PoolObject;

    public int InventorySize { get; private set; }
    public List<PickUpItem> ItemList { get; private set; }

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
        ItemList = new List<PickUpItem>();
        if (PoolObject)
        {
            itemPool = PoolObject.GetComponent<PickupItemPool>();
        }
    }

    void AddItem(PickUpItem NewItem)
    {
        if (ItemList.Count < InventorySize)
        {
            int RemainingQuantity = 0;
            foreach (PickUpItem ListItem in ItemList)
            {
                if (ListItem.itemID == NewItem.itemID && ListItem.itemQuantity < ListItem.itemMaxQuantity)
                {
                    ListItem.SetItemQuantity(NewItem.itemQuantity, out RemainingQuantity);

                    if (RemainingQuantity <= 0)
                    {
                        break;
                    }
                }
            }

            while (RemainingQuantity > 0 && CheckInventorySpace())
            {
                int QuantityforAdd = Mathf.Min(RemainingQuantity, NewItem.itemMaxQuantity);
                GameObject CopyItemObj = itemPool.UseItemPool();
                if (CopyItemObj)
                {
                    PickUpItem CopyPickUpItem = CopyItemObj.GetComponent<PickUpItem>();
                    CopyPickUpItem.InitializePickUpItem(NewItem);
                    CopyPickUpItem.ModifyItemQuantity(QuantityforAdd);
                    ItemList.Add(CopyPickUpItem);
                }

                RemainingQuantity -= QuantityforAdd;
                itemPool.ReturnItemPool(CopyItemObj);
            }

            OnRefreshInventory?.Invoke();
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
            PickUpItem itemData = item.GetComponent<PickUpItem>();
            AddItem(itemData);
            itemPool.ReturnItemPool(item);
        }
    }
}
