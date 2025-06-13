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

    public delegate void EquipWeaponDelegate(PickUpItem weaponItem);
    public event EquipWeaponDelegate OnEquipWeaponEvent;
    public delegate void EquipGearDelegate(PickUpItem gearItem);
    public event EquipGearDelegate OnEquipGearEvent;

    PickupItemPool itemPool;

    void Awake()
    {
        InitializeInventory();
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

    public void AddItem(PickUpItem NewItem)
    {
        if (ItemList.Count < InventorySize)
        {
            int RemainingQuantity = NewItem.itemQuantity;
            foreach (PickUpItem ListItem in ItemList)
            {
                if (ListItem.itemType == NewItem.itemType && ListItem.itemID == NewItem.itemID && ListItem.itemQuantity < ListItem.itemMaxQuantity)
                {
                    RemainingQuantity = ListItem.SetItemQuantity(NewItem.itemQuantity);

                    if (RemainingQuantity <= 0)
                    {
                        break;
                    }
                }
            }

            if (RemainingQuantity > 0 && CheckInventorySpace())
            {
                NewItem.ModifyItemQuantity(RemainingQuantity);
                ItemList.Add(NewItem);
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

    public void UseItem(int listIndex)
    {
        int RemainQuantity = 0;
        ItemList[listIndex].Use(out RemainQuantity);

        switch (ItemList[listIndex].itemType)
        {
            case ItemType.CONSUMABLE:
                UseConsumableItem(ItemList[listIndex]);
                break;
            case ItemType.WEAPON:
                UseWeaponItem(ItemList[listIndex]);
                break;
            case ItemType.GEAR:
                UseGearItem(ItemList[listIndex]);
                break;
            default:
                Debug.Log("Unknown Item Type");
                break;
        }

        if (RemainQuantity <= 0)
        {
            ItemList.RemoveAt(listIndex);
        }
    }

    void UseConsumableItem(PickUpItem UseItem)
    {
        if (UseItem)
        {
            gameObject.GetComponent<PlayerCharacter>().ApplyItemEffect(UseItem.recoverHealthPoint, UseItem.recoverSkillPoint);
        }
    }

    void UseWeaponItem(PickUpItem UseItem)
    {
        OnEquipWeaponEvent?.Invoke(UseItem);
    }

    void UseGearItem(PickUpItem UseItem)
    {
        OnEquipGearEvent?.Invoke(UseItem);
    }
}