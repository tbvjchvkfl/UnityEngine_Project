using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GraphicMenu : MonoBehaviour
{
    public Slider ScreenBrightness;
    public Slider ScreenMode;
    public Slider ResolutionSetting;

    public TMP_Text BrightnessSettingMainText;
    public TMP_Text ScreenModeSettingMainText;
    public TMP_Text ResolutionSettingMainText;

    public TMP_Text BrightnessSettingText;
    public TMP_Text ScreenModeSettingText;
    public TMP_Text ResolutionSettingText;

    void Start()
    {

    }

    void Update()
    {
        OnFocusScreenBrightness();
        OnFocusScreenMode();
        OnFocusResolutionSetting();
    }

    public void SetDefaultUIFocus()
    {
        EventSystem.current.SetSelectedGameObject(ScreenBrightness.gameObject);
    }

    void OnFocusScreenBrightness()
    {
        if (EventSystem.current.currentSelectedGameObject == ScreenBrightness.gameObject)
        {
            BrightnessSettingMainText.color = Color.white;
            BrightnessSettingText.color = Color.white;
        }
        else
        {
            BrightnessSettingMainText.color = Color.gray;
            BrightnessSettingText.color = Color.gray;
        }
    }

    void OnFocusScreenMode()
    {
        if (EventSystem.current.currentSelectedGameObject == ScreenMode.gameObject)
        {
            ScreenModeSettingMainText.color = Color.white;
            ScreenModeSettingText.color= Color.white;
        }
        else
        {
            ScreenModeSettingMainText.color = Color.gray;
            ScreenModeSettingText.color = Color.gray;
        }
    }

    void OnFocusResolutionSetting()
    {
        if (EventSystem.current.currentSelectedGameObject == ResolutionSetting.gameObject)
        {
            ResolutionSettingMainText.color = Color.white;
            ResolutionSettingText.color= Color.white;
        }
        else
        {
            ResolutionSettingMainText.color = Color.gray;
            ResolutionSettingText.color = Color.gray;
        }
    }
}
