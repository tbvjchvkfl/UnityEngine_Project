using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScreenResolutionSet : MonoBehaviour
{
    public Button LeftArrow_Btn;
    public Button RightArrow_Btn;
    public Button ResolutionSet_Btn;
    public TMP_Text TitleText;

    public GameObject DropMenu;
    public Button UHD_Btn;
    public Button QHD_Btn;
    public Button FHD_Btn;
    public Button HD_Btn;

    List<Resolution> ScreenResolutionList = new List<Resolution>();
    int ListIndex = 0;

    public void InitScreenResolutionUI()
    {
        DropMenu.SetActive(false);

        ScreenResolutionList.Clear();
        foreach (Resolution resol in Screen.resolutions)
        {
            if (resol.width == 3840 && resol.height == 2160 ||
                resol.width == 2560 && resol.height == 1440 ||
                resol.width == 1920 && resol.height == 1080 ||
                resol.width == 1280 && resol.height == 720 && 
                resol.refreshRateRatio.value == 60)
            {
                ScreenResolutionList.Add(resol);
                Debug.Log(resol);
            }
        }


        for (int i = 0; i < ScreenResolutionList.Count; i++)
        {
            if (Screen.currentResolution.width == ScreenResolutionList[i].width)
            {
                ListIndex = i;
                break;
            }
        }
        TitleText.text = $"{Screen.currentResolution.width} X {Screen.currentResolution.height}";


        LeftArrow_Btn.onClick.AddListener(OnClicedLeftArrow_Btn);
        RightArrow_Btn.onClick.AddListener(OnClicedRightArrow_Btn);
        ResolutionSet_Btn.onClick.AddListener(OnClicedResolutionSet_Btn);
        UHD_Btn.onClick.AddListener(OnClicedUHD_Btn);
        QHD_Btn.onClick.AddListener(OnClicedQHD_Btn);
        FHD_Btn.onClick.AddListener(OnClicedFHD_Btn);
        HD_Btn.onClick.AddListener(OnClicedHD_Btn);
    }

    void OnClicedUHD_Btn()
    {
        for (int i = 0; i < ScreenResolutionList.Count; i++)
        {
            if (ScreenResolutionList[i].width == 3840)
            {
                Screen.SetResolution(ScreenResolutionList[i].width, ScreenResolutionList[i].height, false);
                ListIndex = i;
                TitleText.text = $"{ScreenResolutionList[i].width} X {ScreenResolutionList[i].height}";
            }
        }
        DropMenu.SetActive(false);
    }

    void OnClicedQHD_Btn()
    {
        for (int i = 0; i < ScreenResolutionList.Count; i++)
        {
            if (ScreenResolutionList[i].width == 2560)
            {
                Screen.SetResolution(ScreenResolutionList[i].width, ScreenResolutionList[i].height, false);
                ListIndex = i;
                TitleText.text = $"{ScreenResolutionList[i].width} X {ScreenResolutionList[i].height}";
            }
        }
        DropMenu.SetActive(false);
    }

    void OnClicedFHD_Btn()
    {
        for (int i = 0; i < ScreenResolutionList.Count; i++)
        {
            if (ScreenResolutionList[i].width == 1920)
            {
                Screen.SetResolution(ScreenResolutionList[i].width, ScreenResolutionList[i].height, false);
                ListIndex = i;
                TitleText.text = $"{ScreenResolutionList[i].width} X {ScreenResolutionList[i].height}";
            }
        }
        DropMenu.SetActive(false);
    }

    void OnClicedHD_Btn()
    {
        for (int i = 0; i < ScreenResolutionList.Count; i++)
        {
            if (ScreenResolutionList[i].width == 1280)
            {
                Screen.SetResolution(ScreenResolutionList[i].width, ScreenResolutionList[i].height, false);
                ListIndex = i;
                TitleText.text = $"{ScreenResolutionList[i].width} X {ScreenResolutionList[i].height}";
            }
        }
        DropMenu.SetActive(false);
    }

    void OnClicedLeftArrow_Btn()
    {
        ListIndex = Mathf.Clamp(ListIndex - 1, 0, ScreenResolutionList.Count - 1);
        for (int i = 0; i < ScreenResolutionList.Count; i++)
        {
            if (ScreenResolutionList[i].width == 2560)
            {
                Screen.SetResolution(ScreenResolutionList[i].width, ScreenResolutionList[i].height, false);
                ListIndex = i;
                TitleText.text = $"{ScreenResolutionList[i].width} X {ScreenResolutionList[i].height}";
            }
            else if (ScreenResolutionList[i].width == 1920)
            {
                Screen.SetResolution(ScreenResolutionList[i].width, ScreenResolutionList[i].height, false);
                ListIndex = i;
                TitleText.text = $"{ScreenResolutionList[i].width} X {ScreenResolutionList[i].height}";
            }
            else if (ScreenResolutionList[i].width == 1280)
            {
                Screen.SetResolution(ScreenResolutionList[i].width, ScreenResolutionList[i].height, false);
                ListIndex = i;
                TitleText.text = $"{ScreenResolutionList[i].width} X {ScreenResolutionList[i].height}";
            }
        }
    }

    void OnClicedRightArrow_Btn()
    {
        ListIndex = Mathf.Clamp(ListIndex + 1, 0, ScreenResolutionList.Count - 1);
        for (int i = 0; i < ScreenResolutionList.Count; i++)
        {
            if (ScreenResolutionList[i].width == 3840)
            {
                Screen.SetResolution(ScreenResolutionList[i].width, ScreenResolutionList[i].height, false);
                ListIndex = i;
                TitleText.text = $"{ScreenResolutionList[i].width} X {ScreenResolutionList[i].height}";
            }
            else if (ScreenResolutionList[i].width == 2560)
            {
                Screen.SetResolution(ScreenResolutionList[i].width, ScreenResolutionList[i].height, false);
                ListIndex = i;
                TitleText.text = $"{ScreenResolutionList[i].width} X {ScreenResolutionList[i].height}";
            }
            else if (ScreenResolutionList[i].width == 1920)
            {
                Screen.SetResolution(ScreenResolutionList[i].width, ScreenResolutionList[i].height, false);
                ListIndex = i;
                TitleText.text = $"{ScreenResolutionList[i].width} X {ScreenResolutionList[i].height}";
            }
        }
    }

    void OnClicedResolutionSet_Btn()
    {
        if (Screen.fullScreenMode == FullScreenMode.Windowed || Screen.fullScreenMode == FullScreenMode.FullScreenWindow)
        {
            if (DropMenu.activeSelf)
            {
                DropMenu.SetActive(false);
            }
            else
            {
                DropMenu.SetActive(true);
            }
        }
    }
}
