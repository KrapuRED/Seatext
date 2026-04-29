using UnityEngine;

public class FishSpeed : MonoBehaviour
{
    [Header("Fish Speed Config")]
    [SerializeField] private float baseSpeed;

    [Header("Speed Factors")]
    [SerializeField] private float roamingSpeedFactor;
    [SerializeField] private float chaseSpeedFactor;
    [SerializeField] private int stackChaseSpeedFactor;

    [Header("Environment")]
    [Range(-100f, 100f)]
    [SerializeField] private float currentWaterStrength;
    [SerializeField] private float waterCurrentMultiplier;

    public void InitiliazeFishSpeed(float speed)
    {
        baseSpeed = speed;
    }

    public float GetFishSpeed(float speedFactor)
    {
        float currentSpeed = (baseSpeed * speedFactor) / 10;
        //Debug.Log($"[FishSpeed - GetFishSpeed] Base Speed: {baseSpeed}, Speed Factor: {speedFactor}, Current Speed: {currentSpeed}");
        return currentSpeed;
    }

    public void AddStackChaseSpeed()
    {
        stackChaseSpeedFactor++;
    }

    public float GetRoamingFishSpeed()
    {
        stackChaseSpeedFactor = 1;
        return GetFishSpeed(roamingSpeedFactor);
    }
    public float GetChaseFishSpeed() => GetFishSpeed(chaseSpeedFactor * stackChaseSpeedFactor);

}
