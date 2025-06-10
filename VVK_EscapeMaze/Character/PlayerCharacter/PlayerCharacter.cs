using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    public HUD PlayerHUD;
    public GameObject CameraComponent;

    CharacterMovement characterMovement;
    CharacterAction characterAction;
    PCInputManager inputManager;
    PlayerInventory playerInventory;
    

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
        inputManager = GetComponent<PCInputManager>();
        playerInventory = GetComponent<PlayerInventory>();

        InitPlayerCharacter();
    }

    void Start()
    {
        inputManager.OnInteractionEvent += Interaction;
        inputManager.OnInventoryEvent += ShowInventory;
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

    void Interaction()
    {
        // ~0은 모든 레이어를 포함하라 라는 의미
        Ray ray = new Ray(transform.position, CameraComponent.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, 1.0f, ~0, QueryTriggerInteraction.Collide))
        {
            if (hit.collider.CompareTag("Item"))
            {
                playerInventory.CheckItem(hit.collider.gameObject);
            }
        }
        Debug.DrawRay(transform.position, CameraComponent.transform.forward * 1.0f, Color.green, 2.0f);
    }

    void ShowInventory()
    {
        if(PlayerHUD)
        {
            PlayerHUD.ToggleInventory();
        }
    }

    public void SetSkillPoint(float value)
    {
        SkillPoint = value;
    }
}
