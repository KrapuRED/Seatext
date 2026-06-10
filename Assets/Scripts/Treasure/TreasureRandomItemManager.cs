using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public enum TreasureRandomItemType
{
    Seacoene,
    AdaptPoint
}

[System.Serializable]
public class TreasureRandomItemConfig
{
    public int Amount;
    [Range(0, 100)]
    public int Chance;
}

[System.Serializable]
public class TreasureRandomItemData
{
    public string TreasureName;
    public TreasureRandomItemType ItemType;
    public int TreasureChange;

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
            cumulativeChance += treasureData.TreasureChange;
            if (roll < cumulativeChance)
            {
                return treasureData;
            }
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
            {
                return config;
            }
        }

        return null;
    }

    public void GetRandomTreasureItem()
    {
        if (this == null ) return; // safety check in case event fires after object is destroyed

        var treasureData = GetRandomTreasureData();

        if (treasureData == null)
        {
            Debug.LogWarning($"[TreasureRandomItemManager - GetRandomTreasureItem] No treasure data found!");
            return;
        }

        var treasureConfig = GetRandomTreasureConfig(treasureData);

        if (treasureConfig == null)
        {
            Debug.LogWarning($"[TreasureRandomItemManager - GetRandomTreasureItem] No treasure config found for {treasureData.TreasureName}!");
            return;
        }

        var newCurrencyData = new CurrecyData(treasureData.ItemType, treasureConfig.Amount);

        Debug.Log($"[TreasureRandomItemManager - GetRandomTreasureItem] Player received {treasureConfig.Amount} of {treasureData.ItemType} from {treasureData.TreasureName}!");

        PanelManager.instance.OpenPanelByTypePanel(PanelType.PanelNotifiication, newCurrencyData);

        GameEvents.OnSetCurrency.Invoke(newCurrencyData);
    }
}
