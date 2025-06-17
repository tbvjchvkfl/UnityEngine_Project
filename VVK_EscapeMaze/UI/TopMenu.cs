using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TopMenu : MonoBehaviour
{
    public Button LeftArrowButton;
    public Button RightArrowButton;
    public Text MenuNameText;

    HUD parentUI;
    public int ListIndex {  get; set; }
    string[] MenuList = new string[4];

    public void InitializeTopMenuUI(HUD parent)
    {
        if (parent)
        {
            parentUI = parent;
            MenuList[0] = "MAP";
            MenuList[1] = "INVENTORY";
            MenuList[2] = "SKILL";
            MenuList[3] = "SETTING";
            ListIndex = 1;
        }

        LeftArrowButton.onClick.AddListener(OnClickedLeftArrowButton);
        RightArrowButton.onClick.AddListener(OnClickedRightButton);
    }

    public void OnClickedLeftArrowButton()
    {
        ListIndex = Mathf.Clamp(ListIndex - 1, 0, MenuList.Length - 1);
        MenuNameText.text = MenuList[ListIndex];
        foreach (KeyValuePair<string, GameObject> list in parentUI.UIObjectMap)
        {
            if (list.Key == MenuList[ListIndex])
            {
                list.Value.SetActive(true);
            }
            else
            {
                list.Value.SetActive(false);
            }
        }
    }

    public void OnClickedRightButton()
    {
        ListIndex = Mathf.Clamp(ListIndex + 1, 0, MenuList.Length - 1);
        MenuNameText.text = MenuList[ListIndex];
        foreach (KeyValuePair<string, GameObject> list in parentUI.UIObjectMap)
        {
            if (list.Key == MenuList[ListIndex])
            {
                list.Value.SetActive(true);
            }
            else
            {
                list.Value.SetActive(false);
            }
        }
    }
}
