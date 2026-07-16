using System;
using UnityEngine;
using System.Collections.Generic;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

[System.Serializable]
public enum BoostType
{
    None,
    All,
    Speed,
    Health,
    Hunger
}

[System.Serializable]
public class PowerUpNodeData
{
    public string powerUpNodeName;
    public BoostType boostType;
    public List<PowerUpSO> powerUpDatas = new();
}

public class PowerUpManager : MonoBehaviour
{
    public static PowerUpManager instance {get; private set;}
    
    [Header("PowerUp Settings")]
    [SerializeField] private List<PowerUpNodeData> activePowerUpDatas = new ();
    
    [Header("PowerUp Active")]
    [SerializeField] private float speedBoost;  // increase speed
    [SerializeField] private float healthBoost; // increase maxHealth
    [SerializeField] private float hungerBoost; // decrease hunger
    
    [Header("TEST")]
    public List<PowerUpSO> TEST_PowerUpDatas = new();
    
    private Dictionary<string, PowerUpNode> _powerUpNodes = new();
    private Dictionary<BoostType, int> _typeCounters = new();
    private StatusPlayerManager _statusPlayerManager;
    private HashSet<string> _unlockedPowerUpIDs = new();
    
    public float SpeedBoost => speedBoost;
    public float HealthBoost => healthBoost;
    public float HungerBoost => hungerBoost;
    
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else 
            Destroy(gameObject);

