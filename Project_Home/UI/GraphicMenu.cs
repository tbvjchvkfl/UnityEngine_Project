using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GraphicMenu : MonoBehaviour
{
    public Image ScreenBrightLArrow;
    public Image ScreenBrightRArrow;
    public Image ScreenModeLArrow;
    public Image ScreenModeRArrow;
    public Image ResolutionSetLArrow;
    public Image ResolutionSetRArrow;

    public Slider ScreenBrightness;
    public Button ScreenMode;
    public Button ResolutionSetting;

    public TMP_Text BrightnessSettingMainText;
    public TMP_Text ScreenModeSettingMainText;
    public TMP_Text ResolutionSettingMainText;

    public TMP_Text BrightnessSettingText;
    public TMP_Text ScreenModeSettingText;
    public TMP_Text ResolutionSettingText;

    public bool bIsScreenBright { get; set; }
    public bool bIsScreenMode { get; set; }
    public bool bIsResolutionSet {  get; set; }
    public bool bIsMenuActive {  get; set; }
    public bool bIsEnableChangeResolutionMode { get; set; }

    List<Resolution> Resolutions;
    Dictionary<int, Resolution> ResolutionDict;
    int ResolutionSettingIndex;

    List<Tuple<bool, FullScreenMode, string>> ScreenModes;
    int ScreenModeSettingIndex;

    void Awake()
    {
        bIsScreenBright = false;
        bIsScreenMode = false;
        bIsResolutionSet = false;
        bIsMenuActive = false;
        InitScreenResolution();
        InitScreenMode();
        InitScreenBright();
    }

    void Start()
    {
    }

    void Update()
    {
        OnFocusScreenBrightness();
        OnFocusScreenMode();
        OnFocusResolutionSetting();
    }
    
    void InitScreenResolution()
    {
        Resolutions = new List<Resolution>();
        ResolutionDict = new Dictionary<int, Resolution>();
        
        foreach (Resolution resol in Screen.resolutions)
        {
            if (resol.width == 3840 && resol.height == 2160 || 
                resol.width == 2560 && resol.height == 1440 || 
                resol.width == 1920 && resol.height == 1080 || 
                resol.width == 1280 && resol.height == 720)
            {
                Resolutions.Add(resol);
            }
        }

        for (int i = 0; i < Resolutions.Count; i++)
        {
            ResolutionDict.Add(i, Resolutions[i]);
        }

        for (int i = 0; i < ResolutionDict.Count; i++)
        {
            if (ResolutionDict[i].width == Screen.currentResolution.width)
            {
                ResolutionSettingIndex = i;
                ResolutionSettingText.text = $"{ResolutionDict[i]}";
                break;
            }
        }
    }

    void InitScreenMode()
    {
        ScreenModeSettingIndex = 2;
        ScreenModes = new List<Tuple<bool, FullScreenMode, string>>();
        ScreenModes.Add(new Tuple<bool, FullScreenMode, string>(true, FullScreenMode.Windowed, "Windowed"));
        ScreenModes.Add(new Tuple<bool, FullScreenMode, string>(false, FullScreenMode.FullScreenWindow, "FullScreenWindow"));
        ScreenModes.Add(new Tuple<bool, FullScreenMode, string>(false, FullScreenMode.ExclusiveFullScreen, "FullScreen"));
        bIsEnableChangeResolutionMode = ScreenModes[ScreenModeSettingIndex].Item1;
        Screen.fullScreenMode = ScreenModes[ScreenModeSettingIndex].Item2;
        ScreenModeSettingText.text = ScreenModes[ScreenModeSettingIndex].Item3;
    }

    void InitScreenBright()
    {
        ScreenBrightness.onValueChanged.AddListener(ChangedBrightSetting);
        BrightnessSettingText.text = $"{Mathf.Round(ScreenBrightness.value * 10.0f)}";
    }

    public void SetDefaultUIFocus()
    {
        EventSystem.current.SetSelectedGameObject(ScreenMode.gameObject);
    }

    void OnFocusScreenBrightness()
    {
        if (EventSystem.current.currentSelectedGameObject == ScreenBrightness.gameObject)
        {
            BrightnessSettingMainText.color = Color.white;
            BrightnessSettingText.color = Color.gray;
            ScreenBrightLArrow.color = Color.white;
            ScreenBrightRArrow.color = Color.white;
            if (bIsScreenBright)
            {
                ScreenMode.interactable = false;
                ResolutionSetting.interactable = false;
            }
        }
        else
        {
            BrightnessSettingMainText.color = Color.gray;
            BrightnessSettingText.color = Color.white;
            ScreenBrightLArrow.color = Color.gray;
            ScreenBrightRArrow.color= Color.gray;
            bIsScreenBright = false;
            if (!bIsScreenBright && !bIsScreenMode && !bIsResolutionSet)
            {
                ScreenMode.interactable = true;
                ResolutionSetting.interactable = true;
            }
        }
    }

    void OnFocusScreenMode()
    {
        if (EventSystem.current.currentSelectedGameObject == ScreenMode.gameObject)
        {
            ScreenModeSettingMainText.color = Color.white;
            ScreenModeSettingText.color= Color.gray;
            ScreenModeLArrow.color = Color.white;
            ScreenModeRArrow.color = Color.white;
            if (bIsScreenMode)
            {
                ScreenBrightness.interactable= false;
                ResolutionSetting.interactable= false;
                ChangedModeSetting();
            }
        }
        else
        {
            ScreenModeSettingMainText.color = Color.gray;
            ScreenModeSettingText.color = Color.white;
            ScreenModeLArrow.color = Color.gray;
            ScreenModeRArrow.color = Color.gray;
            bIsScreenMode = false;
            if (!bIsScreenBright && !bIsScreenMode && !bIsResolutionSet)
            {
                ScreenBrightness.interactable = true;
                ResolutionSetting.interactable = true;
            }
        }
    }

    void OnFocusResolutionSetting()
    {
        if (EventSystem.current.currentSelectedGameObject == ResolutionSetting.gameObject)
        {
            ResolutionSettingMainText.color = Color.white;
            ResolutionSettingText.color= Color.gray;
            ResolutionSetLArrow.color = Color.white;
            ResolutionSetRArrow.color = Color.white;
            if (bIsResolutionSet)
            {
                ScreenBrightness.interactable = false;
                ScreenMode.interactable = false;
                ChangedResolutionSetting();
            }
        }
        else
        {
            ResolutionSettingMainText.color = Color.gray;
            ResolutionSettingText.color = Color.white;
            ResolutionSetLArrow.color = Color.gray;
            ResolutionSetRArrow.color = Color.gray;
            bIsResolutionSet = false;
            if (!bIsScreenBright && !bIsScreenMode && !bIsResolutionSet)
            {
                ScreenBrightness.interactable = true;
                ScreenMode.interactable = true;
            }
        }
    }

    public void OnClickedScreenMode()
    {
        bIsScreenMode = true;
    }

    public void OnClickedResolutionSet()
    {
        bIsResolutionSet = true;
    }

    void ChangedBrightSetting(float value)
    {
        RenderSettings.ambientLight = new Color(value, value, value, 0.0f);
        BrightnessSettingText.text = $"{Mathf.Round(value * 10.0f)}";
    }

    void ChangedModeSetting()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            ScreenModeSettingIndex++;
            ScreenModeSettingIndex = Mathf.Clamp(ScreenModeSettingIndex, 0, 2);
            ScreenModeSettingText.text = ScreenModes[ScreenModeSettingIndex].Item3;
            bIsMenuActive = true;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            ScreenModeSettingIndex--;
            ScreenModeSettingIndex = Mathf.Clamp(ScreenModeSettingIndex, 0, 2);
            ScreenModeSettingText.text = ScreenModes[ScreenModeSettingIndex].Item3;
            bIsMenuActive = true;
        }
        if (Input.GetKeyDown(KeyCode.Return) && bIsMenuActive)
        {
            Screen.fullScreenMode = ScreenModes[ScreenModeSettingIndex].Item2;
            bIsScreenMode = false;
            bIsMenuActive = false;
        }
    }

    void ChangedResolutionSetting()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            ResolutionSettingIndex++;
            ResolutionSettingIndex = Mathf.Clamp(ResolutionSettingIndex, 0, ResolutionDict.Count - 1);
            ResolutionSettingText.text = $"{ResolutionDict[ResolutionSettingIndex]}";
            bIsMenuActive = true;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            ResolutionSettingIndex--;
            ResolutionSettingIndex = Mathf.Clamp(ResolutionSettingIndex, 0, ResolutionDict.Count - 1);
            ResolutionSettingText.text = $"{ResolutionDict[ResolutionSettingIndex]}";
            bIsMenuActive = true;
        }
        if (Input.GetKeyDown(KeyCode.Return) && bIsMenuActive)
        {
            Screen.SetResolution(ResolutionDict[ResolutionSettingIndex].width, ResolutionDict[ResolutionSettingIndex].height, false);
            bIsResolutionSet = false;
            bIsMenuActive = false;
        }
    }

    public void UnSelectedCancel()
    {
        bIsScreenMode = false;
        bIsResolutionSet = false;
        bIsMenuActive = false;
    }
}
