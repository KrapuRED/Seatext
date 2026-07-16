using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CurrecyData
{
    public CurrencyType currencyType;
    public int Amount;

    public CurrecyData(CurrencyType currencyType, int amount)
    {
        this.currencyType = currencyType;
        Amount = amount;
    }

    public CurrecyData(CurrecyData data)
    {
        this.currencyType = data.currencyType;
        Amount = data.Amount;
    }
}

[System.Serializable]
public enum CurrencyType
{
    Seacoene,
    AdaptPoint
}

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager instance {get; private set; }

    [Header("Currency Data")]
    [SerializeField] private List<CurrecyData> initialCurrencyData = new(); // set default values in Inspector

    private Dictionary<CurrencyType, int> currencyValues = new();

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
        }

        InitializeCurrency();
    }

    private void InitializeCurrency()
    {
        // Auto-register ALL enum types with 0 first
        foreach (CurrencyType type in Enum.GetValues(typeof(CurrencyType)))
        {
            currencyValues[type] = 0;
        }

        // Override with Inspector values if set
        foreach (var currencyData in initialCurrencyData)
        {
            currencyValues[currencyData.currencyType] = currencyData.Amount;
        }
    }


    #region Event Listeners

    private void OnEnable()
    {
        GameEvents.OnShowUI.AddListener(ShowCurrency);

        GameEvents.OnSetCurrency.AddListener(UpdateCurrency);
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
        GameEvents.OnShowUI.RemoveListener(ShowCurrency);

        GameEvents.OnSetCurrency.RemoveListener(UpdateCurrency);
    }

    #endregion

    private void Start()
    {
        // Notify UI of initial values
        foreach (var kvp in currencyValues)
        {
            GameEvents.OnUpdateCurrecyUI.Invoke(new CurrecyData(kvp.Key, kvp.Value));
        }
    }

    public void ShowCurrency()
    {
        if (this == null) return;

        int amount = currencyValues[CurrencyType.Seacoene];

        var updatedData = new CurrecyData(CurrencyType.Seacoene, amount);
        GameEvents.OnUpdateCurrecyUI.Invoke(updatedData);
    }

    public bool IsSufficientCurrecny(CurrencyType currencyType, int costValue)
    {
        if (!currencyValues.ContainsKey(currencyType))
        {
            Debug.LogWarning($"[CurrencyManager] Unknown currency type: {currencyType}");
            return false;
        }

        int amaount = currencyValues[currencyType];

        return amaount >= costValue;
    }

    public void UseCurrency(CurrencyType currencyType, int costValue)
    {
        if (!currencyValues.ContainsKey(currencyType))
        {
            Debug.LogWarning($"[CurrencyManager] Unknown currency type: {currencyType}");
            return;
        }
        
        int amount = currencyValues[currencyType] -= costValue;

        var updatedData = new CurrecyData(currencyType, amount);
        GameEvents.OnUpdateCurrecyUI.Invoke(updatedData);

        Debug.Log($"Amount left {currencyType} : {currencyValues[currencyType]}");
    }

    public void UpdateCurrency(CurrecyData currencyData)
    {
        if (!currencyValues.ContainsKey(currencyData.currencyType))
        {
            Debug.LogWarning($"[CurrencyManager] Unknown currency type: {currencyData.currencyType}");
            return;
        }

        currencyValues[currencyData.currencyType] += currencyData.Amount;

        var updatedData = new CurrecyData(currencyData.currencyType, currencyValues[currencyData.currencyType]);
        GameEvents.OnUpdateCurrecyUI.Invoke(updatedData);
    }

    public int GetCurrencyValue(CurrencyType type)
    {
        return currencyValues.TryGetValue(type, out int value) ? value : 0;
    }
}
