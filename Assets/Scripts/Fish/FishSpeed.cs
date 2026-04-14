using UnityEngine;

public class FishSpeed : MonoBehaviour
{
    [Header("Fish Speed Config")]
    [SerializeField] private float baseSpeed;

    public float GetFishSpeed(float speedFactor)
    {
        float currentSpeed = baseSpeed * speedFactor;
        return currentSpeed;
    }
}
