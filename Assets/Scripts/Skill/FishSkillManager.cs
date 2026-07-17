using System;
using UnityEngine;

public class FishSkillManager : MonoBehaviour
{
    public static FishSkillManager Instance;

    [SerializeField] private FishSkillSO fishSkillData;
    [SerializeField] private bool isActive = false;
    [SerializeField] private bool isReady = false;

    [Header("References")]
    [SerializeField] private FishSkillUI fishSkillUI;
    
    [SerializeField] private float _currActiveSkill;
    private float _currCooldownSkill;
    
    public FishSkillSO UseFishSkillData => fishSkillData;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
        
    }

    private void Start()
    {
        fishSkillData = SkillManager.instance.SelectedFishSkillData.fishSkillData;
        
        if (fishSkillData != null)
            fishSkillUI.InitialazeSkill(fishSkillData.fishSkillIcon);
        
    }

    private void Update()
    {
        UpdateCooldown();
        UpdateActiveSkill();
    }

    private void UpdateCooldown()
    {
        if (isReady)
            return; // nothing to tick, avoid re-decrementing / flicker

        _currCooldownSkill -= Time.deltaTime;

        if (_currCooldownSkill <= 0f)
        {
            _currCooldownSkill = 0f;
            isReady = true;
        }

        fishSkillUI.UpdateCoolDownUI(_currCooldownSkill);
    }

    private void UpdateActiveSkill()
    {
        if (!isActive)
            return; // skill isn't running, nothing to do

        _currActiveSkill -= Time.deltaTime;

        if (_currActiveSkill > 0f)
        {
            GameEvents.OnApplyingSkillEffect?.Invoke(true, fishSkillData.areaSkillEffectType, fishSkillData.fishSkillEffectType, fishSkillData.effectValue);
        }
        else
        {
            isActive = false;
            GameEvents.OnApplyingSkillEffect?.Invoke(false, null, null, 0);
        }
    }


    private bool CheckIsReady()
    {
        _currCooldownSkill -= Time.deltaTime;
        
        return _currCooldownSkill <= 0;
    }

    private bool CheckIsActive()
    {
        _currActiveSkill -= Time.deltaTime;
        
        return _currActiveSkill >= 0;
    }
    
    public void UseFishSkill()
    {
        if (!isReady || isActive)
        {
            Debug.Log($"{fishSkillData.fishSkillName} is not available. Ready: {isReady}, Active: {isActive}");
            return;
        }

        isReady = false;
        isActive = true;

        _currCooldownSkill = fishSkillData.effectCooldown;
        _currActiveSkill = fishSkillData.effectDuration;

        fishSkillUI.StartCoolDown(_currCooldownSkill);
    }
}
