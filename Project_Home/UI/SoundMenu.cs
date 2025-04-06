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

    void Start()
    {
        
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
            AllVolumeSubjectText.color = Color.red;
            AllVolumeText.text = $"{Mathf.Round(AllVolume.value * 100.0f)}";
        }
        else
        {
            AllVolumeSubjectText.color = Color.white;
        }
    }

    void OnFocusBackGroundVolumeSlider()
    {
        if (EventSystem.current.currentSelectedGameObject == BackGroundVolume.gameObject)
        {
            BackGroundSubjectVolumeText.color = Color.red;
            BackGroundVolumeText.text = $"{Mathf.Round(BackGroundVolume.value * 100.0f)}";
        }
        else
        {
            BackGroundSubjectVolumeText.color = Color.white;
        }
    }

    void OnFocusSoundEffectVolumeSlider()
    {
        if (EventSystem.current.currentSelectedGameObject == SoundEffectVolume.gameObject)
        {
            SoundEffectSubjectVolumeText.color = Color.red;
            SoundEffectVolumeText.text = $"{Mathf.Round(SoundEffectVolume.value * 100.0f)}";
        }
        else
        {
            SoundEffectSubjectVolumeText.color = Color.white;
        }
    }
}
