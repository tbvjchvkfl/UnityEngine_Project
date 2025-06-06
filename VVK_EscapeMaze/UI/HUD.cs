using UnityEngine;

public class HUD : MonoBehaviour
{
    public PlayerHUD InGameHUD;
    public Inventory PlayerInventory;

    void Awake()
    {
        PlayerInventory.gameObject.SetActive(false);
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void ToggleInventory()
    {
        if (PlayerInventory.gameObject.activeSelf)
        {
            PlayerInventory.gameObject.SetActive(false);
        }
        else
        {
            PlayerInventory.gameObject.SetActive(true);
        }
    }
}
