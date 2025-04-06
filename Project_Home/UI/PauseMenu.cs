using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject SettingMenu;
    public GameObject ControlMenu;
    public GameObject GraphicMenu;
    public GameObject SoundMenu;
    public GameObject DirectionArrow;
    public Button ContinueBTN;
    public Button SettingBTN;
    public Button ExitBTN;
    public Button ControllBTN;
    public Button GraphicBTN;
    public Button SoundBTN;

    public bool bIsSettingMenu {  get; private set; }
    public bool bIsControllMenu { get; private set; }
    public bool bIsGraphicMenu { get; private set; }
    public bool bIsSoundMenu { get; private set; }
    float CurrentAngle;

    void Start()
    {
    }

    void Update()
    {
        GameObject CurrentFocusButton = EventSystem.current.currentSelectedGameObject;
        Vector2 ArrowDirection = CurrentFocusButton.GetComponent<RectTransform>().position - DirectionArrow.GetComponent<RectTransform>().position;
        float angle = Mathf.Atan2(ArrowDirection.y, ArrowDirection.x) * Mathf.Rad2Deg;
        DirectionArrow.GetComponent<RectTransform>().rotation = Quaternion.Euler(0.0f, 0.0f, angle - 180.0f);
    }

    public void SetDefaultFocus()
    {
        // 기본 포커스 설정
        EventSystem.current.SetSelectedGameObject(ContinueBTN.gameObject);
    }

    public void OnClickedContinue()
    {
        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);
            HUD.Instance.bIsPause = false;
            Time.timeScale = 1.0f;
        }
    }

    public void OnClickedSetting()
    {
        if (!bIsSettingMenu)
        {
            bIsSettingMenu = true;
            if (!SettingMenu.activeSelf)
            {
                SettingMenu.SetActive(true);
                ContinueBTN.gameObject.SetActive(false);
                SettingBTN.gameObject.SetActive(false);
                ExitBTN.gameObject.SetActive(false);
                EventSystem.current.SetSelectedGameObject(ControllBTN.gameObject);
            }
        }
    }

    public void OnClickedExit()
    {
        SceneManager.LoadScene("Main Menu");
        //Application.Quit();
    }

    public void OnClickedControl()
    {
        if (ControlMenu)
        {
            if (!bIsControllMenu)
            {
                bIsControllMenu = true;
                if (!ControlMenu.activeSelf)
                {
                    ControlMenu.SetActive(true);
                    ControllBTN.gameObject.SetActive(false);
                    GraphicBTN.gameObject.SetActive(false);
                    SoundBTN.gameObject.SetActive(false);
                }
            }
        }
    }

    public void OnClickedGraphic()
    {
        if (GraphicMenu)
        {
            if (!bIsGraphicMenu)
            {
                bIsGraphicMenu = true;
                if (!GraphicMenu.activeSelf)
                {
                    GraphicMenu.SetActive(true);
                    ControllBTN.gameObject.SetActive(false);
                    GraphicBTN.gameObject.SetActive(false);
                    SoundBTN.gameObject.SetActive(false);
                }
            }
        }
    }

    public void OnClickedSound()
    {
        if (SoundMenu)
        {
            if (!bIsSoundMenu)
            {
                bIsSoundMenu = true;
                if (!SoundMenu.activeSelf)
                {
                    SoundMenu.SetActive(true);
                    ControllBTN.gameObject.SetActive(false);
                    GraphicBTN.gameObject.SetActive(false);
                    SoundBTN.gameObject.SetActive(false);
                    SoundMenu.GetComponent<SoundMenu>().SetDefaultUIFocus();
                }
            }
        }
    }

    public void HideSettingMenu()
    {
        if (SettingMenu.activeSelf)
        {
            SettingMenu.SetActive(false);
            bIsSettingMenu = false;
            ContinueBTN.gameObject.SetActive(true);
            SettingBTN.gameObject.SetActive(true);
            ExitBTN.gameObject.SetActive(true);
            EventSystem.current.SetSelectedGameObject(SettingBTN.gameObject);
        }
    }

    public void HideControlMenu()
    {
        if (ControlMenu.activeSelf)
        {
            ControlMenu.SetActive(false);
            bIsControllMenu = false;
            ControllBTN.gameObject.SetActive(true);
            GraphicBTN.gameObject.SetActive(true);
            SoundBTN.gameObject.SetActive(true);
            EventSystem.current.SetSelectedGameObject(ControllBTN.gameObject);
        }
    }

    public void HideGraphicMenu()
    {
        if (GraphicMenu.activeSelf)
        {
            GraphicMenu.SetActive(false);
            bIsGraphicMenu = false;
            ControllBTN.gameObject.SetActive(true);
            GraphicBTN.gameObject.SetActive(true);
            SoundBTN.gameObject.SetActive(true);
            EventSystem.current.SetSelectedGameObject(GraphicBTN.gameObject);
        }
    }

    public void HideSoundMenu()
    {
        if (SoundMenu.activeSelf)
        {
            SoundMenu.SetActive(false);
            bIsSoundMenu = false;
            ControllBTN.gameObject.SetActive(true);
            GraphicBTN.gameObject.SetActive(true);
            SoundBTN.gameObject.SetActive(true);
            EventSystem.current.SetSelectedGameObject(SoundBTN.gameObject);
        }
    }
}
