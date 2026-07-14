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

[System.Serializable]
public class FishSpawnChannel : SpawnChannel
{
    public FishSpawnerType type = FishSpawnerType.Passive;
}

[System.Serializable]
public class SpawnChannel
{
    public string channelName = "Channel";
    public float spawnInterval = 5f;
    public bool isEnabled = true;
 
    [NonSerialized] public Coroutine routine;
}

public abstract class SpawnerManager<TChannel> : MonoBehaviour, IPausable
    where TChannel : SpawnChannel
{
    [Header("Spawner Config")]
    [SerializeField] private List<SpawingAreaData> spawnAreas = new List<SpawingAreaData>();
    [SerializeField] protected Transform continerSpawning;
    [SerializeField] protected float spawnInterval;
    [SerializeField] protected WordLevel wordLevel;
    [SerializeField] private bool isIntilazeSpawnerByStart;

    [SerializeField] public List<TChannel> spawnChannels = new List<TChannel>();
    
    private Coroutine _spawnCoroutine;
    [SerializeField] protected bool _isSpawning;


    private void Start()
    {
        PauseManager.instance.Register(this);
        if (isIntilazeSpawnerByStart)
        {
            _isSpawning = true;
            OnStartSpawing();
        }

        InitializeSpawnwer(GameManager.instance.LevelDataSO.spawnerData);
    }

    public void OnPause()
    {
        Debug.Log("[SpawnerManager - OnPause] OnPause");
        _isSpawning = false;
 
        foreach (TChannel channel in spawnChannels)
        {
            if (channel.routine != null)
            {
                StopCoroutine(channel.routine);
                channel.routine = null;
            }
        }
    }

    public void OnResume()
    {
        _isSpawning = true;
        OnStartSpawing();
    }

    public void OnStartSpawing()
    {
        Debug.Log("[SpawnerManager - OnStartSpawing] OnStartSpawing");
        _isSpawning  = true;
        
        foreach (TChannel channel in spawnChannels)
        {
            if (!channel.isEnabled)
                continue;
 
            if (channel.routine == null)
                channel.routine = StartCoroutine(SpawingCoroutine(channel));
        }
    }

    public abstract void InitializeSpawnwer(SpawnerDataSO spawnerData);
    
    protected abstract void Spawn(TChannel channel);

    public SpawingAreaData GetRandomSpawmPoint()
    {
        int randomIndex = Random.Range(0, spawnAreas.Count);
        SpawingAreaData aviableSpawn = spawnAreas[randomIndex];
        return aviableSpawn;
    }

    private IEnumerator SpawingCoroutine(TChannel channel)
    {
        while (_isSpawning && channel.isEnabled)
        {
            yield return new WaitForSeconds(channel.spawnInterval);
            Spawn(channel);
        }
 
        channel.routine = null;
    }
}
