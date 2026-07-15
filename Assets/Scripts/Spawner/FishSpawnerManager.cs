using System.Collections.Generic;
using UnityEngine;

public enum FishSpawnerType
{
    None,
    Passive,
    Active
}

[System.Serializable]
public class FishSpawnTableData
{
    public string fishName;
    public FishSpawnerType type;
    public GameObject fishPrefab;
    [Range(0, 100)] public int spawnChance;
    public List<FishSO> fishDatas = new List<FishSO>();
}

[System.Serializable]
public class spawnedFishData
{
    public FishSO fishData;
    public int fishIndex;
}

public class FishSpawnerManager : SpawnerManager<FishSpawnChannel>
{
    [Header("Spawnable Fish Data")]
    [SerializeField] private List<FishSpawnTableData> passiveFishSpawnTableDatas = new List<FishSpawnTableData>();
    [SerializeField] private List<FishSpawnTableData> activeFishSpawnTableDatas = new List<FishSpawnTableData>();
 
    [Header("Spawned Fish Pool")]
    [SerializeField] private List<spawnedFishData> spawnedPassiveFishDatas = new List<spawnedFishData>();
    [SerializeField] private List<spawnedFishData> spawnedActiveFishDatas = new List<spawnedFishData>();
    
    private void OnEnable()
    {
        GameEvents.OnEndDuration.AddListener(OnPause);
        GameEvents.OnRemoveSpawnedFishData.AddListener(RemoveSpawnedFishData);
    }
    
    private void OnDisable()
    {
        GameEvents.OnEndDuration.RemoveListener(OnPause);
        GameEvents.OnRemoveSpawnedFishData.RemoveListener(RemoveSpawnedFishData);
    }

    public override void InitializeSpawnwer(SpawnerDataSO spawnerData)
    {
        spawnChannels.Clear();
        spawnedPassiveFishDatas.Clear();
        spawnedActiveFishDatas.Clear();
        
        spawnChannels = spawnChannels = new List<FishSpawnChannel>(spawnerData.FishSpawnChannels);;
        passiveFishSpawnTableDatas = spawnerData.PassiveFishSpawnTables;
        activeFishSpawnTableDatas = spawnerData.ActiveFishSpawnTables;
        
        OnStartSpawing();
    }

    protected override void Spawn(FishSpawnChannel channel)
    {
        List<FishSpawnTableData> tableList = GetTableList(channel.type);
        List<spawnedFishData> spawnedList = GetSpawnedList(channel.type);
 
        int totalChance = 0;
        foreach (FishSpawnTableData spawnTableData in tableList)
            totalChance += spawnTableData.spawnChance;
 
        if (totalChance > 100)
        {
            Debug.LogWarning($"[FishSpawnerManager - Spawn] ({channel.channelName}/{channel.type}) Total Chance is more than 100%!");
            return;
        }
 
        FishSpawnTableData selectTable = GetRandomSpawnTable(tableList);
        if (selectTable == null)
        {
            Debug.LogWarning($"[FishSpawnerManager - Spawn] ({channel.channelName}/{channel.type}) No Spawn Table is selected!");
            return;
        }
 
        SpawingAreaData spawingAreaData = WaypointManager.Instance.GetRandomFishSpawnPoint();
        Transform spawnPos = spawingAreaData.spawnTransform;
 
        GameObject newFishGO = Instantiate(selectTable.fishPrefab, spawnPos.position, Quaternion.identity, continerSpawning);
        if (newFishGO == null)
        {
            Debug.Log("[FishSpawnerManager - Spawn] Fish is NULL!");
            return;
        }
 
        EnemyFish enemyFish = newFishGO.GetComponent<EnemyFish>();
        Transform endWayPoint = WaypointManager.Instance.GetRandomEndWayPoint(spawingAreaData);
 
        FishSO fishData = GetRandomFishData(selectTable);
        int fishDataIndex = AddSpawnedFishData(spawnedList, fishData);
 
        // NOTE: EnemyFish needs to remember `channel.type` so it can pass it back
        // when it eventually raises GameEvents.OnRemoveSpawnedFishData(type, index).
        enemyFish.IntilazeFish(endWayPoint,selectTable.type , fishData, fishDataIndex);
    }
 
    private List<FishSpawnTableData> GetTableList(FishSpawnerType type)
    {
        return type == FishSpawnerType.Active ? activeFishSpawnTableDatas : passiveFishSpawnTableDatas;
    }
 
    private List<spawnedFishData> GetSpawnedList(FishSpawnerType type)
    {
        return type == FishSpawnerType.Active ? spawnedActiveFishDatas : spawnedPassiveFishDatas;
    }
 
    private FishSpawnTableData GetRandomSpawnTable(List<FishSpawnTableData> tableList)
    {
        int roll = Random.Range(0, 100);
        int cumulativeChance = 0;
 
        foreach (var table in tableList)
        {
            cumulativeChance += table.spawnChance;
            if (roll < cumulativeChance)
                return table;
        }
 
        return null;
    }
 
    private FishSO GetRandomFishData(FishSpawnTableData tableData)
    {
        int index = Random.Range(0, tableData.fishDatas.Count);
        return tableData.fishDatas[index];
    }
 
    private int FindSpawnedFishDataIndex(List<spawnedFishData> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].fishData == null)
                return i;
        }
        return list.Count;
    }
 
    private bool CheckSpawnedFishSlotFree(List<spawnedFishData> list, int listIndex)
    {
        if (list.Count <= 0 || listIndex >= list.Count)
            return false;
 
        return list[listIndex].fishData == null;
    }
 
    private int AddSpawnedFishData(List<spawnedFishData> list, FishSO fishData)
    {
        int index = FindSpawnedFishDataIndex(list);
 
        if (CheckSpawnedFishSlotFree(list, index))
        {
            list[index].fishData = fishData;
        }
        else
        {
            list.Add(new spawnedFishData
            {
                fishData = fishData,
                fishIndex = index
            });
        }
 
        return index;
    }
 
    // Signature includes the type, since Passive index 0 and Active index 0
    // are two different slots. GameEvents.OnRemoveSpawnedFishData (and whatever
    // calls it from EnemyFish) needs to pass the FishSpawnerType along.
    private void RemoveSpawnedFishData(FishSpawnerType type, int fishIndex)
    {
        List<spawnedFishData> list = GetSpawnedList(type);
 
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].fishIndex == fishIndex)
            {
                list[i].fishData = null;
                break;
            }
        }
    }
}
