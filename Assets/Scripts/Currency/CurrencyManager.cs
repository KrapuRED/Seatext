using System;
using UnityEngine;

[System.Serializable]
public class CurrecyData
{
    public TreasureRandomItemType CurrencyType;
    public int Amount;

    public CurrecyData(TreasureRandomItemType itemType, int amount)
    {
        CurrencyType = itemType;
        Amount = amount;
    }
}

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager instance {get; private set; }

    [Header("Currency Data")] 
    [SerializeField] private int seaCoinValue;
    [SerializeField] private int adaptPoint;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
        }
    }

    #region Event Listeners

    private void OnEnable()
    {
        GameEvents.OnSetCurrency.AddListener(SetCurrency);
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
        GameEvents.OnSetCurrency.RemoveListener(SetCurrency);
    }

    #endregion

    public void UseCurrency(TreasureRandomItemType typeCurrecny)
    {
        
    }

    public void SetCurrency(CurrecyData currencyData)
    {
        switch (currencyData.CurrencyType)
        {
            case TreasureRandomItemType.Seacoene:
                seaCoinValue += currencyData.Amount;
                break;

            case TreasureRandomItemType.AdaptPoint:
                adaptPoint += currencyData.Amount;
                break;

            default:
                Debug.LogWarning($"[CurrencyManager - SetCurrency] Unhandled currency type: {currencyData.CurrencyType}");
                break;
        }
    }
}
