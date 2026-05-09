using System;
using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class LevelNodeData
{
    public string    levelNodeName;
    public string    levelNodeID;
    public LevelNode levelNode;
}

public class LevelNodeManager : MonoBehaviour
{
    public static LevelNodeManager instance { get; private set; }

    [Header("Level Node Manager Config")]
    [SerializeField] private Transform containerLevelNode;
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

    private void OnEnable()
    {
        GameEvents.OnButtonTypeBoxComplete.AddListener(OnButtonComplete);
    }

    private void OnDisable()
    {
        GameEvents.OnButtonTypeBoxComplete.RemoveListener(OnButtonComplete);
    }

    private void Start()
    {
        foreach (Transform child in containerLevelNode)
        {
            RegisterLevelNode(child.GetComponent<LevelNode>());
        }
    }
    
    private void OnButtonComplete(ButtonTypeBoxContext buttonContext)
    {
        switch (buttonContext)
        {
            case ButtonTypeBoxContext.ExploreNode:
                ExploreNodeLevel();
                break;
            
            case ButtonTypeBoxContext.ClosePanel:
                UnSelectedNextLevelNode();
                break;
        }
    }
    
    
    public void ExploreNodeLevel()
    {
        Debug.Log($"[{this.name} - ExploreNodeLevel] Explore Node Level : {_currentLevelNode.name}");
        
        //SetLevelNodeBeenExplored(_currentLevelNode.LevelNodeID,  true);
        GameManager.instance.LoadLevel(_currentLevelNode);
    }

    private LevelNode FindLevelNodebyID(string levelNodeID)
    {
        foreach (var levelNodeData in _levelNodeDatas)
        {
            if (levelNodeData.levelNodeID == levelNodeID)
                return levelNodeData.levelNode;
        }
        return null;
    }

    private int GetLevelNodeID(int currentMaxID)
    {
        int levelNodeID = currentMaxID++;
        
        return levelNodeID;
    }
    
    private void RegisterLevelNode(LevelNode levelNode)
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
        string levelNodeID = $"LN-{GetLevelNodeID(_levelNodeDatas.Count)}";
        
        levelNode.IntiliazeLevelNode(levelNodeID);
        
        LevelNodeData newLevelData = new LevelNodeData
        {
            levelNodeName = levelNode.name,
            levelNodeID = levelNodeID,
            levelNode = levelNode
        };

        _levelNodeDatas.Add(newLevelData);
    }

    public void SetNearCurrentLevelNode(LevelNode nearLevelNode)
    {
        if (!_nearCurrentLevelNodes.Contains(nearLevelNode))
            _nearCurrentLevelNodes.Add(nearLevelNode);
    }

    public void SelectedNextLevelNode(LevelNode levelNode)
    {
        _currentLevelNode.SetBeenExplored();
        _levelNodeBeenVisit.Add(_currentLevelNode);
        
        _currentLevelNode = levelNode;
        ResetAllLevelNode(_currentLevelNode);
    }

    public void UnSelectedNextLevelNode()
    {
        if (_levelNodeBeenVisit.Count == 0)
            return;
        
        _currentLevelNode.ResetToHidden();
        _currentLevelNode = null;
        
        LevelNode prevNode = _levelNodeBeenVisit[_levelNodeBeenVisit.Count - 1];
        _levelNodeBeenVisit.Remove(prevNode);

        _currentLevelNode = prevNode;
        _currentLevelNode.IntiliazeLevelNode(_currentLevelNode.LevelNodeID);
    }
    
    public void SetLevelNodeBeenExplored(string levelNodeID, bool explored)
    {
        LevelNode exploredLevelNode = FindLevelNodebyID(levelNodeID);
        
        exploredLevelNode.SetBeenExplored();
        _levelNodeBeenVisit.Add(_currentLevelNode);
        ResetAllLevelNode(exploredLevelNode);
        
        exploredLevelNode.CheckSurroundingLevelNode();
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
