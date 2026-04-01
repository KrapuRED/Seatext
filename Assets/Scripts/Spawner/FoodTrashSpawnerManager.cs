using UnityEngine;

public class FoodTrashSpawnerManager : MonoBehaviour, IPausable
{
    public static FoodTrashSpawnerManager instance;


    [Header("Food/Trash Spawner Config")]
    [SerializeField] private Transform _foodTrashContiner;
    [SerializeField] private GameObject _prefabFoodTrash;
    [SerializeField] private float _spawnInterval;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("Multiple instances of FoodTrashSpawnerManager detected! Destroying duplicate.");
            Destroy(gameObject);
        }
    }

    public void OnPause()
    {
        throw new System.NotImplementedException();
    }

    public void OnResume()
    {
        throw new System.NotImplementedException();
    }


    private void StartSpawning()
    {
        GameObject newFood = Instantiate(_prefabFoodTrash, _foodTrashContiner);

    }
}
