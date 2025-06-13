using UnityEngine;

public enum ItemType
{
    CONSUMABLE,
    WEAPON,
    GEAR,
    MATERIAL,
}

[System.Serializable]
public class  ItemBasicData
{
    public int itemID = 0;
    public string itemName = "Item Name";
    public string itemDescription = "Item Decription";
}

[System.Serializable]
public class ItemNumericData
{
    public int RecoverHealthPoint = 10;
    public int RecoverSkillPoint = 1;
    public int EquipItemRate = 0;
    public int itemQuantity = 1;
    public int itemMaxQuantity = 99;
}

[System.Serializable]
public class  ItemAssetData
{
    public Sprite itemIcon;
}

[System.Serializable]
public class  ItemPrefabData
{
    public GameObject ItemPrefab;
}

[CreateAssetMenu(fileName = "ItemBase", menuName = "Scriptable Objects/ItemBase")]
public class ItemBase : ScriptableObject
{
    public ItemType itemType;
    public ItemBasicData itemBasicData = new ItemBasicData();
    public ItemNumericData itemNumericData = new ItemNumericData();
    public ItemAssetData itemAssetData = new ItemAssetData();
    public ItemPrefabData itemPrefabData = new ItemPrefabData();
}
