using NUnit.Framework.Interfaces;
using UnityEngine;

public class PickUpItem : MonoBehaviour
{
    public ItemType itemType { get; private set; }
    public int itemID { get; private set; }
    public Sprite itemIcon { get; private set; }
    public string itemName { get; private set; }
    public string itemDescription { get; private set; }
    public int itemQuantity { get; private set; }
    public int itemMaxQuantity { get; private set; }

    // New Item Ver
    public void InitializePickUpItem(ItemBase ItemData)
    {
        itemType = ItemData.itemType;

        itemID = ItemData.itemBasicData.itemID;
        itemName = ItemData.itemBasicData.itemName;
        itemDescription = ItemData.itemBasicData.itemDescription;

        itemQuantity = ItemData.itemNumericData.itemQuantity;
        itemMaxQuantity = ItemData.itemNumericData.itemMaxQuantity;

        itemIcon = ItemData.itemAssetData.itemIcon;


        if (ItemData.itemPrefabData.ItemPrefab)
        {
            MeshFilter sourceMeshFilter = ItemData.itemPrefabData.ItemPrefab.GetComponent<MeshFilter>();
            MeshRenderer sourceMeshRenderer = ItemData.itemPrefabData.ItemPrefab.GetComponent<MeshRenderer>();

            MeshFilter targetMeshFilter = GetComponent<MeshFilter>();
            MeshRenderer targetMeshRenderer = GetComponent<MeshRenderer>();

            if (sourceMeshFilter != null && targetMeshFilter != null)
            {
                targetMeshFilter.mesh = sourceMeshFilter.sharedMesh;
            }

            if (sourceMeshRenderer != null && targetMeshRenderer != null)
            {
                targetMeshRenderer.materials = sourceMeshRenderer.sharedMaterials;
            }
        }
    }

    // Copy Item Ver
    public void InitializePickUpItem(PickUpItem CopyItem)
    {
        itemType = CopyItem.itemType;

        itemID = CopyItem.itemID;
        itemName = CopyItem.itemName;
        itemDescription = CopyItem.itemDescription;

        itemQuantity = CopyItem.itemQuantity;
        itemMaxQuantity = CopyItem.itemMaxQuantity;

        itemIcon = CopyItem.itemIcon;

        MeshFilter sourceMeshFilter = CopyItem.GetComponent<MeshFilter>();
        MeshRenderer sourceMeshRenderer = CopyItem.GetComponent<MeshRenderer>();

        MeshFilter targetMeshFilter = GetComponent<MeshFilter>();
        MeshRenderer targetMeshRenderer = GetComponent<MeshRenderer>();

        if (sourceMeshFilter != null && targetMeshFilter != null)
        {
            targetMeshFilter.mesh = sourceMeshFilter.sharedMesh;
        }

        if (sourceMeshRenderer != null && targetMeshRenderer != null)
        {
            targetMeshRenderer.materials = sourceMeshRenderer.sharedMaterials;
        }
    }

    public void SetItemQuantity(int newQuantity, out int RemainQuantity)
    {
        if (itemQuantity + newQuantity > itemMaxQuantity)
        {
            RemainQuantity = (itemQuantity + newQuantity) - itemMaxQuantity;
        }
        else
        {
            RemainQuantity = 0;
        }
        itemQuantity = Mathf.Clamp(itemQuantity + newQuantity, 0, itemMaxQuantity);
    }

    public void ModifyItemQuantity(int newQuantity)
    {
        itemQuantity = newQuantity;
    }
}
