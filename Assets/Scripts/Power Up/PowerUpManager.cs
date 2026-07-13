using System;
using UnityEngine;

[System.Serializable]
public enum BoostType
{
    None,
    Speed,
    Health,
    Hunger
}

[System.Serializable]
public class PowerUpNodeData
{
    public string powerUpNodeID;
    
}

public class PowerUpManager : MonoBehaviour
{
    public static PowerUpManager instance {get; private set;}
    
    [SerializeField] private float speedBoost;  // increase speed
    [SerializeField] private float healthBoost; // increase maxHealth
    [SerializeField] private float hungerBoost; // decrease hunger
    
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else 
            Destroy(gameObject);
    }

    public void AddPowerUp(BoostType boostType, float addPowerUp)
    {
        switch (boostType)
        {
            case BoostType.Speed:
                speedBoost += addPowerUp;
                break;
            case  BoostType.Health:
                healthBoost += addPowerUp;
                break;
            case BoostType.Hunger:
                hungerBoost += addPowerUp;
                break;
            
            default:
                Debug.LogWarning($"Warning cannot find boost for {boostType}");
                break;
        }
    }
}
