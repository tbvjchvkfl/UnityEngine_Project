using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverMenu : MonoBehaviour
{
    public Image BackGroundImage;
    public Button RetryBtn;
    public Button MenuBtn;
    public TMP_Text RetryText;
    public TMP_Text MenuText;
    public TMP_Text TitleText;

    public bool bIsGameOver {  get; set; }
    public bool bIsGameClear {  get; set; }

    void Awake()
    {
        SetInitFocus();
    }

    void Update()
    {
        OnFocusRetryButton();
        OnFocusMenuButton();
        ModifyTitleText();
    }

    void SetInitFocus()
    {
        EventSystem.current.SetSelectedGameObject(RetryBtn.gameObject);
    }

    void OnFocusRetryButton()
    {
        if (EventSystem.current.currentSelectedGameObject == RetryBtn.gameObject)
        {
            RetryText.color = Color.black;
        }
        else
        {
            RetryText.color = Color.white;
        }
    }

    void OnFocusMenuButton()
    {
        if (EventSystem.current.currentSelectedGameObject == MenuBtn.gameObject)
        {
            MenuText.color = Color.black;
        }
        else
        {
            MenuText.color = Color.white;
        }
    }

    public void OnClickedRetryButton()
    {
        if (GameManager.Instance.StageNumber == 1)
        {
            SceneManager.LoadScene("Stage_1");
        }
        if (GameManager.Instance.StageNumber == 2)
        {
            SceneManager.LoadScene("Stage_1_Boss");
        }
        GameManager.Instance.LoadGame();
        bIsGameOver = false;
        bIsGameClear = false;
        Time.timeScale = 1.0f;
    }

    public void OnClickedMenuButton()
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1.0f;
    }

    void ModifyTitleText()
    {
        if (bIsGameOver)
        {
            TitleText.text = "GAME OVER";
        }
        if (bIsGameClear)
        {
            TitleText.text = "GAME CLEAR";
        }
    }
}
