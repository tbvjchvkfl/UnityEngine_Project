using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GraphicSet : MonoBehaviour
{
    public GameObject ScreenModeObject;
    public GameObject ScreenResolutionObject;
    public GameObject ScreenBrightnessObject;

    ScreenModeSet screenModeUI;
    ScreenResolutionSet screenResolutionUI;
    ScreenBrightnessSet screenBrightnessUI;

    
    

    public void InitGraphicSet(GameObject player)
    {
        if (player)
        {
            if (ScreenModeObject)
            {
                screenModeUI = ScreenModeObject.GetComponent<ScreenModeSet>();
                screenModeUI.InitScreenModeUI();
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
}
