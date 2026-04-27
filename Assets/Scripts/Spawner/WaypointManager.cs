using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class SpawingAreaData
{
    public string spawingAreaName;
    public WayPointPosition position;
    public Transform spawnTransform;
}


public class WaypointManager : MonoBehaviour
{
    public static WaypointManager Instance { get; private set; }

    [Header("RoamingPoint Manager Config")]
    [SerializeField] private List<SpawingAreaData> _spawingFishAreaDatas = new List<SpawingAreaData>();
    [SerializeField] private List<SpawingAreaData> _spawingTrashAreaDatas = new List<SpawingAreaData>();
    [SerializeField] private List<SpawingAreaData> _waypointRoaming = new List<SpawingAreaData>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public SpawingAreaData GetRandomFishSpawnPoint()
    {
        if (_spawingFishAreaDatas.Count == 0)
        {
            Debug.LogWarning($"[WaypointManager - GetRandomFishSpawnPoint] No Fish Spawn Point is available!");
            return null;
        }

        int randomIndex = Random.Range(0, _spawingFishAreaDatas.Count);
        return _spawingFishAreaDatas[randomIndex];
    }

    public Transform GetRandomEndWayPoint(SpawingAreaData startWayPoint)
    {

        List<SpawingAreaData> aviableEndWayPoints = new List<SpawingAreaData>();

        foreach (SpawingAreaData spawn in _spawingFishAreaDatas)
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

    public SpawingAreaData GetRandomTrashSpawnPoint()
    {
        if (_spawingTrashAreaDatas.Count == 0)
        {
            Debug.LogWarning($"[WaypointManager - GetRandomTrashSpawnPoint] No Trash Spawn Point is available!");
            return null;
        }

        int randomIndex = Random.Range(0, _spawingTrashAreaDatas.Count);
        return _spawingTrashAreaDatas[randomIndex];
    }

    public SpawingAreaData GetRandomRoamingPoint()
    {
        if (_waypointRoaming.Count == 0)
        {
            Debug.LogWarning($"[WaypointManager - GetRandomRoamingPoint] No Roaming Point is available!");
            return null;
        }

        int randomIndex = Random.Range(0, _waypointRoaming.Count);
        return _waypointRoaming[randomIndex];
    }
}
