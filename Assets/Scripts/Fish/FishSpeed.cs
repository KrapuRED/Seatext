using System;
using UnityEngine;

public class FishSpeed : MonoBehaviour
{
    [Header("Fish Speed Config")]
    public FishType ownerFishType;
    [SerializeField] private float baseSpeed;
    [SerializeField] private float slowSpeed = 0.5f;
    [SerializeField] private float someMaxSpeed;

    [Header("Speed Factors")]
    [SerializeField] private float roamingSpeedFactor;
    [SerializeField] private float chaseSpeedFactor;
    [SerializeField] private int stackChaseSpeedFactor;
    [SerializeField] private float reduceFishSpeedFactor;
    [SerializeField] private float increaseFishSpeedFactor;
    [SerializeField] private bool isBeenReduce;
    [SerializeField] private bool isBeenIncrease;
    
    [Header("Environment")]
    [SerializeField] private float currentWaterStrength;

    private GameManager _gameManager;
    private PowerUpManager _powerUpManager;

    #region  event

    private void OnEnable()
    {
        GameEvents.OnPlayerGainingSpeed.AddListener(AddStackChaseSpeed);
    }

    private void OnDisable()
    {
        OnRemoveListener();
    }

    private void OnDestroy()
    {
        OnRemoveListener();
    }

    private void OnRemoveListener()
    {
        GameEvents.OnPlayerGainingSpeed.RemoveListener(AddStackChaseSpeed);
    }
    #endregion
    
    public void InitiliazeFishSpeed(float speed)
    {
        _gameManager = GameManager.instance;
        _powerUpManager = PowerUpManager.instance;
        
        baseSpeed = speed;

        if (_gameManager == null)
        {
            Debug.LogWarning($"{gameObject.name} dont know the GameManager is been instance or not");
        }
        else
        {
            currentWaterStrength = _gameManager.LevelDataSO.currentFlowSpeed;
        }
    }

    private float CalculateFishSpeedWithBoost(float fishSpeed)
    {
        if (_powerUpManager.SpeedBoost <= 0)
            return fishSpeed;
        
        float newFishSpeed = fishSpeed + _powerUpManager.SpeedBoost / 100f;
        Debug.Log($"Player get boost speed: {_powerUpManager.SpeedBoost}% -> {newFishSpeed}");
        
        return newFishSpeed;
    }
    
    public void ReduceFishSpeed(bool isEffected, float reduceFishSpeed)
    {
        Debug.Log($"Fish Speed get reduce speed: {reduceFishSpeed}");
        isBeenReduce  = isEffected;
        reduceFishSpeedFactor = reduceFishSpeed;
    }
    
    public void IncreaseFishSpeed(bool isEffected, float increaseFishSpeed)
    {
        isBeenIncrease  = isEffected;
        increaseFishSpeedFactor = increaseFishSpeed;
    }
    
    public float GetFishSpeed(float speedFactor)
    {
        float currentSpeed = ((baseSpeed - currentWaterStrength) * speedFactor) / 10;

        if (ownerFishType == FishType.Player)
            currentSpeed = CalculateFishSpeedWithBoost(currentSpeed);
        
        if (isBeenIncrease)
        {
            currentSpeed *= 1f + (increaseFishSpeedFactor / 100f); // 30 -> x1.3
            Debug.Log($"Player get increased speed: {currentSpeed}");
        }

        if (isBeenReduce)
        {
            currentSpeed *= 1f - (reduceFishSpeedFactor / 100f); // 30 -> x0.7
        }

        if (ownerFishType != FishType.Big)
        {
            currentSpeed = Mathf.Clamp(currentSpeed, slowSpeed, someMaxSpeed);
        }
        
        return currentSpeed;
    }

    private void AddStackChaseSpeed()
    {
        if (ownerFishType == FishType.Player)
            stackChaseSpeedFactor++;
    }

    public void ResetStackChaseSpeed()
    {
        stackChaseSpeedFactor = 1;
    }
    
    public float GetRoamingFishSpeed()
    {
        stackChaseSpeedFactor = 1;
        return GetFishSpeed(roamingSpeedFactor);
    }
    
    public float GetChaseFishSpeed() => GetFishSpeed(chaseSpeedFactor * stackChaseSpeedFactor);

}
