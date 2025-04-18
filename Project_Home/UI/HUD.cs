using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HUD : MonoBehaviour
{
    // Property
    GameObject PlayerHUD;
    GameObject PauseMenu;
    GameObject GameOver;
    GameObject MainMenu;

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
    public GameOverMenu GameOverUI
    {
        get { return GameOver.GetComponent<GameOverMenu>(); }
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
        InitUIObjects();
    }

    void InitUIObjects()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            if (!MainMenu)
            {
                if (MainMenu = GameObject.FindGameObjectWithTag("Main Menu"))
                {
                    MainMenu.SetActive(true);
                }
            }
        }
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            if (!PauseMenu)
            {
                if (PauseMenu = GameObject.FindGameObjectWithTag("Pause Menu"))
                {
                    PauseMenu.SetActive(false);
                    bIsPause = false;
                    Time.timeScale = 1.0f;
                }
            }
            if (!PlayerHUD)
            {
                if (PlayerHUD = GameObject.FindGameObjectWithTag("Player HUD"))
                {
                    PlayerHealthBar.ShowPlayerUI();
                }
            }
            if (!GameOver)
            {
                if (GameOver = GameObject.FindGameObjectWithTag("GameOver Menu"))
                {
                    GameOver.SetActive(false);
                }
            }
        }
        if (SceneManager.GetActiveScene().buildIndex == 2)
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
            if (!GameOver)
            {
                if (GameOver = GameObject.FindGameObjectWithTag("GameOver Menu"))
                {
                    GameOver.SetActive(false);
                }
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

    public void ShowGameOverUI()
    {
        GameOver.SetActive(true);
        GameOverUI.bIsGameOver = true;
        Time.timeScale = 0.0f;
    }
}
