using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class SpawingAreaData
{
    public string spawingAreaName;
    public Transform spawnTransform;
}

public class FoodTrashSpawnerManager : MonoBehaviour, IPausable
{
    public static FoodTrashSpawnerManager instance;

    [Header("Food/Trash Spawner Config")]
    [SerializeField] private List<SpawingAreaData> spawns = new List<SpawingAreaData>();
    [SerializeField] private Transform _foodTrashContiner;
    [SerializeField] private GameObject _prefabFoodTrash;
    [SerializeField] private float _spawnInterval;
    [SerializeField] private bool _isSpawning;


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

    public void OnPause()
    {
        _isSpawning = false;
    }

    public void OnResume()
    {
        _isSpawning = true;
        OnSpawningFood();
    }


    private void OnSpawningFood()
    {
        if (!_isSpawning)
            return;

        Debug.Log("[FoodTrashSpawnerManager - OnSpawningFood] Try to spawn food or trash");
        GameObject newFoodGO = Instantiate(_prefabFoodTrash, GetRandomSpawmPoint(),_foodTrashContiner);
        
        if (newFoodGO == null)
        {
            Debug.Log($"[FoodTrashSpawnerManager - OnSpawningFood] Food or Trash is NULL!");
            return;
        }

        Food newFood = newFoodGO.GetComponent<Food>();
        newFood.InitializeFood(WordLevel.hard);
    }

    private Transform GetRandomSpawmPoint()
    {
        int randomIndex = Random.Range(0, spawns.Count);

        Transform aviableSpawn = spawns[randomIndex].spawnTransform;

        return aviableSpawn;
    }
}
