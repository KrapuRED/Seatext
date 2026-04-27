using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnTableData
{
    public string fishName;
    public GameObject fishPrefab;
    [Range(0, 100)] public int spawnChance;
    public List<FishOS> fishDatas = new List<FishOS>();
}

public class FishSpawnerManager : SpawnerManager
{
    [Header("Fish Spawn Manager")]
    [SerializeField] private List<SpawnTableData> _spawnTableDatas = new List<SpawnTableData>();

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

        enemyFish.IntilazeFish(endWayPoint, GetRandomDishData(selectTable));
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

    private FishOS GetRandomDishData(SpawnTableData tableData)
    {
        int index = Random.Range(0, tableData.fishDatas.Count);

        FishOS data = tableData.fishDatas[index];

        return data;
    }

}
