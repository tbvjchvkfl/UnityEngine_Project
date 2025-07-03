using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    public float MaxHP = 100;
    public float CurrentHP;
    public float AttackDamage;
    public float Gravity = -9.81f;

    CharacterController enemyController;
    Vector3 movementDirection;

    void Awake()
    {
        CurrentHP = MaxHP;
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
        //Move();
    }

    void ApplyGravity()
    {
        if (enemyController.isGrounded && movementDirection.y < 0.0f)
        {
            movementDirection.y = -2.0f;
        }
        movementDirection.y += Gravity * Time.deltaTime;
    }

    void Move()
    {
        ApplyGravity();

        enemyController.Move(movementDirection * Time.deltaTime);
    }
}
