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
    [SerializeField] private List<LevelNode> _nearCurrentLevelNodes = new List<LevelNode>();
    [SerializeField] private List<LevelNode> _levelNodeBeenVisit = new List<LevelNode>();
    [SerializeField] private LevelNode _currentLevelNode;

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

    public void ExploreNodeLevel()
    {
        Debug.Log($"[{this.name} - ExploreNodeLevel] Explore Node Level : {_currentLevelNode.name}");
        GameManager.Instance.LoadLevel(_currentLevelNode);
    }

    public void RegisterLevelNode(LevelNode levelNode)
    {
        //Debug.Log($"[{this.name} - RegisterLevelNode] Register Level Node : {levelNode.name}");
        if (levelNode.TileType == LevelNodeType.StartPoint)
        {
            _currentLevelNode = levelNode;
        }

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

    public void SetNearCurrentLevelNode(LevelNode nearLevelNode)
    {
        if (!_nearCurrentLevelNodes.Contains(nearLevelNode))
            _nearCurrentLevelNodes.Add(nearLevelNode);
    }

    public void SelectedNextLevelNode(LevelNode nextLevelNode)
    {
        _currentLevelNode.SetBeenVisited();
        _levelNodeBeenVisit.Add(_currentLevelNode);
        
        ResetAllLevelNode(nextLevelNode);

        _currentLevelNode = nextLevelNode;
        _currentLevelNode.CheckSurroundingLevelNode();
    }

    private void ResetAllLevelNode(LevelNode excludeNode)
    {
        foreach (var levelNode in _nearCurrentLevelNodes)
        {
            if (levelNode == excludeNode) continue;
            levelNode.ResetToHidden();
        }
        _nearCurrentLevelNodes.Clear();
    }
}
