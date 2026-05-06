using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum WayPointPosition
{
    none,
    left,
    right, 
    top, 
    bottom
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
    [SerializeField] private bool IntilazeSpawnerByStart;

    private Coroutine _spawnCoroutine;

    private void Start()
    {
        PauseManager.instance.Register(this);
        if (IntilazeSpawnerByStart)
            Spawn();

        OnStartSpawing();
    }

    public void OnPause()
    {
        Debug.Log("[SpawnerManager - OnPause] OnPause");
        _isSpawning = false;
        bool isCoroutineRunning = _spawnCoroutine != null;
    }

    public void OnResume()
    {
        _isSpawning = true;
        OnStartSpawing();
    }

    public void OnStartSpawing()
    {
        _spawnCoroutine = StartCoroutine(SpawingCoroutine());
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

    private IEnumerator SpawingCoroutine()
    {
        while (_isSpawning)
        {
            yield return new WaitForSeconds(_spawnInterval);
            Spawn();
        }
    }
}
