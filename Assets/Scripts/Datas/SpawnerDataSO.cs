using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "SpawnerDataSO", menuName = "Spawner Data/SpawnerDataSO")]
public class SpawnerDataSO : ScriptableObject
{
    public string SpawnerDataName;
    public string SpawnerDataID;

    [Header("Spawner Data for FISH")] 
    public List<FishSpawnChannel> FishSpawnChannels = new();
    public List<FishSpawnTableData>  PassiveFishSpawnTables = new();
    public List<FishSpawnTableData>  ActiveFishSpawnTables = new();
    
    [Header("Spawner Data for TRASH")]
    public List<SpawnChannel> TrashSpawnChannels = new();
    public List<TrashSpawnTableData>  TrashTables = new();
}
