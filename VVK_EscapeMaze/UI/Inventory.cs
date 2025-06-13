using UnityEngine;

public class Inventory : MonoBehaviour
{
    public GameObject ItemInventoryObj;
    public GameObject EquipInventoryObj;


    public void InitializeInventory(GameObject OwnerCharacter)
    {
        if (ItemInventoryObj)
        {
            ItemInventoryObj.GetComponent<ItemInventory>().InitPanel(OwnerCharacter);
        }
        if (EquipInventoryObj)
        {
            EquipInventoryObj.GetComponent<EquipInventory>().InitEquipInventory(OwnerCharacter);
        }
    }
}
