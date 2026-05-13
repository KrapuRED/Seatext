using System;
using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class LevelNodeData
{
    public string    levelNodeName;
    public string    levelNodeID;
    public LevelNode levelNode;
    public bool isExplored;
}

public class LevelNodeManager : MonoBehaviour
{
    [Header("Level Node Manager Config")]
    [SerializeField] private Transform containerLevelNode;
    [SerializeField] private List<LevelNodeData> _levelNodeDatas = new List<LevelNodeData>();
    private List<LevelNode> _nearCurrentLevelNodes = new List<LevelNode>();
    [SerializeField] private LevelNode _currentLevelNode;
    [SerializeField] private LevelNode _previousLevelNode;

    public bool IsReady { get; private set; }

    private void Awake()
    {
        _levelNodeDatas.Clear();
        _nearCurrentLevelNodes.Clear();
    }

    private void OnEnable()
    {
        GameEvents.OnButtonTypeBoxComplete.RemoveListener(OnButtonComplete);
        GameEvents.OnButtonTypeBoxComplete.AddListener(OnButtonComplete);
        
        GameEvents.OnSetLevelNodeBeenExplored.RemoveListener(SetLevelNodeBeenExplored);
        GameEvents.OnSetLevelNodeBeenExplored.AddListener(SetLevelNodeBeenExplored);
    
        GameEvents.OnSelectedNextLevelNode.RemoveListener(SelectedNextLevelNode);
        GameEvents.OnSelectedNextLevelNode.AddListener(SelectedNextLevelNode);
    
        GameEvents.OnSetNearCurrentLevelNode.RemoveListener(SetNearCurrentLevelNode);
        GameEvents.OnSetNearCurrentLevelNode.AddListener(SetNearCurrentLevelNode);
        
        GameEvents.OnSelectedPreviousLevelNode.RemoveListener(UnSelectedNextLevelNode);
        GameEvents.OnSelectedPreviousLevelNode.AddListener(UnSelectedNextLevelNode);
    }

    private void OnDisable()
    {
        GameEvents.OnButtonTypeBoxComplete.RemoveListener(OnButtonComplete);
        GameEvents.OnSetLevelNodeBeenExplored.RemoveListener(SetLevelNodeBeenExplored);
        GameEvents.OnSelectedNextLevelNode.RemoveListener(SelectedNextLevelNode);
        GameEvents.OnSetNearCurrentLevelNode.RemoveListener(SetNearCurrentLevelNode);
        GameEvents.OnSelectedPreviousLevelNode.RemoveListener(UnSelectedNextLevelNode);
        
    }

    private void OnDestroy()
    {
        GameEvents.OnButtonTypeBoxComplete.RemoveListener(OnButtonComplete);
        GameEvents.OnSetLevelNodeBeenExplored.RemoveListener(SetLevelNodeBeenExplored);
        GameEvents.OnSelectedNextLevelNode.RemoveListener(SelectedNextLevelNode);
        GameEvents.OnSetNearCurrentLevelNode.RemoveListener(SetNearCurrentLevelNode);
        GameEvents.OnSelectedPreviousLevelNode.RemoveListener(UnSelectedNextLevelNode);

        _currentLevelNode = null;
        _previousLevelNode = null;
        _levelNodeDatas.Clear();
        _nearCurrentLevelNodes.Clear();
        
    }

    private void Start()
    {
        foreach (Transform child in containerLevelNode)
        {
            RegisterLevelNode(child.GetComponent<LevelNode>());
        }
        
        IsReady = true;
        SetAllLevelNodeBeenExplored(GameStateManager.Instance.GetExploredNodeIDs());
    }

    private void OnButtonComplete(ButtonTypeBoxContext buttonContext)
    {
        Debug.Log("[LevelNodeManager - OnButtonComplete] OnButtonComplete Get Called!");
        
        switch (buttonContext)
        {
            case ButtonTypeBoxContext.ExploreNode:
                ExploreNodeLevel();
                break;
        }
    }
    
