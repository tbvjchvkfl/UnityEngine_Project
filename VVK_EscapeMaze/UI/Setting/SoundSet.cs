using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SoundSet : MonoBehaviour
{
    public Slider AllSoundSlider;
    public Button AllSoundLeftArrow_Btn;
    public Button AllSoundRightArrow_Btn;
    public TMP_Text AllSoundValueText;

    public Slider SoundEffectSlider;
    public Button SoundeEffectLeftArrow_Btn;
    public Button SoundeEffectRightArrow_Btn;
    public TMP_Text SoundEffectValueText;

    public Slider BackGroundSlider;
    public Button BackGroundLeftArrow_Btn;
    public Button BackGroundRightArrow_Btn;
    public TMP_Text BackGroundValueText;

    public void InitSoundSet()
    {
        AllSoundSlider.minValue = 0.0f;
        AllSoundSlider.maxValue = 1.0f;
        AllSoundSlider.value = 1.0f;

        SoundEffectSlider.minValue = 0.0f;
        SoundEffectSlider.maxValue = 1.0f;
        SoundEffectSlider.value = 1.0f;

        BackGroundSlider.minValue = 0.0f;
        BackGroundSlider.maxValue = 1.0f;
        BackGroundSlider.value = 1.0f;

        AllSoundValueText.text = $"{Mathf.RoundToInt(AllSoundSlider.value * 100.0f)}";
        SoundEffectValueText.text = $"{Mathf.RoundToInt(SoundEffectSlider.value * 100.0f)}";
        BackGroundValueText.text = $"{Mathf.RoundToInt(BackGroundSlider.value * 100.0f)}";

        AllSoundSlider.onValueChanged.AddListener(OnChangedAllSliderValue);
        SoundEffectSlider.onValueChanged.AddListener(OnChangedSoundEffectSliderValue);
        BackGroundSlider.onValueChanged.AddListener(OnChangedBackGroundSliderValue);

        AllSoundLeftArrow_Btn.onClick.AddListener(OnClickedAllSoundLeftArrow_Btn);
        AllSoundRightArrow_Btn.onClick.AddListener(OnClickedAllSoundRightArrow_Btn);
        SoundeEffectLeftArrow_Btn.onClick.AddListener(OnClickedSoundeEffectLeftArrow_Btn);
        SoundeEffectRightArrow_Btn.onClick.AddListener(OnClickedSoundeEffectRightArrow_Btn);
        BackGroundLeftArrow_Btn.onClick.AddListener(OnClickedBackGroundLeftArrow_Btn);
        BackGroundRightArrow_Btn.onClick.AddListener(OnClickedBackGroundRightArrow_Btn);
    }

    void OnChangedAllSliderValue(float value)
    {
        AllSoundValueText.text = $"{Mathf.RoundToInt(value * 100.0f)}";
        // 슬라이더 값 직접 수정 (전체 사운드)
    }

    void OnChangedSoundEffectSliderValue(float value)
    {
        SoundEffectValueText.text = $"{Mathf.RoundToInt(value * 100.0f)}";
        // 슬라이더 값 직접 수정 (사운드 이펙트)
    }

    void OnChangedBackGroundSliderValue(float value)
    {
        BackGroundValueText.text = $"{Mathf.RoundToInt(value * 100.0f)}";
        // 슬라이더 값 직접 수정 (배경음)
    }

    // 화살표 버튼을 이용한 값 수정
    void OnClickedAllSoundLeftArrow_Btn()
    {
        AllSoundSlider.value = Mathf.Clamp(AllSoundSlider.value - 0.1f, AllSoundSlider.minValue, AllSoundSlider.maxValue);
    }

    void OnClickedAllSoundRightArrow_Btn()
    {
        AllSoundSlider.value = Mathf.Clamp(AllSoundSlider.value + 0.1f, AllSoundSlider.minValue, AllSoundSlider.maxValue);
    }

    void OnClickedSoundeEffectLeftArrow_Btn()
    {
        SoundEffectSlider.value = Mathf.Clamp(SoundEffectSlider.value - 0.1f, SoundEffectSlider.minValue, SoundEffectSlider.maxValue);
    }

    void OnClickedSoundeEffectRightArrow_Btn()
    {
        SoundEffectSlider.value = Mathf.Clamp(SoundEffectSlider.value + 0.1f, SoundEffectSlider.minValue, SoundEffectSlider.maxValue);
    }

    void OnClickedBackGroundLeftArrow_Btn()
    {
        BackGroundSlider.value = Mathf.Clamp(BackGroundSlider.value - 0.1f, BackGroundSlider.minValue, BackGroundSlider.maxValue);
    }

    void OnClickedBackGroundRightArrow_Btn()
    {
        BackGroundSlider.value = Mathf.Clamp(BackGroundSlider.value + 0.1f, BackGroundSlider.minValue, BackGroundSlider.maxValue);
    }
}
