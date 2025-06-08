using System.Collections.Generic;
using UnityEngine;

public class PickupItemPool : MonoBehaviour
{
    public GameObject SpawnItemDummy;
    public int PoolSize = 100;

    List<GameObject> ItemPoolList = new List<GameObject>();

    public bool bIsPoolReady { get; private set; }

    void Awake()
    {
        bIsPoolReady = false;
        if (SpawnItemDummy)
        {
            for (int i = 0; i < PoolSize; i++)
            {
                GameObject SpawnItem = Instantiate(SpawnItemDummy, transform);
                SpawnItem.SetActive(false);
                ItemPoolList.Add(SpawnItem);
            }
            bIsPoolReady = true;
        }
    }

    public GameObject UseItemPool()
    {
        if (ItemPoolList.Count > 0)
        {
            GameObject UsingItem = ItemPoolList[ItemPoolList.Count - 1];
            ItemPoolList.RemoveAt(ItemPoolList.Count - 1);
            UsingItem.SetActive(true);
            return UsingItem;
        }
        return null;
    }

    public void ReturnItemPool(GameObject ReturnItem)
    {
        if (ReturnItem)
        {
            ReturnItem.SetActive(false);
            ItemPoolList.Add(ReturnItem);
        }
    }
}
