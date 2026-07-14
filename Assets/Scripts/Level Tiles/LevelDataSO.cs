using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "LevelDataSO", menuName = "Level Data/LevelDataSO")]
public class LevelDataSO : ScriptableObject, IPanelDisplayable
{
    public string levelDataID;
    public string levelDataName;
    public string levelDescription;
    public float durationLevelDataNode;
    public float currentFlowSpeed;
    public Sprite environmentSprite;

    [Header("Fish Spawn Table Data")] 
    public List<FishSpawnTableData> fishSpawnTable = new List<FishSpawnTableData>();

    public string DisplayName => levelDataName;
    public string DisplayDescription => levelDescription;
    public string DisplayFlow => currentFlowSpeed.ToString();

    public Sprite DisplaySprite => environmentSprite;
    
    [Header("Spawner Table Data")]
    public SpawnerDataSO  spawnerData;
}
