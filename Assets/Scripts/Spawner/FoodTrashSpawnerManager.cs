using UnityEngine;
using System.Collections.Generic;

public class FoodTrashSpawnerManager : SpawnerManager
{
    [Header("Food and Trash Spawner Config")]
    [SerializeField] private List<DropFoodSO> _foodDataList = new List<DropFoodSO>();

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
        newFood.InitializeFood(wordLevel, RandomFoodData());
    }

    private DropFoodSO RandomFoodData()
    {
        DropFoodSO randomFoodData = _foodDataList[Random.Range(0, _foodDataList.Count)];

        return randomFoodData;
    }
}
