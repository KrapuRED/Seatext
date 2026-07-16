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
    
    [Header("Environment")]
    [SerializeField] private float currentWaterStrength;

    private GameManager _gameManager;
    private PowerUpManager _powerUpManager;
    
    private void OnEnable()
    {
        _gameManager = GameManager.instance;
        _powerUpManager = PowerUpManager.instance;
        
        GameEvents.OnPlayerGainingSpeed.AddListener(AddStackChaseSpeed);
    }

    private void OnDisable()
    {
        GameEvents.OnPlayerGainingSpeed.RemoveListener(AddStackChaseSpeed);
    }
    
    public void InitiliazeFishSpeed(float speed)
    {
        currentWaterStrength = _gameManager.LevelDataSO.currentFlowSpeed;
        baseSpeed = speed;
    }

    private float CalculateFishSpeedWithBoost(float fishSpeed)
    {
        if (_powerUpManager.SpeedBoost <= 0)
            return fishSpeed;
        
        float newFishSpeed = fishSpeed * _powerUpManager.SpeedBoost;
        Debug.Log($"Player get boost speed: {_powerUpManager.SpeedBoost}% -> {newFishSpeed}");
        
        return newFishSpeed;
    }
    
    public float GetFishSpeed(float speedFactor)
    {
        float currentSpeed = ((baseSpeed - currentWaterStrength) * speedFactor) / 10;

        if (ownerFishType == FishType.Player)
            currentSpeed = CalculateFishSpeedWithBoost(currentSpeed);
        
        if (ownerFishType != FishType.Big)
            currentSpeed = Mathf.Clamp(currentSpeed, slowSpeed, someMaxSpeed);
        
        Debug.Log($"[FishSpeed - GetFishSpeed] Base Speed: {baseSpeed}, Speed Factor: {speedFactor}, Current Water {currentWaterStrength} , Current Speed: {currentSpeed}");
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
