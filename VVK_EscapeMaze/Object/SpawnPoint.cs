using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public enum SpawnType
{
    Character,
    Item
}

public class SpawnPoint : MonoBehaviour
{
    public SpawnType SpawnType;
    public ItemBase ItemData;
    public EnemyBase EnemyData;
    public GameObject ItemPoolObj;
    public GameObject EnemyPoolObj;
    public bool bIsLinkingTrigger = false;
    public bool bIsDelayEnd { get; set; } = false;


    void Awake()
    {

    }

    void Start()
    {
        if (!bIsLinkingTrigger)
        {
            ObjectSpawn();
        }
        else
        {
            StartCoroutine(SpawnDelay());
        }
    }

    void ObjectSpawn()
    {
        if (SpawnType == SpawnType.Item)
        {
            if (ItemPoolObj)
            {
                StartCoroutine(SpawnItem(ItemPoolObj.GetComponent<PickupItemPool>()));
            }
        }
        else if (SpawnType == SpawnType.Character)
        {
            if (EnemyPoolObj)
            {
                StartCoroutine(SpawnEnemy(EnemyPoolObj.GetComponent<EnemyPool>()));
            }
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
            Debug.Log("Item nullptr");
        }
        Destroy(gameObject);
    }
    
    IEnumerator SpawnEnemy(EnemyPool enemyPool)
    {
        while (!enemyPool.bIsPoolReady)
        {
            yield return null;
        }
        GameObject SpawnEnemy = enemyPool.UseEnemyPool();
        if (SpawnEnemy)
        {
            SpawnEnemy.GetComponent<NavMeshAgent>().Warp(transform.position);
            SpawnEnemy.GetComponent<EnemyCharacter>().InitializedEnemyData(EnemyData);
        }
        else
        {
            Debug.Log("Enemy nullptr");
        }
        Destroy(gameObject);
    }

    IEnumerator SpawnDelay()
    {
        while (!bIsDelayEnd)
        {
            yield return null;
        }
        ObjectSpawn();
    }
}
