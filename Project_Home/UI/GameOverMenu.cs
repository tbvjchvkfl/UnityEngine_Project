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

    public bool bIsGameOver {  get; set; }

    void Awake()
    {
        SetInitFocus();
    }

    void Update()
    {
        OnFocusRetryButton();
        OnFocusMenuButton();
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
        SceneManager.LoadScene("Stage_1_Boss");
        Time.timeScale = 1.0f;
    }

    public void OnClickedMenuButton()
    {
        SceneManager.LoadScene("Main Menu");
        Time.timeScale = 1.0f;
    }
}
