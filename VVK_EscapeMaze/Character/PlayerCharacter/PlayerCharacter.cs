using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerCharacter : MonoBehaviour
{
    public HUD PlayerHUD;

    CharacterMovement characterMovement;
    CharacterAction characterAction;
    PCInputManager inputManager;
    PlayerInventory playerInventory;
    Camera mainCamera;

    Coroutine reactCoroutine = null;


    public float maxHealth { get; private set; } = 100.0f;
    public float currentHealth { get; private set; }
    public int maxSkillPoint { get; private set; } = 13;
    public int currentSkillPoint { get; private set; } = 0;
    public int technicalPoint { get; private set; } = 10;

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
        mainCamera = Camera.main;

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
            Ray ray = new Ray(transform.position, mainCamera.transform.forward);
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
                if (hit.collider.CompareTag("Cabinet Door"))
                {
                    hit.collider.gameObject.GetComponent<LO_Cabinet>().OpenDoor();
                }
                if (hit.collider.CompareTag("Puse Quest"))
                {
                    hit.collider.gameObject.GetComponent<QuestObjectPuse>().InteractionObject();
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
        currentHealth = Mathf.Clamp(currentHealth - damage, 0.0f, maxHealth);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        if (currentHealth <= 0.0f)
        {
            inputManager.bIsDead = true;
            if (reactCoroutine == null)
            {
                reactCoroutine = StartCoroutine(OnReact(1.0f));
                Debug.Log("Death");
            }
        }
        else
        {
            inputManager.bIsHit = true;
            if (reactCoroutine == null)
            {
                reactCoroutine = StartCoroutine(OnReact(0.15f));
                Debug.Log("Hit");
            }
        }
    }

    IEnumerator OnReact(float shakingTime)
    {
        float CameraShakingTime = shakingTime;
        Vector3 CamOriginPos = mainCamera.transform.position;
        while (CameraShakingTime > 0.0f)
        {
            float XAxisValue = Random.Range(-1.0f, 1.1f);
            float YAxisValue = Random.Range(-1.0f, 1.1f);
            mainCamera.transform.localPosition = new Vector3(mainCamera.transform.localPosition.x + XAxisValue, mainCamera.transform.localPosition.y + YAxisValue, mainCamera.transform.localPosition.z);
            CameraShakingTime -= Time.deltaTime;
            mainCamera.transform.position = CamOriginPos;
            yield return new WaitForSeconds(0.01f);
        }

        mainCamera.transform.position = CamOriginPos;

        if (inputManager.bIsDead)
        {
            yield return null;
        }
        else
        {
            yield return new WaitForSeconds(0.15f);
            inputManager.bIsHit = false;
        }
        reactCoroutine = null;
    }
}
