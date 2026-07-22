
using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class TrashSpawnTableData
{
    public string trashName;
    public GameObject trashPrefab;
    [Range(0, 100)] public int spawnChance;
    public DropFoodSO trashData;
}

public class FoodTrashSpawnerManager : SpawnerManager<SpawnChannel>
{
    [Header("Food and Trash Spawner Config")]
    [SerializeField] private int maxTrashAmount;
    [SerializeField] private List<TrashSpawnTableData> trashDataLists = new();

    private bool IsReachedMaxTrashAmount()
    {
        return continerSpawning.childCount >= maxTrashAmount;
    }

    public override void InitializeSpawnwer(SpawnerDataSO spawnerData)
    {
        spawnChannels.Clear();
        trashDataLists.Clear();

        if (spawnerData.TrashSpawnChannels == null || spawnerData.TrashSpawnChannels.Count == 0)
        {
            Debug.LogWarning("[FoodTrashSpawnerManager - Spawn] TrashSpawnChannels is NULL!");
            return;
        }
        
        spawnChannels  = new List<SpawnChannel>(spawnerData.TrashSpawnChannels);;
        trashDataLists = spawnerData.TrashTables;
        OnStartSpawing();
    }

    protected override void Spawn(SpawnChannel channel)
    {
        if (!_isSpawning)
            return;
 
        if (IsReachedMaxTrashAmount())
        {
            Debug.LogWarning($"[FoodTrashSpawnerManager - Spawn] ({channel.channelName}) Trash reached max amount of {maxTrashAmount}!");
            return;
        }
 
        SpawingAreaData spawingAreaData = GetRandomSpawmPoint();
        Transform spawnPos = spawingAreaData.spawnTransform;
 
        TrashSpawnTableData trashSpawnTableData = GetRandomTrashData();
 
        GameObject newFoodGO = Instantiate(trashSpawnTableData.trashPrefab, spawnPos.position, Quaternion.identity, continerSpawning);
 
        if (newFoodGO == null)
        {
            Debug.Log("[FoodTrashSpawnerManager - Spawn] Food or Trash is NULL!");
            return;
        }
 
        Food newFood = newFoodGO.GetComponent<Food>();
        newFood.InitializeFood(wordLevel, trashSpawnTableData.trashData);
    }

    private TrashSpawnTableData GetRandomTrashData()
    {
        TrashSpawnTableData trashSpawnTableData = trashDataLists[Random.Range(0, trashDataLists.Count)];

        return trashSpawnTableData;
    }
}
