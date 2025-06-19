using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SettingMenu : MonoBehaviour
{
    public Button Control_Btn;
    public Button Graphic_Btn;
    public Button Sound_Btn;
    public Button Return_Btn;

    public GameObject ControlMenu;
    public GameObject GraphicMenu;
    public GameObject SoundMenu;
    public GameObject ReturnMenu;

    GraphicSet graphicMenu;
    SoundSet soundMenu;
    PlayerCharacter playerCharacter;

    public void InitializeSettingMenu(GameObject owner)
    {
        InitComponent(owner);

        Control_Btn.onClick.AddListener(OnClickedControlButton);
        Graphic_Btn.onClick.AddListener(OnClickedGraphicButton);
        Sound_Btn.onClick.AddListener(OnClickedSoundButton);
        Return_Btn.onClick.AddListener(OnClickedReturnButton);

        EventSystem.current.SetSelectedGameObject(Control_Btn.gameObject);
    }

    void InitComponent(GameObject owner)
    {
        if (owner)
        {
            playerCharacter = owner.GetComponent<PlayerCharacter>();

            if (ControlMenu)
            {
                ControlMenu.SetActive(false);
            }
            if (GraphicMenu)
            {
                graphicMenu = GraphicMenu.GetComponent<GraphicSet>();
                graphicMenu.InitGraphicSet(owner);
                GraphicMenu.SetActive(false);

            }
            if (SoundMenu)
            {
                soundMenu = SoundMenu.GetComponent<SoundSet>();
                soundMenu.InitSoundSet();
                SoundMenu.SetActive(false);
            }
            if (ReturnMenu)
            {
                ReturnMenu.SetActive(false);
            }
        }
    }

    public void OnClickedControlButton()
    {
        ControlMenu.SetActive(true);
        GraphicMenu.SetActive(false);
        SoundMenu.SetActive(false);
        ReturnMenu.SetActive(false);
    }

    public void OnClickedGraphicButton()
    {
        ControlMenu.SetActive(false);
        GraphicMenu.SetActive(true);
        SoundMenu.SetActive(false);
        ReturnMenu.SetActive(false);
    }

    public void OnClickedSoundButton()
    {
        ControlMenu.SetActive(false);
        GraphicMenu.SetActive(false);
        SoundMenu.SetActive(true);
        ReturnMenu.SetActive(false);
    }

    public void OnClickedReturnButton()
    {
        ControlMenu.SetActive(false);
        GraphicMenu.SetActive(false);
        SoundMenu.SetActive(false);
        ReturnMenu.SetActive(true);
    }
}
