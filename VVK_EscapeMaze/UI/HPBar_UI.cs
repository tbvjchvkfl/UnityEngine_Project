using UnityEngine;
using UnityEngine.UI;

public class HPBar_UI : MonoBehaviour
{
    [Header("UI Component")]
    
    public Text HP_Text;

    PlayerCharacter playerCharacter;
    Slider HPBar_Slider;

  
    public void InitHPBar(GameObject owner)
    {
        if(owner)
        {
            playerCharacter = owner.GetComponent<PlayerCharacter>();
            HPBar_Slider = GetComponent<Slider>();
            if (playerCharacter)
            {
                HPBar_Slider.maxValue = playerCharacter.maxHealth;
                HPBar_Slider.value = playerCharacter.currentHealth;
                HP_Text.text = $"{playerCharacter.currentHealth} / {playerCharacter.maxHealth}";

                playerCharacter.OnHealthChanged += RefreshHPBar;
            }
        }
    }

    void RefreshHPBar(float currentHP, float maxHP)
    {
        if (playerCharacter)
        {
            HPBar_Slider.value = currentHP;
            HP_Text.text = $"{currentHP:F0} / {maxHP:F0}";
        }
    }

    private void OnDestroy()
    {
        if(playerCharacter)
        {
            playerCharacter.OnHealthChanged -= RefreshHPBar;
        }
    }
}
