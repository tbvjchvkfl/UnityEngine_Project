using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    CharacterMovement characterMovement;
    CharacterAction characterAction;

    public float maxHealth { get; private set; } = 100.0f;
    public float currentHealth { get; private set; }


    public bool bIsDead { get; private set; } = false;
    public float SkillPoint { get; private set; }

    public delegate void OnHealthChangedDelegate(float currentHealth, float maxHealth);
    public event OnHealthChangedDelegate OnHealthChanged;

    void Awake()
    {
        characterMovement = GetComponent<CharacterMovement>();
        characterAction = GetComponent<CharacterAction>();
        InitPlayerCharacter();
    }

    void Update()
    {
        Debug.DrawRay(transform.position, transform.forward * 2, Color.red); // 현재 전방
        Debug.DrawRay(transform.position, characterMovement.currentMoveDirection * 2, Color.blue); // 이동 방향
    }

    void FixedUpdate()
    {
        characterMovement.Move();
    }
    void InitPlayerCharacter()
    {
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public void SetSkillPoint(float value)
    {
        SkillPoint = value;
    }
}
