using System;
using UnityEngine;
using System.Collections.Generic;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

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
    
    [Header("PowerUp Settings")]
    [SerializeField] private Transform powerUpContainer;
    private List<PowerUpNode>  _powerUpNodes = new();
    
    [Header("PowerUp Active")]
    [SerializeField] private float speedBoost;  // increase speed
    [SerializeField] private float healthBoost; // increase maxHealth
    [SerializeField] private float hungerBoost; // decrease hunger
    
    private HashSet<string> _speedPowerUpNodeIDs;
    private HashSet<string> _healthPowerUpNodeIDs;
    private HashSet<string> _hungerPowerUpNodeIDs;
    
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else 
            Destroy(gameObject);
    }

    private void Start()
    {

        IntializePowerUps();
    }

    private string GetPowerUpNodeID(int intialID, BoostType boostType)
    {
        string newID =string.Empty;
        
        string boostID = boostType switch
        {
            BoostType.Health => "HP",
            BoostType.Hunger => "HG",
            BoostType.Speed => "SP",
            _ => intialID.ToString()
        };
        
        newID = $"{boostID}_{intialID}";
        
        return newID;
    }
    
    private void IntializePowerUps()
    {
        for (int i = 0; i < powerUpContainer.childCount; i++)
        {
            PowerUpNode node = powerUpContainer.GetChild(i).GetComponent<PowerUpNode>();
            
            string powerUpNodeID = GetPowerUpNodeID(i, node.BoostType);
            
            Debug.Log($"{powerUpContainer.GetChild(i).name} PowerUpNodeID: {powerUpNodeID}");
            //_powerUpNodes.Add(node);
        }

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
