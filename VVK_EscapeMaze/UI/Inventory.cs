using UnityEngine;

public class Inventory : MonoBehaviour
{
    public InventoryPanel InventoryPanel;

    void Awake()
    {

    }

    void Start()
    {
        if (InventoryPanel)
        {
            InventoryPanel.InitPanel();
        }
    }

    void Update()
    {
        
    }
}
