using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "LevelDataSO", menuName = "Level Data/LevelDataSO")]
public class LevelDataSO : ScriptableObject
{
    public string levelDataID;
    public string levelDataName;
    public string levelDescription;
    public float durationLevelDataNode;
    public float currentFlowSpeed;

    [Header("Fish Spawn Table Data")] 
    public List<SpawnTableData> fishSpawnTable = new List<SpawnTableData>();
}
