using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "LevelDataSO", menuName = "Level Data/LevelDataSO")]
public class LevelDataSO : ScriptableObject
{
    public string levelName;
    public string levelDescription;
    public float durationLevelNode;
    public LevelNodeType levelType;
    public float currentFlowSpeed;

    [Header("Fish Spawn Table Data")] 
    public List<SpawnTableData> fishSpawnTable = new List<SpawnTableData>();
}
