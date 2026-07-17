using UnityEngine;

[System.Serializable]
public enum AreaSkillEffectType
{
    None,
    Player,
    Around
}

public enum FishSkillEffectType
{
    None,
    Movement,
    Berserk
}

[CreateAssetMenu(fileName = "FishSkillSO", menuName = "Fish Skill/FishSkillSO")]
public class FishSkillSO : ScriptableObject
{
    public string fishSkillName;
    public string fishSkillDescription;
    public Sprite fishSkillIcon;
    public AreaSkillEffectType areaSkillEffectType;
    public FishSkillEffectType fishSkillEffectType;
    public float effectValue;
    public int effectRequired;
    public float effectDuration;
    public float effectCooldown;
}
