using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class HUD : MonoBehaviour
{
    public GameObject PlayerHUD;
    public GameObject ScreenHUD;
    public GameObject PauseMenu;

    bool bIsPause;

    void Start()
    {
        bIsPause = false;
        if (PauseMenu)
        {
            PauseMenu.gameObject.SetActive(false);
        }
        else
        {
            PauseMenu = GameObject.FindGameObjectWithTag("Pause Menu");
        }
    }

    void Update()
    {
        
    }

    public void TogglePauseMenu()
    {
        if (bIsPause)
        {
            if (PauseMenu)
            {
                PauseMenu.SetActive(false);
                bIsPause = false;
                Time.timeScale = 1.0f;
            }
        }
        else
        {
            if (PauseMenu)
            {
                PauseMenu.SetActive(true);
                bIsPause = true;
                Time.timeScale = 0.0f;
            }
        }
    }
}
