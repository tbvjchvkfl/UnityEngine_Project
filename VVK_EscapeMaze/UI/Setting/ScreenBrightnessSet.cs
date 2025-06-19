using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScreenBrightnessSet : MonoBehaviour
{
    public Slider BrightnessSlider;
    public TMP_Text TitleText;
    public Button LeftArrow_Btn;
    public Button RightArrow_Btn; 

    public void InitScreenBrightness()
    {
        BrightnessSlider.minValue = 0.0f;
        BrightnessSlider.maxValue = 1.0f;
        BrightnessSlider.value = 0.5f;

        TitleText.text = $"{Mathf.RoundToInt(BrightnessSlider.value * 100.0f)}";

        LeftArrow_Btn.onClick.AddListener(OnClickedLeftArrow_Btn);
        RightArrow_Btn.onClick.AddListener(OnClickedRightArrow_Btn);
    }

    void OnClickedLeftArrow_Btn()
    {
        BrightnessSlider.value = Mathf.Clamp(BrightnessSlider.value - 0.1f, BrightnessSlider.minValue, BrightnessSlider.maxValue);
        TitleText.text = $"{Mathf.RoundToInt(BrightnessSlider.value * 100.0f)}";
        RenderSettings.ambientLight = new Color(BrightnessSlider.value, BrightnessSlider.value, BrightnessSlider.value, 0.0f);
    }

    void OnClickedRightArrow_Btn()
    {
        BrightnessSlider.value = Mathf.Clamp(BrightnessSlider.value + 0.1f, BrightnessSlider.minValue, BrightnessSlider.maxValue);
        TitleText.text = $"{Mathf.RoundToInt(BrightnessSlider.value * 100.0f)}";
        RenderSettings.ambientLight = new Color(BrightnessSlider.value, BrightnessSlider.value, BrightnessSlider.value, 0.0f);
    }
}
