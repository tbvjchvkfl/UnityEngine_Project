using System.Xml.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    public Image BackIMG;
    public Image HeartObjectIMG;
    public TMP_Text HOQuantityText;
    public TMP_Text MAXText;
    public float UIHidingTime;

    float HideTime;
    bool bIsHide;
    bool bIsTwinkle;

    void Awake()
    {
        HOQuantityText.text = "0";
        MAXText.text = "MAX";
        MAXText.enabled = false;
        HideTime = UIHidingTime;
    }

    void Update()
    {
        if (GameManager.Instance.PCInput.bIsView)
        {
            HUD.Instance.PlayerHealthBar.ShowPlayerUI();
        }
        HidePlayerUI();
        TwinkleMAXText();
        HOQuantityText.text = $"{GameManager.Instance.PCInfo.CurrentHP}";
    }

    public void ShowPlayerUI()
    {
        HideTime = UIHidingTime;
        bIsHide = false;
        HeartObjectIMG.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        BackIMG.color = new Color(0.3f, 0.3f, 0.3f, 1.0f);
        HOQuantityText.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        if (GameManager.Instance.PCInfo.CurrentHP >= 3)
        {
            MAXText.enabled = true;
            MAXText.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        }
    }

    void HidePlayerUI()
    {
        HideTime -= Time.deltaTime;
        if (HideTime <= 0.0f)
        {
            bIsHide = true;
            HideTime = 0.0f;
            HeartObjectIMG.color -= new Color(0.0f, 0.0f, 0.0f, Time.deltaTime);
            BackIMG.color -= new Color(0.0f, 0.0f, 0.0f, Time.deltaTime);
            HOQuantityText.color -= new Color(0.0f, 0.0f, 0.0f, Time.deltaTime);
            if (HeartObjectIMG.color.a <= 0.0f)
            {
                HeartObjectIMG.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                BackIMG.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                HOQuantityText.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
            }
        }
    }

    void TwinkleMAXText()
    {
        if (!bIsHide)
        {
            if (bIsTwinkle)
            {
                MAXText.color -= new Color(0.0f, 0.0f, 0.0f, Time.deltaTime);
                if (MAXText.color.a <= 0.3f)
                {
                    bIsTwinkle = false;
                }
            }
            else
            {
                MAXText.color += new Color(0.0f, 0.0f, 0.0f, Time.deltaTime);
                if (MAXText.color.a >= 0.9f)
                {
                    bIsTwinkle = true;
                }
            }
        }
        else
        {
            MAXText.color -= new Color(0.0f, 0.0f, 0.0f, Time.deltaTime);
            if (MAXText.color.a <= 0.0f)
            {
                MAXText.color -= new Color(1.0f, 1.0f, 1.0f, 0.0f);
            }
        }
    }
}
