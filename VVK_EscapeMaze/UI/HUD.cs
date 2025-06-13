using UnityEngine;

public class HUD : MonoBehaviour
{
    public GameObject InGameHUD;
    public GameObject Inventory;
    public GameObject PlayerCharacter;


    PlayerHUD playerHUD;
    Inventory inventoryUI;

    void Awake()
    {
        if (InGameHUD)
        {
            playerHUD = InGameHUD.GetComponent<PlayerHUD>();
        }
        if (Inventory)
        {
            inventoryUI = Inventory.GetComponent<Inventory>();
        }
    }

    void Start()
    {
        if (PlayerCharacter)
        {
            playerHUD.InitializePlayerHUD(PlayerCharacter);
            inventoryUI.InitializeInventory(PlayerCharacter);
        }
        Inventory.SetActive(false);
    }

    public void ToggleInventory()
    {
        if (Inventory.activeSelf)
        {
            Inventory.SetActive(false);
        }
        else
        {
            Inventory.SetActive(true);
        }
    }
}
