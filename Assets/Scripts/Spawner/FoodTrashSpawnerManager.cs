using UnityEngine;
using System.Collections.Generic;

public class FoodTrashSpawnerManager : SpawnerManager
{

    private void Start()
    {
        Spawn();
    }

    public override void Spawn()
    {
        if (!_isSpawning)
            return;

        Debug.Log("[FoodTrashSpawnerManager - OnSpawningFood] Try to spawn food or trash");

        SpawingAreaData spawingAreaData = GetRandomSpawmPoint();
        Transform spawnPos = spawingAreaData.spawnTransform;

        GameObject newFoodGO = Instantiate(_prefab, spawnPos.position, Quaternion.identity, _continer);

        if (newFoodGO == null)
        {
            Debug.Log($"[FoodTrashSpawnerManager - OnSpawningFood] Food or Trash is NULL!");
            return;
        }

        Food newFood = newFoodGO.GetComponent<Food>();
        newFood.InitializeFood(wordLevel);
    }
}