    public void ExploreNodeLevel()
    {
        if (_currentLevelNode == null)
        {
            Debug.LogWarning($"current LevelNode is null/destroyed : {_currentLevelNode}");
            return;
        }
        
        GameStateManager.Instance.AddLevelNodeBeenExplored(_currentLevelNode.LevelNodeID);
        GameStateManager.Instance.SetCurrentLevelNode(_currentLevelNode.LevelNodeID);
        //GameStateManager.Instance.UpdateLevelNodeGameProgress();
        GameManager.instance.LoadLevel(_currentLevelNode);
    }

    public LevelNode FindLevelNodebyID(string levelNodeID)
    {
        foreach (var levelNodeData in _levelNodeDatas)
        {
            if (levelNodeData.levelNodeID == levelNodeID)
            {
                return levelNodeData.levelNode;
            }
        }
        return null;
    }

    private int GetLevelNodeID(int currentMaxID)
    {
        int levelNodeID = currentMaxID++;
        return levelNodeID;
    }
    
    public void RegisterLevelNode(LevelNode levelNode)
    {
        string levelNodeID = $"LN-{GetLevelNodeID(_levelNodeDatas.Count)}";
        bool isLevelNodeBeenExplored = GameStateManager.Instance.IsLevelNodeBeenExplored(levelNodeID);
        
        if (levelNode.TileType == LevelNodeType.StartPoint && !isLevelNodeBeenExplored)
        {
            _currentLevelNode = levelNode;
            GameStateManager.Instance.SetCurrentLevelNode(levelNodeID);
        }
        
        levelNode.IntiliazeLevelNode(levelNodeID);
        
        LevelNodeData newLevelData = new LevelNodeData
        {
            levelNodeName = levelNode.name,
            levelNodeID = levelNodeID,
            levelNode = levelNode
        };
        _levelNodeDatas.Add(newLevelData);
        
        if (GameStateManager.Instance.IsLevelNodeGameProgressExist(levelNodeID))
            return;
        
        GameStateManager.Instance.SetLevelNodeGameProgress(levelNodeID, newLevelData.levelNode.LevelNodeState);
    }

    public void SetNearCurrentLevelNode(LevelNode nearLevelNode)
    {
        if (!_nearCurrentLevelNodes.Contains(nearLevelNode))
            _nearCurrentLevelNodes.Add(nearLevelNode);
    }

    public void SelectedNextLevelNode(LevelNode levelNode)
    {
        if (_currentLevelNode == levelNode) return;
        
        _previousLevelNode = _currentLevelNode;
        
        LevelNode nextNode = FindLevelNodebyID(levelNode.LevelNodeID);
        _currentLevelNode = nextNode;
        
        ResetAllLevelNode(_currentLevelNode);
    }
    
    public void UnSelectedNextLevelNode()
    {
        if (!IsReady) return;
        
        if (_previousLevelNode == null) return;
        
        _currentLevelNode.ResetToHidden();
        
        _currentLevelNode = _previousLevelNode;
        _previousLevelNode = null;
        
        _currentLevelNode.IntiliazeLevelNode(_currentLevelNode.LevelNodeID);
    }

    private void SetAllLevelNodeBeenExplored(List<string> levelNodeIDs)
    {
        Debug.Log($"[LevelNodeManager] SetLevelNodeBeenExplored called, IsReady={IsReady}, nodeCount={_levelNodeDatas.Count}");
        foreach (var levelNodeID in  levelNodeIDs)
        {
            LevelNode exploredLevelNode = FindLevelNodebyID(levelNodeID);
            exploredLevelNode.SetBeenExplored();
        }
    }
    
    public void SetLevelNodeBeenExplored(string levelNodeID)
    {
        LevelNode exploredLevelNode = FindLevelNodebyID(levelNodeID);
        _currentLevelNode = exploredLevelNode;

        if (exploredLevelNode == null)
        {
            Debug.LogWarning($"LevelNode has no been assign to level node {levelNodeID}");
            return;
        }
        
        GameStateManager.Instance.SetCurrentLevelNode(exploredLevelNode.LevelNodeID);
        GameStateManager.Instance.AddLevelNodeBeenExplored(levelNodeID);
        GameStateManager.Instance.UpdateLevelNodeGameProgress(levelNodeID, exploredLevelNode.LevelNodeState);
        
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
