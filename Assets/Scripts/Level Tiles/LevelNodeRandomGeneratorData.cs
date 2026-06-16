using UnityEngine;
using System.Collections.Generic;

public class LevelNodeRandomGeneratorData : MonoBehaviour
{
    [SerializeField] private List<LevelDataSO> dataSOs = new();


    public LevelDataSO GetRandomDataLevelNode()
    {
        int random = Random.Range(0, dataSOs.Count);

        return dataSOs[random];
    }
}
