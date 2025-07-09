using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    public float MaxHP = 100;
    public float CurrentHP;
    public float AttackDamage;
    public float Gravity = -9.81f;

    CharacterController enemyController;
    EnemyStateMachine enemyStateMachine;
    Vector3 movementDirection;

    void Awake()
    {
        CurrentHP = MaxHP;
        enemyStateMachine = GetComponent<EnemyStateMachine>();
        if (enemyStateMachine)
        {
            enemyStateMachine.OnTakeDamageEvent += RefreshHealthPoint;
        }
    }

    void Start()
    {
        enemyController = GetComponent<CharacterController>();
    }

    void Update()
    {

    }

    void FixedUpdate()
    {

    }

    void RefreshHealthPoint(float damage)
    {
        CurrentHP -= damage;
    }
}
