using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public enum TreasureRandomItemType
{
    Currency,
    Item
}

[System.Serializable]
public enum ItemType
{
    FinItem,
    Potion,
    // add more...
}

[System.Serializable]
public abstract class BaseRewardData
{
    public abstract TreasureRandomItemType RewardType { get; }
    public abstract BaseRewardData Clone();
}

[System.Serializable]
public class CurrencyRewardData : BaseRewardData
{
    public override TreasureRandomItemType RewardType => TreasureRandomItemType.Currency;
    public CurrencyType CurrencyType;
    public int Amount;

    public override BaseRewardData Clone() => new CurrencyRewardData
    {
        CurrencyType = this.CurrencyType,
        Amount = this.Amount
    };
}

[System.Serializable]
public class ItemRewardData : BaseRewardData
{
    public override TreasureRandomItemType RewardType => TreasureRandomItemType.Item;
    public ItemType ItemType;
    public int Quantity;

    public override BaseRewardData Clone() => new ItemRewardData
    {
        ItemType = this.ItemType,
        Quantity = this.Quantity
    };
}

[System.Serializable]
public class TreasureRandomItemConfig
{
    public int Chance;
    [SerializeReference] public BaseRewardData Reward;
}

[System.Serializable]
public class TreasureRandomItemData
{
    public string TreasureName;
    [Range(0, 100)]
    public int TreasureChance;
    public List<TreasureRandomItemConfig> TreasureConfigs;
}

public class TreasureRandomItemManager : MonoBehaviour
{
    public static TreasureRandomItemManager Instance { get; private set; }

    [Header("Treasure Config")]
    [SerializeField] private List<TreasureRandomItemData> treasureRandomItemDataList =  new();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    #region Event Listeners

    private void OnEnable()
    {
        GameEvents.OnGetRandomTreasureItem.AddListener(GetRandomTreasureItem);
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
        GameEvents.OnGetRandomTreasureItem.RemoveListener(GetRandomTreasureItem);
    }

    #endregion

    private TreasureRandomItemData GetRandomTreasureData()
    {
        int roll = Random.Range(0, 100);
        int cumulativeChance = 0;

        foreach (var treasureData in treasureRandomItemDataList)
        {
            cumulativeChance += treasureData.TreasureChance;
            if (roll < cumulativeChance)
                return treasureData;
        }

        Debug.LogWarning($"[TreasureRandomItemManager - GetRandomTreasureData] No treasure data selected for roll: {roll}");
        return null;
    }

    private TreasureRandomItemConfig GetRandomTreasureConfig(TreasureRandomItemData treasureData)
    {
        int roll = Random.Range(0, 100);
        int cumulativeChance = 0;

        foreach (var config in treasureData.TreasureConfigs)
        {
            cumulativeChance += config.Chance;
            if (roll < cumulativeChance)
                return config;
        }

        Debug.LogWarning($"[TreasureRandomItemManager - GetRandomTreasureConfig] No config selected for roll: {roll}");
        return null;
    }

    private void HandleReward(BaseRewardData reward)
    {
        switch (reward.RewardType)
        {
            case TreasureRandomItemType.Currency:
                var currencyReward = reward as CurrencyRewardData;
                var currencyData = new CurrecyData(currencyReward.CurrencyType, currencyReward.Amount);
                GameEvents.OnSetCurrency.Invoke(currencyData);
                PanelManager.instance.OpenPanelByTypePanel(PanelType.PanelNotification, currencyData);
                break;

            case TreasureRandomItemType.Item:
                var itemReward = reward as ItemRewardData;
                // GameEvents.OnSetItem.Invoke(itemReward);
                // PanelManager.instance.OpenPanelByTypePanel(PanelType.PanelNotification, itemReward);
                break;
        }
    }

    public void GetRandomTreasureItem()
    {
        if (this == null) return;

        var treasureData = GetRandomTreasureData();
        if (treasureData == null)
        {
            Debug.LogWarning("[TreasureRandomItemManager - GetRandomTreasureItem] No treasure data found!");
            return;
        }

        var treasureConfig = GetRandomTreasureConfig(treasureData);
        if (treasureConfig == null)
        {
            Debug.LogWarning($"[TreasureRandomItemManager - GetRandomTreasureItem] No treasure config found for {treasureData.TreasureName}!");
            return;
        }

        Debug.Log($"[TreasureRandomItemManager - GetRandomTreasureItem] Reward from {treasureData.TreasureName}!");

        HandleReward(treasureConfig.Reward);
    }
}
