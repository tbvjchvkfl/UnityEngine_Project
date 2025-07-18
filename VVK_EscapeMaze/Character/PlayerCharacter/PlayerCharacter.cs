using System.Collections;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    public HUD PlayerHUD;
    public GameObject CameraComponent;

    CharacterMovement characterMovement;
    CharacterAction characterAction;
    PCInputManager inputManager;
    PlayerInventory playerInventory;

    Coroutine reactCoroutine = null;

    public float maxHealth { get; private set; } = 100.0f;
    public float currentHealth { get; private set; }
    public int maxSkillPoint { get; private set; } = 13;
    public int currentSkillPoint { get; private set; } = 0;
    public int technicalPoint { get; private set; } = 0;

    public float AttackRate { get; private set; }
    public float ArmorRate { get; private set; }
    public float AimRate { get; private set; }
    
    public delegate void OnHealthChangedDelegate(float currentHealth, float maxHealth);
    public event OnHealthChangedDelegate OnHealthChanged;

    void Awake()
    {
        characterMovement = GetComponent<CharacterMovement>();
        inputManager = GetComponent<PCInputManager>();
        playerInventory = GetComponent<PlayerInventory>();
        characterAction = GetComponentInChildren<CharacterAction>();

        InitPlayerCharacter();
        characterMovement.InitEssentialData();
        characterAction.InitEssentialData();
        playerInventory.InitializeInventory();
        playerInventory.InitializeSkillInventory();
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

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (PlayerHUD.Inventory.activeSelf || PlayerHUD.SkillTree.activeSelf || PlayerHUD.SettingUI.activeSelf)
            {
                PlayerHUD.LeftChangedUIMenu();
            }
        }
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
        if (!PlayerHUD.Inventory.activeSelf && !PlayerHUD.SkillTree.activeSelf && !PlayerHUD.SettingUI.activeSelf)
        {
            Ray ray = new Ray(transform.position, CameraComponent.transform.forward);
            if (Physics.Raycast(ray, out RaycastHit hit, 5.0f, ~0, QueryTriggerInteraction.Collide))
            {
                if (hit.collider.CompareTag("Item"))
                {
                    playerInventory.CheckItem(hit.collider.gameObject);
                }
                if (hit.collider.CompareTag("InteractableObj"))
                {
                    hit.collider.gameObject.GetComponent<LinkingBridge>().StartLink();
                }
            }
        }
        else
        {
            PlayerHUD.RightChangedUIMenu();
        }
    }

    void ShowInventory()
    {
        if(PlayerHUD)
        {
            PlayerHUD.ToggleInventory();
        }
    }

    public void SetSkillPoint(int value)
    {
        currentSkillPoint = value;
    }

    public void ApplyItemEffect(float ItemHealthPoint, int ItemSkillPoint)
    {
        currentHealth = Mathf.Clamp(currentHealth + ItemHealthPoint, 0.0f, maxHealth);

        currentSkillPoint = Mathf.Clamp(currentSkillPoint + ItemSkillPoint, 0, maxSkillPoint);
    }

    public void ApplyCharacterStat(int TP, float Attack, float Armor, float Aim)
    {
        technicalPoint -= TP;
        AttackRate += Attack;
        ArmorRate += Armor;
        AimRate += Aim;
    }

    public void TakeDamage(float damage)
    {
        inputManager.bIsHit = true;
        currentHealth = Mathf.Clamp(currentHealth - damage, 0.0f, maxHealth);

        if (reactCoroutine == null)
        {
            reactCoroutine = StartCoroutine(OnReact());
        }

        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    IEnumerator OnReact()
    {
        yield return new WaitForSeconds(0.15f);
        inputManager.bIsHit = false;
        reactCoroutine = null;
    }

    
}
