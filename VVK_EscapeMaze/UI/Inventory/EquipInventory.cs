using UnityEngine;

public class EquipInventory : MonoBehaviour
{
    public GameObject WeaponSlotObj;
    public GameObject GearSlotObj;
    public GameObject ItemInventoryObj;

    GameObject ownerCharacterObj;
    PlayerInventory playerInventory;
    EquipSlot weaponSlot;
    EquipSlot gearSlot;

    public void InitEquipInventory(GameObject OwnerCharacter)
    {
        ownerCharacterObj = OwnerCharacter;
        if (OwnerCharacter)
        {
            playerInventory = OwnerCharacter.GetComponent<PlayerInventory>();
            playerInventory.OnEquipWeaponEvent += EquipWeapon;
            playerInventory.OnEquipGearEvent += EquipGear;
        }
        if (WeaponSlotObj && GearSlotObj)
        {
            weaponSlot = WeaponSlotObj.GetComponent<EquipSlot>();
            weaponSlot.InitializeEquipSlot();
            weaponSlot.OnReturnItemInventoryEvent += ReturnItemInventory;

            gearSlot = GearSlotObj.GetComponent<EquipSlot>();
            gearSlot.InitializeEquipSlot();
        }
    }

    public void EquipWeapon(PickUpItem weaponItem)
    {
        if (weaponItem && ownerCharacterObj)
        {
            weaponSlot.SetItemData(weaponItem);
        }
    }

    public void EquipGear(PickUpItem gearItem)
    {
        if (gearItem && ownerCharacterObj)
        {
            gearSlot.SetItemData(gearItem);
        }
    }

    void ReturnItemInventory(PickUpItem item)
    {
        playerInventory.AddItem(item);
    }
}
