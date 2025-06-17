using System.Collections.Generic;
using UnityEngine;

public class HUD : MonoBehaviour
{
    public GameObject InGameHUD;
    public GameObject Inventory;
    public GameObject SkillTree;
    public GameObject TopMenu;
    public GameObject PlayerCharacter;

    public Dictionary<string, GameObject> UIObjectMap {  get; private set; }

    PlayerHUD playerHUD;
    Inventory inventoryUI;
    SkillTree skillTreeUI;
    TopMenu topMenuUI;

    void Awake()
    {
        UIObjectMap = new Dictionary<string, GameObject>();

        if (InGameHUD)
        {
            playerHUD = InGameHUD.GetComponent<PlayerHUD>();
        }
        if (Inventory)
        {
            inventoryUI = Inventory.GetComponent<Inventory>();
            UIObjectMap.Add("INVENTORY", Inventory);
        }
        if (SkillTree)
        {
            skillTreeUI = SkillTree.GetComponent<SkillTree>();
            UIObjectMap.Add("SKILL", SkillTree);
        }
        if (TopMenu)
        {
            topMenuUI = TopMenu.GetComponent<TopMenu>();
        }
    }

    void Start()
    {
        if (PlayerCharacter)
        {
            playerHUD.InitializePlayerHUD(PlayerCharacter);
            inventoryUI.InitializeInventory(PlayerCharacter);
            skillTreeUI.InitializeSkillTreeUI(PlayerCharacter);
            topMenuUI.InitializeTopMenuUI(this);
        }
        Inventory.SetActive(false);
        SkillTree.SetActive(false);
        TopMenu.SetActive(false);
    }

    public void ToggleInventory()
    {
        if (TopMenu.activeSelf || Inventory.activeSelf || SkillTree.activeSelf)
        {
            Inventory.SetActive(false);
            TopMenu.SetActive(false);
            SkillTree.SetActive(false);
        }
        else
        {
            Inventory.SetActive(true);
            TopMenu.SetActive(true);
            topMenuUI.MenuNameText.text = "INVENTORY";
            topMenuUI.ListIndex = 1;
        }
    }

    public void LeftChangedUIMenu()
    {
        topMenuUI.OnClickedLeftArrowButton();
    }

    public void RightChangedUIMenu()
    {
        topMenuUI.OnClickedRightButton();
    }
}
