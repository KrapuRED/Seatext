using UnityEngine;
using System.Collections.Generic;

public class LevelNodeDatas 
{
    public LevelNodeType levelNodeType;
    public List<LevelDataSO> levelNodeDatas;
}
public class LevelNodeRandomData : MonoBehaviour
{
    [Header("Level Node Aviable Datas")]
    [SerializeField] private List<LevelNodeDatas> levelNodeDatas = new();
}
