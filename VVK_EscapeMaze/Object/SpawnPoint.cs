using System.Collections;
using UnityEngine;

public enum SpawnType
{
    Character,
    Item
}

public class SpawnPoint : MonoBehaviour
{
    public SpawnType SpawnType;
    public ItemBase ItemData;
    public GameObject ItemPoolObj;

    void Awake()
    {

    }

    void Start()
    {
        if(SpawnType == SpawnType.Item)
        {
            if (ItemPoolObj)
            {
                StartCoroutine(SpawnItem(ItemPoolObj.GetComponent<PickupItemPool>()));
            }
        }
        else if (SpawnType == SpawnType.Character)
        {

        }
    }

    IEnumerator SpawnItem(PickupItemPool itemPool)
    {
        while (!itemPool.bIsPoolReady)
        {
            yield return null;
        }
        GameObject SpawnItem = itemPool.UseItemPool();
        if (SpawnItem)
        {
            SpawnItem.GetComponent<PickUpItem>().InitializePickUpItem(ItemData);
            SpawnItem.transform.position = transform.position;
        }
        else
        {
            Debug.Log("nullptr");
        }
        Destroy(gameObject);
    }
    
}
