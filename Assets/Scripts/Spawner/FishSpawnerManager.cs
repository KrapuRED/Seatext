using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnTableData
{
    public string fishName;
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

public class FishSpawnerManager : SpawnerManager
{
    [Header("Spawnable Fish Data")]
    [SerializeField] private List<SpawnTableData> _spawnTableDatas = new List<SpawnTableData>();

    [Header("Spawned Fish Pool")]
    [SerializeField] private List<spawnedFishData> _spawnedFishDatas = new List<spawnedFishData>();

    private void OnEnable()
    {
        GameEvents.OnRemoveSpawnedFishData.AddListener(RemoveSpawnedFishData);
    }
    
    private void OnDisable()
    {
        GameEvents.OnRemoveSpawnedFishData.RemoveListener(RemoveSpawnedFishData);
    }

    public override void Spawn()
    {
        if (!_isSpawning)
            return;

        int totalChance = 0;

        foreach (SpawnTableData spawnTableData in _spawnTableDatas)
        {
            totalChance += spawnTableData.spawnChance;
        }

        if (totalChance > 100)
        {
            Debug.LogWarning($"[FishSpawnerManager - OnSpawing] Total Chance is more than 100%!");
            return;
        }

        SpawnTableData selectTable = GetRandomSpawnTable();
        if (selectTable == null)
        {
            Debug.LogWarning($"[FishSpawnerManager - OnSpawing] No Spawn Table is selected!");
            return;
        }

        SpawingAreaData spawingAreaData = WaypointManager.Instance.GetRandomFishSpawnPoint();
        Transform spawnPos = spawingAreaData.spawnTransform;

        GameObject newFishGO = Instantiate(selectTable.fishPrefab, spawnPos.position, Quaternion.identity, _continer);

        if (newFishGO == null)
        {
            Debug.Log($"[FishSpawnManager - OnSpawningFish] Fish is NULL!");
            return;
        }

        EnemyFish enemyFish = newFishGO.GetComponent<EnemyFish>();
        Transform endWayPoint = WaypointManager.Instance.GetRandomEndWayPoint(spawingAreaData);

        FishSO fishData = GetRandomDishData(selectTable);
        int fishDataIndex = AddSpawnedFishData(fishData);

        enemyFish.IntilazeFish(endWayPoint, fishData, fishDataIndex);
    }

    private SpawnTableData GetRandomSpawnTable()
    {
        int roll = Random.Range(0, 100);
        int cumulativeChance = 0;

        foreach (var table in _spawnTableDatas)
        {
            cumulativeChance += table.spawnChance;
            if (roll < cumulativeChance)
            {
                return table;
            }
        }

        return null;
    }

    private FishSO GetRandomDishData(SpawnTableData tableData)
    {
        int index = Random.Range(0, tableData.fishDatas.Count);

        FishSO data = tableData.fishDatas[index];

        return data;
    }

    private int FindSpawnedFishDataIndex()
    {
        int count = _spawnedFishDatas.Count;
        for (int i = 0; i < count; i++)
        {
            if (_spawnedFishDatas[i].fishData == null)
            {
                return i;
            }
        }
        return count;
    }

    private bool CheckSpawnedFishData(int  listIndex)
    {
        if (_spawnedFishDatas.Count <= 0)
            return false;

        if (listIndex >= _spawnedFishDatas.Count)
            return false;
            
        return _spawnedFishDatas[listIndex].fishData == null;
    }
    
    private int AddSpawnedFishData(FishSO fishData)
    {
        int index = FindSpawnedFishDataIndex();
        
        if (index == -1)
        {
            Debug.LogError($"[FishSpawnerManager - AddSpawnedFishData] No empty slot for spawned fish data!");
            return -1;
        }

        if (CheckSpawnedFishData(index))
        {
            _spawnedFishDatas[index].fishData = fishData;
        }
        else
        {
            _spawnedFishDatas.Add(
                new spawnedFishData
                {
                    fishData  = fishData,
                    fishIndex = index
                });
        }
        
        return index;
    }
    
    private void RemoveSpawnedFishData(int fishIndex)
    {
        for (int i = 0; i < _spawnedFishDatas.Count; i++)
        {
            if (_spawnedFishDatas[i].fishIndex == fishIndex)
            {
                _spawnedFishDatas[i].fishData = null;
                break;
            }
        }
    }
}
