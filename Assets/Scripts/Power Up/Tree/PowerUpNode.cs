using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class PowerUpNode : MonoBehaviour
{
    [SerializeField] private BoostType boostType;
    [SerializeField] private PowerUpSO powerUpSO;
    [SerializeField] private string _powerUpNodeID;
    
    [Header("System Power Up")]
    [SerializeField] private Image powerUpIcon;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private PowerUpTypeBox powerUpTypeBox;

    [Header("Power Up Information")]
    [SerializeField] private TMP_Text powerUpNameBox;
    [SerializeField] private TMP_Text powerUpDescriptionBox;
    [SerializeField] private TMP_Text powerUpCostBox;
    
    [SerializeField] private bool isUnlock;
    [SerializeField] private bool canBuy;
    
    
    public BoostType BoostType => boostType;
    public bool CanBuy => canBuy;

    private void Start()
    {
        powerUpIcon.sprite = powerUpSO.lockIcon;
    }

    #region Event

    private void OnEnable()
    {
        GameEvents.OnUpdatePowerUpNode.AddListener(RefreshPowerUpNode);
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
        GameEvents.OnUpdatePowerUpNode.RemoveListener(RefreshPowerUpNode);
    }
    
    #endregion
    
    private List<string> GetPrerequisiteIDs()
    {
        List<string> prereqs = new List<string>();

        // The root node has zero prerequisites, always purchasable first
        if (_powerUpNodeID == "All_0")
            return prereqs;

        string[] parts = _powerUpNodeID.Split('_');
        string prefix = parts[0];
        int index = int.Parse(parts[1]);

        if (boostType == BoostType.All)
        {
            prereqs.Add("All_0");
            if (index > 0)
                prereqs.Add($"{prefix}_{index - 1}");
        }
        else
        {
            prereqs.Add("All_0");

            if (index > 0)
                prereqs.Add($"{prefix}_{index - 1}");
        }

        return prereqs;
    }

    private bool CheckPrerequisites()
    {
        if (powerUpSO == null)
        {
            Debug.LogError($"[PowerUpNode] {_powerUpNodeID} has no PowerUpSO assigned!");
            return false;
        }

        foreach (var prereqID in GetPrerequisiteIDs())
        {
            if (!PowerUpManager.instance.PowerUpExists(prereqID))
                return false;
        }

        return true;
    }

    private void ShowPowerUpInfo()
    {
        if (powerUpNameBox == null || powerUpCostBox == null || powerUpCostBox == null)
        {
            Debug.LogError($"Null is power up info!");
            return;
        }
        
        powerUpNameBox.text = powerUpSO.powerUpName;
        powerUpDescriptionBox.text = powerUpSO.powerUpDescription;
        powerUpCostBox.text = $"Cost AP {powerUpSO.powerUpCost.ToString()}";
    }
    
    public void InitializePowerUpNode(string powerUpNodeID)
    {
        _powerUpNodeID = powerUpNodeID;

        bool alreadyUnlocked = PowerUpManager.instance.PowerUpExists(_powerUpNodeID);
        isUnlock = alreadyUnlocked; // sync local state from the persistent manager

        if (!alreadyUnlocked)
        {
            ShowPowerUpInfo();
        }

        RefreshUnlockState(); // always refresh visuals, regardless of unlock state
    }

    private void RefreshUnlockState()
    {
        canBuy = CheckPrerequisites();
        
        Debug.Log($"[PowerUpNode] {_powerUpNodeID}: canBuy={canBuy}, isUnlock={isUnlock}, canvasGroup={(canvasGroup != null ? "assigned" : "NULL")}");
        
        if (canvasGroup != null)
        {
            if (!isUnlock)
            {
                canvasGroup.alpha = canBuy ? 1f : 0f;
                powerUpIcon.sprite = canBuy ? powerUpSO.selectedIcon :  powerUpSO.lockIcon;
            }
            else
                canvasGroup.alpha = 0f;
        }

        WordData wordData = WordBankManager.instance.GetRandomWordData(WordLevel.easy);
        
        powerUpTypeBox.SetTextToType(wordData.word);
    }

    public void BuyPowerUpNode()
    {
        Debug.Log($"[PowerUpNode] {_powerUpNodeID}: BuyPowerUpNode");
        
        if (isUnlock)
        {
            Debug.Log($"{_powerUpNodeID} is already unlocked.");
            return;
        }

        if (!CheckPrerequisites())
        {
            Debug.LogWarning($"{_powerUpNodeID} cannot be unlocked — missing prerequisites.");
            return;
        }

        if (!CurrencyManager.instance.IsSufficientCurrecny(powerUpSO.currencyType, powerUpSO.powerUpCost))
        {
            return;
        }

        CurrencyManager.instance.UseCurrency(powerUpSO.currencyType, powerUpSO.powerUpCost);
        
        isUnlock = true;
        powerUpIcon.sprite = powerUpSO.unlockIcon;
        PowerUpManager.instance.UnlockPowerUpNode(_powerUpNodeID, powerUpSO);

        canvasGroup.alpha = 0;
    }
    
    private void RefreshPowerUpNode() => RefreshUnlockState();
}
