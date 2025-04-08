using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class HUD : MonoBehaviour
{
    // Property
    GameObject PlayerHUD;
    GameObject PauseMenu;

    public bool bIsPause { get; set; }

    public static HUD Instance { get; private set; }
    public PauseMenu PauseMenuInstance
    {
        get { return PauseMenu.GetComponent<PauseMenu>(); }
    }
    public PlayerHUD PlayerHealthBar
    {
        get { return PlayerHUD.GetComponent<PlayerHUD>(); }
    }

    void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        InitUIObjects();
    }

    void Update()
    {
    }

    void InitUIObjects()
    {
        if (!PauseMenu)
        {
            if (PauseMenu = GameObject.FindGameObjectWithTag("Pause Menu"))
            {
                PauseMenu.SetActive(false);
                bIsPause = false;
            }
        }
        if (!PlayerHUD)
        {
            if (PlayerHUD = GameObject.FindGameObjectWithTag("Player HUD"))
            {
                PlayerHealthBar.ShowPlayerUI();
            }
        }
    }

    public void TogglePauseMenu()
    {
        if (bIsPause)
        {
            if (PauseMenu)
            {
                PauseMenu.SetActive(false);
                PauseMenu.GetComponent<PauseMenu>().HideSettingMenu();
                bIsPause = false;
                Time.timeScale = 1.0f;
            }
        }
        else
        {
            if (PauseMenu)
            {
                PauseMenu.SetActive(true);
                PauseMenu.GetComponent<PauseMenu>().SetDefaultFocus();
                bIsPause = true;
                Time.timeScale = 0.0f;
            }
        }
    }
}
