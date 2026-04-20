using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelSO", menuName = "Level Data/LevelSO")]
public class LevelSO : ScriptableObject
{
    public string levelName;
    public int levelID;

    public List<SpawnTableData> FishSpawnTableData = new List<SpawnTableData>();
}
