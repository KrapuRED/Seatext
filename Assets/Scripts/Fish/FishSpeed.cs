using UnityEngine;

public class FishSpeed : MonoBehaviour
{
    [Header("Fish Speed Config")]
    public FishType ownerFishType;
    [SerializeField] private float baseSpeed;

    [Header("Speed Factors")]
    [SerializeField] private float roamingSpeedFactor;
    [SerializeField] private float chaseSpeedFactor;
    [SerializeField] private int stackChaseSpeedFactor;

    [Header("Environment")]
    [Range(-100f, 100f)]
    [SerializeField] private float currentWaterStrength;
    [SerializeField] private float waterCurrentMultiplier;

    private void OnEnable()
    {
        GameEvents.OnPlayerGainingSpeed.AddListener(AddStackChaseSpeed);
    }

    private void OnDisable()
    {
        GameEvents.OnPlayerGainingSpeed.RemoveListener(AddStackChaseSpeed);
    }

    public void InitiliazeFishSpeed(float speed)
    {
        baseSpeed = speed;
    }

    private void SetWaterCurrent()
    {

    }

    public float GetFishSpeed(float speedFactor)
    {
        float currentSpeed       = ((baseSpeed - currentWaterStrength) * speedFactor) / 10;

        //Debug.Log($"[FishSpeed - GetFishSpeed] Base Speed: {baseSpeed}, Speed Factor: {speedFactor}, Current Speed: {currentSpeed}");
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
