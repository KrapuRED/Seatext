using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class LevelNodeData
{
    public string    levelNodeName;
    public LevelNode levelNode;
}

public class LevelNodeManager : MonoBehaviour
{
    public static LevelNodeManager instance { get; private set; }

    [Header("Level Node Manager Config")]
    [SerializeField] private List<LevelNodeData> _levelNodeDatas = new List<LevelNodeData>();

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    public void RegisterLevelNode(LevelNode levelNode)
    {
        //Debug.Log($"[{this.name} - RegisterLevelNode] Register Level Node : {levelNode.name}");

        if (_levelNodeDatas.Exists(x => x.levelNodeName == levelNode.name))
        {
            Debug.LogWarning($"[{this.name} - RegisterLevelNode] Level Node with name {levelNode.name} already exists. Skipping registration.");
            return;
        }

        LevelNodeData newLevelData = new LevelNodeData
        {
            levelNodeName = levelNode.name,
            levelNode = levelNode
        };

        _levelNodeDatas.Add(newLevelData);
    } 
}
