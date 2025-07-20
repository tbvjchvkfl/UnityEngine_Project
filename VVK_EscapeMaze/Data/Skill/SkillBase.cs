using UnityEngine;

[CreateAssetMenu(fileName = "SkillBase", menuName = "Scriptable Objects/SkillBase")]
public class SkillBase : ScriptableObject
{
    public string SkillName = "New Skill";
    public string SkillDescription = "Skill Description";

    public int SkillID = 0;
    public float SkillIndex = 0.0f;
    public int NeedTechnicalPoint = 1;

    public float IncreaseAttackRate = 1.0f;
    public float IncreaseAimRate = 1.0f;
    public float IncreaseArmorRate = 1.0f;

    public bool bIsActivity = false;

    public Sprite SkillIcon = null;

    public SkillBase CopyData()
    {
        var Copy = CreateInstance<SkillBase>();
        Copy.SkillName = SkillName;
        Copy.SkillDescription = SkillDescription;
        Copy.SkillID = SkillID;
        Copy.NeedTechnicalPoint = NeedTechnicalPoint;
        Copy.IncreaseAttackRate = IncreaseAttackRate;
        Copy.IncreaseAimRate = IncreaseAimRate;
        Copy.IncreaseArmorRate = IncreaseArmorRate;
        Copy.bIsActivity = bIsActivity;
        Copy.SkillIcon = SkillIcon;

        return Copy;
    }
}
