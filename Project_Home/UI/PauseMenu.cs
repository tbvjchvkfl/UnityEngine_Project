using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public Image DirectionArrow;

    void Start()
    {
    }

    void Update()
    {
        
    }

    public void OnClickedContinue()
    {
        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }
    }

    public void OnClickedSetting()
    {

    }

    public void OnClickedExit()
    {
        Application.Quit();
    }
}
