using UnityEngine;

public class Inventory : MonoBehaviour
{
    public InventoryPanel InventoryPanel;
    public GameObject PlayerCharacter;

    void Awake()
    {

    }

    void Start()
    {
        if (InventoryPanel)
        {
            InventoryPanel.InitPanel(PlayerCharacter.GetComponent<PlayerInventory>());
        }
    }

    void Update()
    {
        
    }
}
