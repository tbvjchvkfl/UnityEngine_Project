using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScreenModeSet : MonoBehaviour
{
    public Button LeftArrow_Btn;
    public Button RightArrow_Btn;
    public Button ScreenMode_Btn;
    public TMP_Text TitleText;

    public GameObject DropMenu;
    public Button FullScreen_Btn;
    public Button BorderlessFullScreen_Btn;
    public Button WindowScreen_Btn;

    List<FullScreenMode> ScreenModeList = new List<FullScreenMode>();
    int ListIndex = 2;

    public void InitScreenModeUI()
    {
        DropMenu.SetActive(false);

        ScreenModeList.Clear();
        ScreenModeList.Add(FullScreenMode.Windowed);
        ScreenModeList.Add(FullScreenMode.FullScreenWindow);
        ScreenModeList.Add(FullScreenMode.ExclusiveFullScreen);

        TitleText.text = "Full Screen";
        Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;

        FullScreen_Btn.onClick.AddListener(OnClickedFullScreen_Btn);
        BorderlessFullScreen_Btn.onClick.AddListener(OnClickedBorderlessFullScreen_Btn);
        WindowScreen_Btn.onClick.AddListener(OnClickedWindowScreen_Btn);
        LeftArrow_Btn.onClick.AddListener(OnClickedLeftArrow_Btn);
        RightArrow_Btn.onClick.AddListener(OnClickedRightArrow_Btn);
        ScreenMode_Btn.onClick.AddListener(OnClickedScreenMode_Btn);
    }

    void OnClickedFullScreen_Btn()
    {
        Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
        ListIndex = 2;
        TitleText.text = "Full Screen";
        DropMenu.SetActive(false);
    }

    void OnClickedBorderlessFullScreen_Btn()
    {
        Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        ListIndex = 1;
        TitleText.text = "Borderless FullScreen";
        DropMenu.SetActive(false);
    }

    void OnClickedWindowScreen_Btn()
    {
        Screen.fullScreenMode = FullScreenMode.Windowed;
        ListIndex = 0;
        TitleText.text = "Window";
        DropMenu.SetActive(false);
    }

    void OnClickedLeftArrow_Btn()
    {
        ListIndex = Mathf.Clamp(ListIndex - 1, 0, ScreenModeList.Count - 1);
        Screen.fullScreenMode = ScreenModeList[ListIndex];
        if (ScreenModeList[ListIndex] == FullScreenMode.FullScreenWindow)
        {
            TitleText.text = "Borderless FullScreen";
        }
        else if (ScreenModeList[ListIndex] == FullScreenMode.Windowed)
        {
            TitleText.text = "Window";
        }
    }

    void OnClickedRightArrow_Btn()
    {
        ListIndex = Mathf.Clamp(ListIndex + 1, 0, ScreenModeList.Count - 1);
        Screen.fullScreenMode = ScreenModeList[ListIndex];
        if (ScreenModeList[ListIndex] == FullScreenMode.ExclusiveFullScreen)
        {
            TitleText.text = "Full Screen";
        }
        else if (ScreenModeList[ListIndex] == FullScreenMode.FullScreenWindow)
        {
            TitleText.text = "Borderless FullScreen";
        }
    }

    void OnClickedScreenMode_Btn()
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
