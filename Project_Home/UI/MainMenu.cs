using TMPro;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Main Menu")]
    public GameObject MenuObject;
    public GameObject RotationObject;
    public Button GameStartBtn;
    public Button TutorialBtn;
    public Button ExitBtn;
    public TMP_Text GameStartText;
    public TMP_Text TutorialText;
    public TMP_Text ExitText;
    

    [Header("Tutorial Menu")]
    public TMP_Text TitleText;
    public TMP_Text Tuto_1_Desc;
    public TMP_Text Tuto_2_Desc;
    public TMP_Text Tuto_3_Desc;
    public Image TutoImg;



    bool bIsRotation { get; set; }
    bool bIsRotation_R { get; set; }
    bool bIsRotation_L { get; set; }
    float RotationSpeed;
    float MenuObjMovingSpeed;


    void Awake()
    {
        SetDefaultFocus();
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            if (!bIsRotation)
            {
                if (EventSystem.current.currentSelectedGameObject == GameStartBtn.gameObject)
                {
                    EventSystem.current.SetSelectedGameObject(null);
                    bIsRotation = true;
                    InvokeRepeating("OnRotateGameStarttoTutorial", 0.0f, Time.deltaTime);
                }
                if (EventSystem.current.currentSelectedGameObject == ExitBtn.gameObject)
                {
                    EventSystem.current.SetSelectedGameObject(null);
                    bIsRotation = true;
                    InvokeRepeating("OnRotateExittoGameStart", 0.0f, Time.deltaTime);
                }
            }
        }
        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            if (!bIsRotation)
            {
                if (EventSystem.current.currentSelectedGameObject == GameStartBtn.gameObject)
                {
                    EventSystem.current.SetSelectedGameObject(null);
                    bIsRotation = true;
                    InvokeRepeating("OnRotateGameStarttoExit", 0.0f, Time.deltaTime);
                }
                if (EventSystem.current.currentSelectedGameObject == TutorialBtn.gameObject)
                {
                    EventSystem.current.SetSelectedGameObject(null);
                    bIsRotation = true;
                    InvokeRepeating("OnRotateTutorialtoGameStart", 0.0f, Time.deltaTime);
                }
            }
        }

        SetTextColor();
    }

    void SetDefaultFocus()
    {
        EventSystem.current.SetSelectedGameObject(GameStartBtn.gameObject);
        TutorialBtn.interactable = false;
        ExitBtn.interactable = false;
    }

    void SetTextColor()
    {
        if (EventSystem.current.currentSelectedGameObject == GameStartBtn.gameObject)
        {
            GameStartText.color = Color.black;
            TutorialText.color = new Color(0.7f, 0.7f, 0.7f, 1.0f);
            ExitText.color = new Color(0.7f, 0.7f, 0.7f, 1.0f);
        }
        if (EventSystem.current.currentSelectedGameObject == TutorialBtn.gameObject)
        {
            GameStartText.color = new Color(0.7f, 0.7f, 0.7f, 1.0f);
            TutorialText.color = Color.black;
            ExitText.color = new Color(0.7f, 0.7f, 0.7f, 1.0f);
        }
        if (EventSystem.current.currentSelectedGameObject == ExitBtn.gameObject)
        {
            GameStartText.color = new Color(0.7f, 0.7f, 0.7f, 1.0f);
            TutorialText.color = new Color(0.7f, 0.7f, 0.7f, 1.0f);
            ExitText.color = Color.black;
        }
        if (!EventSystem.current.currentSelectedGameObject)
        {
            GameStartText.color = new Color(0.7f, 0.7f, 0.7f, 1.0f);
            TutorialText.color = new Color(0.7f, 0.7f, 0.7f, 1.0f);
            ExitText.color = new Color(0.7f, 0.7f, 0.7f, 1.0f);
        }
    }

    void OnRotateGameStarttoTutorial()
    {
        if (bIsRotation)
        {
            RotationSpeed += 3.0f;
            RotationSpeed = Mathf.Clamp(RotationSpeed, 0.0f, 200.0f);
            RotationObject.transform.Rotate(0.0f, 0.0f, -Time.deltaTime * RotationSpeed);
            if (RotationObject.transform.eulerAngles.z <= 295.0f && RotationObject.transform.eulerAngles.z > 65.0f)
            {
                bIsRotation = false;
                RotationSpeed = 0.0f;
                RotationObject.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 295.0f);
                TutorialBtn.interactable = true;
                GameStartBtn.interactable = false;
                EventSystem.current.SetSelectedGameObject(TutorialBtn.gameObject);
                CancelInvoke();
            }
        }
    }

    void OnRotateExittoGameStart()
    {
        if (bIsRotation)
        {
            RotationSpeed += 3.0f;
            RotationSpeed = Mathf.Clamp(RotationSpeed, 0.0f, 200.0f);
            RotationObject.transform.Rotate(0.0f, 0.0f, -Time.deltaTime * RotationSpeed);
            if (RotationObject.transform.eulerAngles.z >= 65.0f)
            {
                bIsRotation = false;
                RotationSpeed = 0.0f;
                RotationObject.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
                ExitBtn.interactable = false;
                GameStartBtn.interactable = true;
                EventSystem.current.SetSelectedGameObject(GameStartBtn.gameObject);
                CancelInvoke();
            }
        }
    }

    void OnRotateTutorialtoGameStart()
    {
        if (bIsRotation)
        {
            RotationSpeed += 3.0f;
            RotationSpeed = Mathf.Clamp(RotationSpeed, 0.0f, 200.0f);
            RotationObject.transform.Rotate(0.0f, 0.0f, Time.deltaTime * RotationSpeed);
            if (RotationObject.transform.eulerAngles.z <= 295.0f && RotationObject.transform.eulerAngles.z >= 0.0f)
            {
                bIsRotation = false;
                RotationSpeed = 0.0f;
                RotationObject.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
                TutorialBtn.interactable = false;
                GameStartBtn.interactable = true;
                EventSystem.current.SetSelectedGameObject(GameStartBtn.gameObject);
                CancelInvoke();
            }
        }
    }

    void OnRotateGameStarttoExit()
    {
        if (bIsRotation)
        {
            RotationSpeed += 3.0f;
            RotationSpeed = Mathf.Clamp(RotationSpeed, 0.0f, 200.0f);
            RotationObject.transform.Rotate(0.0f, 0.0f, Time.deltaTime * RotationSpeed);
            if (RotationObject.transform.eulerAngles.z >= 65.0f)
            {
                bIsRotation = false;
                RotationSpeed = 0.0f;
                RotationObject.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 65.0f);
                ExitBtn.interactable = true;
                GameStartBtn.interactable = false;
                EventSystem.current.SetSelectedGameObject(ExitBtn.gameObject);
                CancelInvoke();
            }
        }
    }

    public void OnClickedGameStartButton()
    {
        SceneManager.LoadScene("Stage_1");
    }

    public void OnClickedTutorialButton()
    {
        InvokeRepeating("ShowTutorialMenu", 0.0f, Time.deltaTime);
    }

    public void OnClickedExitButton()
    {

    }

    void ShowTutorialMenu()
    {
        TitleText.color += new Color(0.0f, 0.0f, 0.0f, Time.deltaTime);
        Tuto_1_Desc.color += new Color(0.0f, 0.0f, 0.0f, Time.deltaTime);
        Tuto_2_Desc.color += new Color(0.0f, 0.0f, 0.0f, Time.deltaTime);
        Tuto_3_Desc.color += new Color(0.0f, 0.0f, 0.0f, Time.deltaTime);
        TutoImg.color += new Color(0.0f, 0.0f, 0.0f, Time.deltaTime);
        if (TitleText.color.a >= 1.0f)
        {
            TitleText.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            Tuto_1_Desc.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            Tuto_2_Desc.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            Tuto_3_Desc.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            TutoImg.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        }

        MenuObjMovingSpeed += 5.0f;
        MenuObjMovingSpeed = Mathf.Clamp(MenuObjMovingSpeed, 0.0f, 200.0f);
        MenuObject.transform.Translate(Vector3.down * Time.deltaTime * MenuObjMovingSpeed);
        Debug.Log(MenuObject.transform.position.y);
        if (MenuObject.transform.position.y <= -660.0f)
        {
            MenuObjMovingSpeed = 0.0f;
            MenuObject.transform.position = new Vector3(0.0f, -660.0f, 0.0f);
        }
        if (MenuObject.transform.position.y <= -1200.0f && TitleText.color.a >= 1.0f)
        {
            CancelInvoke();
        }
    }

    void HideTutorialMenu()
    {

    }
}
