using UnityEngine;

[System.Serializable]
public class StatusData
{
    public float MaxHP;
    public float AttackRate;
    public float DefenseRate;
    public float DodgeRate;
}

[System.Serializable]
public class EnemyPrefabData
{
    public GameObject EnemyPrefab;
}

[System.Serializable]
public class DropItemData
{
    public GameObject ItemPrefab;
}

[CreateAssetMenu(fileName = "EnemyBase", menuName = "Scriptable Objects/EnemyBase")]
public class EnemyBase : ScriptableObject
{
    public StatusData statusData = new StatusData();
    public EnemyPrefabData enemyPrefabData = new EnemyPrefabData();
    public DropItemData dropItemData = new DropItemData();
}