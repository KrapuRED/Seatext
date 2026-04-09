using System.Collections.Generic;
using UnityEngine;

public enum WayPointPosition
{
    none,
    left,
    right, 
    top, 
    bottom
}

[System.Serializable]
public class SpawingAreaData
{
    public string spawingAreaName;
    public WayPointPosition position;
    public Transform spawnTransform;
}


public class SpawnerManager : MonoBehaviour, IPausable
{
    [Header("Spawner Config")]
    [SerializeField] private List<SpawingAreaData> spawnAreas = new List<SpawingAreaData>();
    [SerializeField] protected Transform _continer;
    [SerializeField] protected GameObject _prefab;
    [SerializeField] protected float _spawnInterval;
    [SerializeField] protected bool _isSpawning;
    [SerializeField] protected WordLevel wordLevel;

    private void Start()
    {
        PauseManager.instance.Register(this);
        Spawn();
    }

    public void OnPause()
    {
        _isSpawning = false;
    }

    public void OnResume()
    {
        _isSpawning = true;
        Spawn();
    }

    public virtual void Spawn()
    {
        Debug.Log("[SpawnerManager - Spawn] Spawning...");
    }

    public SpawingAreaData GetRandomSpawmPoint()
    {
        int randomIndex = Random.Range(0, spawnAreas.Count);

        //Debug.Log($"[FoodTrashSpawnerManager - GetRandomSpawmPoint] Random Index : {randomIndex} | Spawn Name : {spawnAreas[randomIndex].spawingAreaName}");
        SpawingAreaData aviableSpawn = spawnAreas[randomIndex];

        return aviableSpawn;
    }

    public Transform GetRandomEndWayPoint(SpawingAreaData startWayPoint)
    {

        List<SpawingAreaData> aviableEndWayPoints = new List<SpawingAreaData>();

        foreach (SpawingAreaData spawn in spawnAreas)
        {
            if (spawn.position != startWayPoint.position)
            {
                aviableEndWayPoints.Add(spawn);
            }
        }

        if (aviableEndWayPoints.Count <= 0)
            return GetRandomEndWayPoint(startWayPoint);

        int randomIndex = Random.Range(0, aviableEndWayPoints.Count);
        return aviableEndWayPoints[randomIndex].spawnTransform;
    }
}
