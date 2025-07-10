using UnityEngine;

public class EnemyCharacter : MonoBehaviour
{
    
    public float CurrentHP { get; private set; }
    public float MaxHP { get; private set; }
    public float AttackDamage { get; private set; }
    public float DefenseRate { get; private set; }
    public float DodgeRate { get; private set; }

    EnemyStateMachine enemyStateMachine;
    EnemyBase enemyData;

    public void InitializedEnemyData(EnemyBase EnemyData)
    {
        if (EnemyData)
        {
            enemyData = EnemyData;
            MaxHP = enemyData.statusData.MaxHP;
            AttackDamage = enemyData.statusData.AttackRate;
            DefenseRate = enemyData.statusData.DefenseRate;
            DodgeRate = enemyData.statusData.DodgeRate;

            CurrentHP = MaxHP;
            enemyStateMachine = GetComponent<EnemyStateMachine>();
            if (enemyStateMachine)
            {
                enemyStateMachine.OnTakeDamageEvent += RefreshHealthPoint;
            }

            for (int i = 0; i < 6; i++)
            {
                transform.GetChild(2).GetChild(i).GetComponent<SkinnedMeshRenderer>().material = enemyData.enemyPrefabData.EnemyPrefab.transform.GetChild(2).GetChild(i).GetComponent<SkinnedMeshRenderer>().sharedMaterial;
            }

            enemyStateMachine.InitEssentialData(AttackDamage, DefenseRate, DodgeRate);
        }
    }

    void RefreshHealthPoint(float damage)
    {
        CurrentHP = Mathf.Clamp(CurrentHP - damage, 0, MaxHP);
    }
}
