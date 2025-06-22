using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GraphicSet : MonoBehaviour
{
    public GameObject ScreenModeObject;
    public GameObject ScreenResolutionObject;
    public GameObject ScreenBrightnessObject;

    ScreenModeSet screenModeUI;
    ScreenResolutionSet screenResolutionUI;
    ScreenBrightnessSet screenBrightnessUI;

    public delegate void OnExitFocusDelegate();
    public event OnExitFocusDelegate ExitFocusEvent;

    public void InitGraphicSet(GameObject player)
    {
        if (player)
        {
            if (ScreenModeObject)
            {
                screenModeUI = ScreenModeObject.GetComponent<ScreenModeSet>();
                screenModeUI.InitScreenModeUI();
                screenModeUI.OnExitFocusEvent += ReturnFocusGraphicSet;
            }
            if (ScreenResolutionObject)
            {
                screenResolutionUI = ScreenResolutionObject.GetComponent<ScreenResolutionSet>();
                screenResolutionUI.InitScreenResolutionUI();
            }
            if (ScreenBrightnessObject)
            {
                screenBrightnessUI = ScreenBrightnessObject.GetComponent<ScreenBrightnessSet>();
                screenBrightnessUI.InitScreenBrightness();
            }
        }
    }

    public void SetDefaultFocus()
    {
        if (screenModeUI)
        {
            EventSystem.current.SetSelectedGameObject(screenModeUI.ScreenMode_Btn.gameObject);
        }
    }

    void ReturnFocusGraphicSet()
    {
        ExitFocusEvent?.Invoke();
    }
}
