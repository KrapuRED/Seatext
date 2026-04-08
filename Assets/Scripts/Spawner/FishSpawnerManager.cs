using System.Collections.Generic;
using UnityEngine;

public class FishSpawnerManager : SpawnerManager
{
    private void Start()
    {
        Spawn();
    }

    public override void Spawn()
    {
        if (!_isSpawning)
            return;

        Debug.Log("[FishSpawnerManager - Spawn] Try to spawn food or trash");

        SpawingAreaData spawingAreaData = GetRandomSpawmPoint();

        Transform spawnPos = spawingAreaData.spawnTransform;

        GameObject newFoodGO = Instantiate(_prefab, spawnPos.position, Quaternion.identity, _continer);

        if (newFoodGO == null)
        {
            Debug.Log($"[FishSpawnManager - OnSpawningFood] Food or Trash is NULL!");
            return;
        }

        EnemyFish enemyFish = newFoodGO.GetComponent<EnemyFish>();
        enemyFish.IntilazeFish(GetRandomEndWayPoint(spawingAreaData));
    }

}
