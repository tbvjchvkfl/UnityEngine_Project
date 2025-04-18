using System.Collections;
using System.IO;
using TMPro;
using UnityEditor.Overlays;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Main Menu")]
    public GameObject MenuObject;
    public GameObject RotationObject;
    public GameObject RotationSubObject;
    public Button GameStartBtn;
    public Button TutorialBtn;
    public Button ExitBtn;
    public TMP_Text GameStartText;
    public TMP_Text TutorialText;
    public TMP_Text ExitText;

    [Header("Tutorial Menu")]
    public TMP_Text TutoTitleText;
    public TMP_Text Tuto_1_Desc;
    public TMP_Text Tuto_2_Desc;
    public TMP_Text Tuto_3_Desc;
    public Image TutoImg;

    [Header("Title Menu")]
    public GameObject TitleObj;
    public TMP_Text TitleText;
    public Button SubmitBtn;
    public Button CancelBtn;

    bool bIsRotation { get; set; }
    bool bIsTutorial { get; set; }
    bool bIsGameStart { get; set; }
    float RotationSpeed;
    float MenuObjMovingSpeed;

    Coroutine CurrentCoroutine;

    void Awake()
    {
        SetDefaultFocus();
        SetEssentialData();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (!bIsRotation && !bIsGameStart)
            {
                if (EventSystem.current.currentSelectedGameObject == GameStartBtn.gameObject)
                {
                    EventSystem.current.SetSelectedGameObject(null);
                    bIsRotation = true;
                    CheckActiveCoroutine(StartCoroutine(RotateGametoTuto()));
                }
                if (EventSystem.current.currentSelectedGameObject == ExitBtn.gameObject)
                {
                    EventSystem.current.SetSelectedGameObject(null);
                    bIsRotation = true;
                    CheckActiveCoroutine(StartCoroutine(RotateExittoGame()));
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (!bIsRotation && !bIsGameStart)
            {
                if (EventSystem.current.currentSelectedGameObject == GameStartBtn.gameObject)
                {
                    EventSystem.current.SetSelectedGameObject(null);
                    bIsRotation = true;
                    CheckActiveCoroutine(StartCoroutine(RotateGametoExit()));
                }
                if (EventSystem.current.currentSelectedGameObject == TutorialBtn.gameObject)
                {
                    EventSystem.current.SetSelectedGameObject(null);
                    bIsRotation = true;
                    CheckActiveCoroutine(StartCoroutine(RotateTutotoGame()));
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (bIsTutorial)
            {
                CheckActiveCoroutine(StartCoroutine(HideTutorialMenu()));
            }
            if (bIsGameStart)
            {
                CheckActiveCoroutine(StartCoroutine(HideTitleMenu()));
            }
        }
        SetTextColor();
    }

    void SetEssentialData()
    {
        TutoTitleText.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        Tuto_1_Desc.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        Tuto_2_Desc.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        Tuto_3_Desc.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        TutoImg.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
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

    void CheckActiveCoroutine(Coroutine SelectCoroutine)
    {
        if (CurrentCoroutine != null)
        {
            StopCoroutine(CurrentCoroutine);
        }
        CurrentCoroutine = SelectCoroutine;
    }

    IEnumerator RotateGametoTuto()
    {
        while (bIsRotation)
        {
            yield return null;
            TitleText.color -= new Color(0.0f, 0.0f, 0.0f, Time.fixedDeltaTime * 2.0f);
            if (TitleText.color.a <= 0.0f)
            {
                TitleText.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
            }
            SetTutorialMenu();
            RotationSpeed += 3.0f;
            RotationSpeed = Mathf.Clamp(RotationSpeed, 0.0f, 200.0f);
            RotationObject.transform.Rotate(0.0f, 0.0f, -Time.fixedDeltaTime * RotationSpeed);
            RotationSubObject.transform.Rotate(0.0f, 0.0f, -Time.fixedDeltaTime * RotationSpeed);
            if (RotationObject.transform.eulerAngles.z <= 295.0f && RotationObject.transform.eulerAngles.z > 65.0f)
            {
                bIsRotation = false;
                RotationSpeed = 0.0f;
                RotationObject.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 295.0f);
                RotationSubObject.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 295.0f);
                TutorialBtn.interactable = true;
                GameStartBtn.interactable = false;
                EventSystem.current.SetSelectedGameObject(TutorialBtn.gameObject);
            }
        }
    }

    IEnumerator RotateTutotoGame()
    {
        while (bIsRotation)
        {
            yield return null;
            TitleText.color += new Color(0.0f, 0.0f, 0.0f, Time.fixedDeltaTime * 2.0f);
            if (TitleText.color.a >= 1.0f)
            {
                TitleText.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            }
            CollapsedTutorialMenu();
            RotationSpeed += 3.0f;
            RotationSpeed = Mathf.Clamp(RotationSpeed, 0.0f, 200.0f);
            RotationObject.transform.Rotate(0.0f, 0.0f, Time.fixedDeltaTime * RotationSpeed);
            RotationSubObject.transform.Rotate(0.0f, 0.0f, Time.fixedDeltaTime * RotationSpeed);
            if (RotationObject.transform.eulerAngles.z <= 295.0f && RotationObject.transform.eulerAngles.z >= 0.0f)
            {
                bIsRotation = false;
                RotationSpeed = 0.0f;
                RotationObject.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
                RotationSubObject.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
                TutorialBtn.interactable = false;
                GameStartBtn.interactable = true;
                EventSystem.current.SetSelectedGameObject(GameStartBtn.gameObject);
            }
        }
    }

    IEnumerator RotateGametoExit()
    {
        while (bIsRotation)
        {
            yield return null;
            TitleText.color -= new Color(0.0f, 0.0f, 0.0f, Time.fixedDeltaTime * 2.0f);
            if (TitleText.color.a <= 0.0f)
            {
                TitleText.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
            }
            RotationSpeed += 3.0f;
            RotationSpeed = Mathf.Clamp(RotationSpeed, 0.0f, 200.0f);
            RotationObject.transform.Rotate(0.0f, 0.0f, Time.fixedDeltaTime * RotationSpeed);
            RotationSubObject.transform.Rotate(0.0f, 0.0f, Time.fixedDeltaTime * RotationSpeed);
            if (RotationObject.transform.eulerAngles.z >= 65.0f)
            {
                bIsRotation = false;
                RotationSpeed = 0.0f;
                RotationObject.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 65.0f);
                RotationSubObject.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 65.0f);
                ExitBtn.interactable = true;
                GameStartBtn.interactable = false;
                EventSystem.current.SetSelectedGameObject(ExitBtn.gameObject);
            }
        }
    }

    IEnumerator RotateExittoGame()
    {
        while (bIsRotation)
        {
            yield return null;
            TitleText.color += new Color(0.0f, 0.0f, 0.0f, Time.fixedDeltaTime * 2.0f);
            if (TitleText.color.a >= 1.0f)
            {
                TitleText.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            }
            RotationSpeed += 3.0f;
            RotationSpeed = Mathf.Clamp(RotationSpeed, 0.0f, 200.0f);
            RotationObject.transform.Rotate(0.0f, 0.0f, -Time.fixedDeltaTime * RotationSpeed);
            RotationSubObject.transform.Rotate(0.0f, 0.0f, -Time.fixedDeltaTime * RotationSpeed);
            if (RotationObject.transform.eulerAngles.z >= 65.0f)
            {
                bIsRotation = false;
                RotationSpeed = 0.0f;
                RotationObject.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
                RotationSubObject.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
                ExitBtn.interactable = false;
                GameStartBtn.interactable = true;
                EventSystem.current.SetSelectedGameObject(GameStartBtn.gameObject);
            }
        }
    }

    IEnumerator ShowTutorialMenu()
    {
        while (!bIsTutorial)
        {
            yield return null;
            TutoTitleText.color += new Color(0.0f, 0.0f, 0.0f, Time.fixedDeltaTime);
            Tuto_1_Desc.color += new Color(0.0f, 0.0f, 0.0f, Time.fixedDeltaTime);
            Tuto_2_Desc.color += new Color(0.0f, 0.0f, 0.0f, Time.fixedDeltaTime);
            Tuto_3_Desc.color += new Color(0.0f, 0.0f, 0.0f, Time.fixedDeltaTime);
            TutoImg.color += new Color(0.0f, 0.0f, 0.0f, Time.fixedDeltaTime);
            if (TutoTitleText.color.a >= 1.0f)
            {
                TutoTitleText.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                Tuto_1_Desc.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                Tuto_2_Desc.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                Tuto_3_Desc.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                TutoImg.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            }

            MenuObjMovingSpeed += 2.0f;
            MenuObjMovingSpeed = Mathf.Clamp(MenuObjMovingSpeed, 0.0f, 20.0f);
            MenuObject.transform.Translate(Vector3.down * MenuObjMovingSpeed);
            if (MenuObject.transform.position.y <= -660.0f)
            {
                MenuObjMovingSpeed = 0.0f;
                MenuObject.transform.position = new Vector3(MenuObject.transform.position.x, -660.0f, MenuObject.transform.position.z);
            }

            if (MenuObject.transform.position.y <= -660.0f && TutoTitleText.color.a >= 1.0f)
            {
                bIsTutorial = true;
            }
        }
    }

    IEnumerator HideTutorialMenu()
    {
        while (bIsTutorial)
        {
            yield return null;
            TutoTitleText.color -= new Color(0.0f, 0.0f, 0.0f, 0.008f);
            Tuto_1_Desc.color -= new Color(0.0f, 0.0f, 0.0f, 0.008f);
            Tuto_2_Desc.color -= new Color(0.0f, 0.0f, 0.0f, 0.008f);
            Tuto_3_Desc.color -= new Color(0.0f, 0.0f, 0.0f, 0.008f);
            TutoImg.color -= new Color(0.0f, 0.0f, 0.0f, 0.008f);
            if (TutoTitleText.color.a <= 0.008f)
            {
                TutoTitleText.color = new Color(1.0f, 1.0f, 1.0f, 0.008f);
                Tuto_1_Desc.color = new Color(1.0f, 1.0f, 1.0f, 0.008f);
                Tuto_2_Desc.color = new Color(1.0f, 1.0f, 1.0f, 0.008f);
                Tuto_3_Desc.color = new Color(1.0f, 1.0f, 1.0f, 0.008f);
                TutoImg.color = new Color(1.0f, 1.0f, 1.0f, 0.008f);
            }

            MenuObjMovingSpeed += 2.0f;
            MenuObjMovingSpeed = Mathf.Clamp(MenuObjMovingSpeed, 0.0f, 20.0f);
            MenuObject.transform.Translate(Vector3.up * MenuObjMovingSpeed);
            if (MenuObject.transform.position.y >= 0.0f)
            {
                MenuObjMovingSpeed = 0.0f;
                MenuObject.transform.position = new Vector3(MenuObject.transform.position.x, 0.0f, MenuObject.transform.position.z);
            }

            if (MenuObject.transform.position.y >= 0.0f && TutoTitleText.color.a <= 0.008f)
            {
                bIsTutorial = false;
            }
        }
    }

    IEnumerator ShowTitleMenu()
    {
        while (!bIsGameStart)
        {
            yield return null;
            MenuObjMovingSpeed += 2.0f;
            MenuObjMovingSpeed = Mathf.Clamp(MenuObjMovingSpeed, 0.0f, 20.0f);
            MenuObject.transform.Translate(Vector3.down * MenuObjMovingSpeed);
            if (MenuObject.transform.position.y <= -660.0f)
            {
                MenuObjMovingSpeed = 0.0f;
                MenuObject.transform.position = new Vector3(MenuObject.transform.position.x, -660.0f, MenuObject.transform.position.z);
                bIsGameStart = true;
                TitleObj.SetActive(true);
                EventSystem.current.SetSelectedGameObject(SubmitBtn.gameObject);
                GameStartBtn.interactable = false;
            }
        }
    }

    IEnumerator HideTitleMenu()
    {
        while (bIsGameStart)
        {
            yield return null;
            MenuObjMovingSpeed += 2.0f;
            MenuObjMovingSpeed = Mathf.Clamp(MenuObjMovingSpeed, 0.0f, 20.0f);
            MenuObject.transform.Translate(Vector3.up * MenuObjMovingSpeed);
            if (MenuObject.transform.position.y >= 0.0f)
            {
                MenuObjMovingSpeed = 0.0f;
                MenuObject.transform.position = new Vector3(MenuObject.transform.position.x, 0.0f, MenuObject.transform.position.z);
                bIsGameStart = false;
                TitleObj.SetActive(false);
                GameStartBtn.interactable = true;
                EventSystem.current.SetSelectedGameObject(GameStartBtn.gameObject);
            }
        }
    }

    void SetTutorialMenu()
    {
        TutoTitleText.color += new Color(1.0f, 1.0f, 1.0f, Time.fixedDeltaTime);
        Tuto_1_Desc.color += new Color(1.0f, 1.0f, 1.0f, Time.fixedDeltaTime);
        Tuto_2_Desc.color += new Color(1.0f, 1.0f, 1.0f, Time.fixedDeltaTime);
        Tuto_3_Desc.color += new Color(1.0f, 1.0f, 1.0f, Time.fixedDeltaTime);
        TutoImg.color += new Color(1.0f, 1.0f, 1.0f, Time.fixedDeltaTime);
        if (TutoTitleText.color.a >= 0.008f)
        {
            TutoTitleText.color = new Color(1.0f, 1.0f, 1.0f, 0.008f);
            Tuto_1_Desc.color = new Color(1.0f, 1.0f, 1.0f, 0.008f);
            Tuto_2_Desc.color = new Color(1.0f, 1.0f, 1.0f, 0.008f);
            Tuto_3_Desc.color = new Color(1.0f, 1.0f, 1.0f, 0.008f);
            TutoImg.color = new Color(1.0f, 1.0f, 1.0f, 0.008f);
        }
    }

    void CollapsedTutorialMenu()
    {
        TutoTitleText.color -= new Color(1.0f, 1.0f, 1.0f, Time.fixedDeltaTime);
        Tuto_1_Desc.color -= new Color(1.0f, 1.0f, 1.0f, Time.fixedDeltaTime);
        Tuto_2_Desc.color -= new Color(1.0f, 1.0f, 1.0f, Time.fixedDeltaTime);
        Tuto_3_Desc.color -= new Color(1.0f, 1.0f, 1.0f, Time.fixedDeltaTime);
        TutoImg.color -= new Color(1.0f, 1.0f, 1.0f, Time.fixedDeltaTime);
        if (TutoTitleText.color.a <= 0.0f)
        {
            TutoTitleText.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
            Tuto_1_Desc.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
            Tuto_2_Desc.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
            Tuto_3_Desc.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
            TutoImg.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        }
    }

    public void OnClickedGameStartButton()
    {
        if (File.Exists(GameManager.Instance.SavePath))
        {
            CheckActiveCoroutine(StartCoroutine(ShowTitleMenu()));
        }
        else
        {
            SceneManager.LoadScene("Stage_1");
        }
    }

    public void OnClickedTutorialButton()
    {
        CheckActiveCoroutine(StartCoroutine(ShowTutorialMenu()));
    }

    public void OnClickedExitButton()
    {
        Application.Quit();
    }

    public void OnClickedSubmitButton()
    {
        if (GameManager.Instance.StageNumber <= 1)
        {
            SceneManager.LoadScene("Stage_1");
        }
        if (GameManager.Instance.StageNumber == 2)
        {
            SceneManager.LoadScene("Stage_1_Boss");
        }
    }

    public void OnClickedCancelButton()
    {
        CheckActiveCoroutine(StartCoroutine(HideTitleMenu()));
    }
}