        _statusPlayerManager = StatusPlayerManager.Instance;
    }

    private void Start()
    {
        activePowerUpDatas.Clear();

        InitializeActivePowerUpSlots();

        foreach (var powerUp in TEST_PowerUpDatas )
        {
            AddPowerUp(powerUp);
        }
    }

    private void InitializeActivePowerUpSlots()
    {
        foreach (BoostType type in Enum.GetValues(typeof(BoostType)))
        {
            if (type == BoostType.None)
                continue;

            activePowerUpDatas.Add(new PowerUpNodeData
            {
                powerUpNodeName = type.ToString(),
                boostType = type,
                powerUpDatas = new List<PowerUpSO>()
            });
        }
    }

    private string GetPowerUpNodeID(int intialID, BoostType boostType)
    {
        string newID =string.Empty;
        
        if (!_typeCounters.ContainsKey(boostType))
            _typeCounters[boostType] = 0;

        int index = _typeCounters[boostType];
        _typeCounters[boostType]++;
        
        string boostID = boostType switch
        {
            BoostType.All => "All",
            BoostType.Health => "HP",
            BoostType.Hunger => "HG",
            BoostType.Speed => "SP",
            _ => "NONE"
        };
        
        newID = $"{boostID}_{index}";
        
        return newID;
    }

    private string GetBoostPrefix(BoostType boostType)
    {
        return boostType switch
        {
            BoostType.All    => "All",
            BoostType.Health => "HP",
            BoostType.Hunger => "HG",
            BoostType.Speed  => "SP",
            _ => "NONE"
        };
    }
    
    private void ReIntializePowerUps(Transform powerUpContainer)
    {
        Dictionary<BoostType, int> localCounters = new();

        for (int i = 0; i < powerUpContainer.childCount; i++)
        {
            PowerUpNode node = powerUpContainer.GetChild(i).GetComponent<PowerUpNode>();

            if (node == null)
                continue;

            if (!localCounters.ContainsKey(node.BoostType))
                localCounters[node.BoostType] = 0;

            int index = localCounters[node.BoostType];
            localCounters[node.BoostType]++;

            string powerUpNodeID = $"{GetBoostPrefix(node.BoostType)}_{index}";

            if (_powerUpNodes.ContainsKey(powerUpNodeID))
            {
                // Same ID as before — just point it at the new instance
                _powerUpNodes[powerUpNodeID] = node;
            }
            else
            {
                // Container has more nodes than last time — treat as new
                _powerUpNodes.Add(powerUpNodeID, node);
            }

            node.InitializePowerUpNode(powerUpNodeID);

            Debug.Log($"Re-initialized {powerUpContainer.GetChild(i).name} PowerUpNodeID: {powerUpNodeID}");
        }
    }
    
    public void IntializePowerUps(Transform powerUpContainer)
    {
        if (_powerUpNodes.Count > 0)
        {
            ReIntializePowerUps(powerUpContainer);
            return;
        }
        
        PowerUpNode previousOfType = null;
        Dictionary<BoostType, PowerUpNode> lastNodeByType = new();

        for (int i = 0; i < powerUpContainer.childCount; i++)
        {
            PowerUpNode node = powerUpContainer.GetChild(i).GetComponent<PowerUpNode>();

            if (node == null)
                continue;

            string powerUpNodeID = GetPowerUpNodeID(i, node.BoostType);

            // Chain: this node's prerequisite is the last node of the same type
            lastNodeByType.TryGetValue(node.BoostType, out previousOfType);

            node.InitializePowerUpNode(powerUpNodeID);

            lastNodeByType[node.BoostType] = node;
            _powerUpNodes.Add(powerUpNodeID, node);
        }
    }

    private void ApplyBooster(BoostType boostType)
    {
        switch (boostType)
        {
            case BoostType.Health:
                _statusPlayerManager.UpgradeMaxHealth(healthBoost);
                break;
            case BoostType.Hunger:
                _statusPlayerManager.UpgradeMaxHunger(hungerBoost);
                break;
            case BoostType.All:
                _statusPlayerManager.UpgradeMaxHealth(healthBoost);
                _statusPlayerManager.UpgradeMaxHunger(hungerBoost);
                break;
        }
    }
    
    public void AddPowerUp(PowerUpSO powerUpData)
    {
        if (powerUpData == null)
        {
            Debug.LogWarning("[PowerUpManager] AddPowerUp called with null PowerUpSO");
            return;
        }

        if (powerUpData.powerUpBoostType == BoostType.All)
        {
            // 1) Applies to all stats — add to the "All" bucket
            var allData = activePowerUpDatas.Find(d => d.boostType == BoostType.All);
            allData?.powerUpDatas.Add(powerUpData);
        }
        else
        {
            // 2) Add to the specific matching boost type bucket
            var matchingData = activePowerUpDatas.Find(d => d.boostType == powerUpData.powerUpBoostType);

            if (matchingData == null)
            {
                Debug.LogWarning($"[PowerUpManager] No active slot found for {powerUpData.powerUpBoostType}");
                return;
            }

            matchingData.powerUpDatas.Add(powerUpData);
        }

        // 3) Recalculate totals
        RecalculateBoosts();
        
        // 4) Applay to status
        ApplyBooster(powerUpData.powerUpBoostType);
            
    }

    private void RecalculateBoosts()
    {
        speedBoost = 0f;
        healthBoost = 0f;
        hungerBoost = 0f;

        var allBoosts = activePowerUpDatas.Find(d => d.boostType == BoostType.All);
        float allBonus = allBoosts != null ? SumBoostValues(allBoosts.powerUpDatas) : 0f;

        foreach (var data in activePowerUpDatas)
        {
            float sum = SumBoostValues(data.powerUpDatas) + allBonus;

            switch (data.boostType)
            {
                case BoostType.Speed:
                    speedBoost = sum;
                    break;
                case BoostType.Health:
                    healthBoost = sum;
                    break;
                case BoostType.Hunger:
                    hungerBoost = sum;
                    break;
            }
        } 
    }

    private float SumBoostValues(List<PowerUpSO> powerUps)
    {
        float total = 0f;
        foreach (var p in powerUps)
            total += p.valuePowerUp; // <-- assumption, see below
        return total;
    }

    public bool PowerUpExists(string powerUpID)
    {
        return _unlockedPowerUpIDs.Contains(powerUpID);
    }
    
    public void UnlockPowerUpNode(string powerUpID, PowerUpSO powerUpData)
    {
        if (_unlockedPowerUpIDs.Contains(powerUpID))
        {
            Debug.LogWarning($"[PowerUpManager] {powerUpID} is already unlocked.");
            return;
        }
        
        _unlockedPowerUpIDs.Add(powerUpID);
        AddPowerUp(powerUpData);
        
        //Update all Power Up UI
        GameEvents.OnUpdatePowerUpNode.Invoke();
    }
}
