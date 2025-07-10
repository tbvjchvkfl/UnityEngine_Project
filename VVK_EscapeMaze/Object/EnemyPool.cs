using NUnit.Framework.Interfaces;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    public GameObject EnemyDummy;
    public int PoolSize = 50;

    List<GameObject> EnemyList = new List<GameObject>();

    public bool bIsPoolReady { get; private set; }

    void Awake()
    {
        EnemyList.Clear();
        if (EnemyDummy)
        {
            for(int i = 0; i< PoolSize; i++)
            {
                GameObject EnemyCharacter = Instantiate(EnemyDummy, transform);
                EnemyCharacter.SetActive(false);
                EnemyList.Add(EnemyCharacter);
            }
            bIsPoolReady = true;
        }
    }

    public GameObject UseEnemyPool()
    {
        if (EnemyList.Count > 0)
        {
            GameObject UseEnemy = EnemyList[EnemyList.Count - 1];
            EnemyList.RemoveAt(EnemyList.Count - 1);
            UseEnemy.SetActive(true);
            return UseEnemy;
        }
        return null;
    }

    public void ReturnEnemyPool(GameObject Enemy)
    {
        if (Enemy)
        {
            Enemy.SetActive(false);
            Enemy.transform.position = transform.position;
            EnemyList.Add(Enemy);
        }
    }
}
