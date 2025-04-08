using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SoundMenu : MonoBehaviour
{
    public Slider AllVolume;
    public Slider BackGroundVolume;
    public Slider SoundEffectVolume;

    public TMP_Text AllVolumeSubjectText;
    public TMP_Text BackGroundSubjectVolumeText;
    public TMP_Text SoundEffectSubjectVolumeText;

    public TMP_Text AllVolumeText;
    public TMP_Text BackGroundVolumeText;
    public TMP_Text SoundEffectVolumeText;

    void Awake()
    {
        AllVolume.onValueChanged.AddListener(OnChangedAllVolumeValue);
        BackGroundVolume.onValueChanged.AddListener(OnChangedBackGroundVolumeValue);
        SoundEffectVolume.onValueChanged.AddListener(OnChangedSoundEffectVolumeValue);
    }

    void Update()
    {
        OnFocusAllVolumeSlider();
        OnFocusBackGroundVolumeSlider();
        OnFocusSoundEffectVolumeSlider();
    }

    public void SetDefaultUIFocus()
    {
        EventSystem.current.SetSelectedGameObject(AllVolume.gameObject);
    }

    void OnFocusAllVolumeSlider()
    {
        if (EventSystem.current.currentSelectedGameObject == AllVolume.gameObject)
        {
            AllVolumeSubjectText.color = Color.white;
            AllVolumeText.text = $"{Mathf.Round(AllVolume.value * 100.0f)}";
        }
        else
        {
            AllVolumeSubjectText.color = Color.gray;
        }
    }

    void OnFocusBackGroundVolumeSlider()
    {
        if (EventSystem.current.currentSelectedGameObject == BackGroundVolume.gameObject)
        {
            BackGroundSubjectVolumeText.color = Color.white;
            BackGroundVolumeText.text = $"{Mathf.Round(BackGroundVolume.value * 100.0f)}";
        }
        else
        {
            BackGroundSubjectVolumeText.color = Color.gray;
        }
    }

    void OnFocusSoundEffectVolumeSlider()
    {
        if (EventSystem.current.currentSelectedGameObject == SoundEffectVolume.gameObject)
        {
            SoundEffectSubjectVolumeText.color = Color.white;
            SoundEffectVolumeText.text = $"{Mathf.Round(SoundEffectVolume.value * 100.0f)}";
        }
        else
        {
            SoundEffectSubjectVolumeText.color = Color.gray;
        }
    }

    void OnChangedAllVolumeValue(float value)
    {
        AudioListener.volume = value;
    }

    void OnChangedBackGroundVolumeValue(float value)
    {
        SoundManager.Instance.SetBackGroundValue(value);
    }

    void OnChangedSoundEffectVolumeValue(float value)
    {
        SoundManager.Instance.SetSoundEffectValue(value);
    }
}
