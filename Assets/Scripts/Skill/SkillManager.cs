using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FishSkillData
{
    public string fishSkillName;
    public FishSkillSO fishSkillData;
    [Range(0, 100)]
    public float change;
}

public class SkillManager : MonoBehaviour
{
    public static SkillManager  instance;

    [Header("Fish Skill Configuration")]
    [SerializeField] private FishSkillData selectedFishSkillData;
    [SerializeField] private List<FishSkillData> fishSkillDataList = new();
    
    public FishSkillData SelectedFishSkillData => selectedFishSkillData;
    
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
        }
    }
}
