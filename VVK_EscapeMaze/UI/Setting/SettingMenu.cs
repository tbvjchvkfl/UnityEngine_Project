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
                graphicMenu.ExitFocusEvent += SetOutGraphicFocus;
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
        ModifyingButtonColor(Control_Btn);
    }

    public void OnClickedGraphicButton()
    {
        ControlMenu.SetActive(false);
        GraphicMenu.SetActive(true);
        SoundMenu.SetActive(false);
        ReturnMenu.SetActive(false);
        graphicMenu.SetDefaultFocus();
        ModifyingButtonColor(Graphic_Btn);
    }

    public void OnClickedSoundButton()
    {
        ControlMenu.SetActive(false);
        GraphicMenu.SetActive(false);
        SoundMenu.SetActive(true);
        ReturnMenu.SetActive(false);
        ModifyingButtonColor(Sound_Btn);
    }

    public void OnClickedReturnButton()
    {
        ControlMenu.SetActive(false);
        GraphicMenu.SetActive(false);
        SoundMenu.SetActive(false);
        ReturnMenu.SetActive(true);
        ModifyingButtonColor(Return_Btn);
    }

    void ModifyingButtonColor(Button SelectButton)
    {
        ColorBlock Con_Btn = Control_Btn.colors;
        ColorBlock Graph_Btn = Graphic_Btn.colors;
        ColorBlock Sou_Btn = Sound_Btn.colors;
        ColorBlock Ret_Btn = Return_Btn.colors;

        if (SelectButton == Control_Btn)
        {
            Con_Btn.normalColor = new Color(0.2f, 1.0f, 0.0f, 1.0f);
            Graph_Btn.normalColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            Sou_Btn.normalColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            Ret_Btn.normalColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        }
        else if (SelectButton == Graphic_Btn)
        {
            Graph_Btn.normalColor = new Color(0.2f, 1.0f, 0.0f, 1.0f);
            Con_Btn.normalColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            Sou_Btn.normalColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            Ret_Btn.normalColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        }
        else if (SelectButton == Sound_Btn)
        {
            Sou_Btn.normalColor = new Color(0.2f, 1.0f, 0.0f, 1.0f);
            Con_Btn.normalColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            Graph_Btn.normalColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            Ret_Btn.normalColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        }
        else if (SelectButton == Return_Btn)
        {
            Con_Btn.normalColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            Graph_Btn.normalColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            Sou_Btn.normalColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            Ret_Btn.normalColor = new Color(0.2f, 1.0f, 0.0f, 1.0f);
        }
        else
        {
            Con_Btn.normalColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            Graph_Btn.normalColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            Sou_Btn.normalColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            Ret_Btn.normalColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        }
        Control_Btn.colors = Con_Btn;
        Graphic_Btn.colors = Graph_Btn;
        Sound_Btn.colors = Sou_Btn;
        Return_Btn.colors = Ret_Btn;
    }

    void SetOutGraphicFocus()
    {
        EventSystem.current.SetSelectedGameObject(Graphic_Btn.gameObject);
        ModifyingButtonColor(null);
    }
}
