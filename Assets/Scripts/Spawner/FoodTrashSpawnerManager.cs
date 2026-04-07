using UnityEngine;
using System.Collections.Generic;

public class FoodTrashSpawnerManager : SpawnerManager
{
    public static FoodTrashSpawnerManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            PauseManager.instance.Register(this);
        }
        else
        {
            Debug.LogWarning("Multiple instances of FoodTrashSpawnerManager detected! Destroying duplicate.");
            Destroy(gameObject);
        }
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
