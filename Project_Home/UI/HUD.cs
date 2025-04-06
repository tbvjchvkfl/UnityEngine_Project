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


    // Functionary
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
        InitUIObjects();
    }

    void Start()
    {
    }

    void Update()
    {
        InitUIObjects();
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
                Debug.Log("Find");
            }
            else
            {
                Debug.Log("Null");
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
