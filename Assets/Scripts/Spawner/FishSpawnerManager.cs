using System.Collections.Generic;
using UnityEngine;

public class FishSpawnerManager : SpawnerManager
{
    [Header("Fish Spawn Manager")]
    [SerializeField] private List<FishOS> fishDatas = new List<FishOS>();

    public override void Spawn()
    {
        if (!_isSpawning)
            return;

        Debug.Log("[FishSpawnerManager - Spawn] Try to spawn food or trash");

        SpawingAreaData spawingAreaData = GetRandomSpawmPoint();

        Transform spawnPos = spawingAreaData.spawnTransform;

        GameObject newFishGO = Instantiate(_prefab, spawnPos.position, Quaternion.identity, _continer);

        if (newFishGO == null)
        {
            Debug.Log($"[FishSpawnManager - OnSpawningFood] Food or Trash is NULL!");
            return;
        }

        EnemyFish enemyFish = newFishGO.GetComponent<EnemyFish>();
        enemyFish.IntilazeFish(GetRandomEndWayPoint(spawingAreaData), GetRandomDishData());
    }

    private FishOS GetRandomDishData()
    {
        int index = Random.Range(0, fishDatas.Count);

        FishOS data = fishDatas[index];

        return data;
    }

}
