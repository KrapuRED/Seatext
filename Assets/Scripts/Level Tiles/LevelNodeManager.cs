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
    [SerializeField] private List<LevelNode> _nearCurrentLevelNodes = new List<LevelNode>();
    [SerializeField] private LevelNode _currentLevelNode;

    public bool IsReady { get; private set; }

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
    }

    private void OnDisable()
    {
        GameEvents.OnButtonTypeBoxComplete.RemoveListener(OnButtonComplete);
        GameEvents.OnSetLevelNodeBeenExplored.RemoveListener(SetLevelNodeBeenExplored);
        GameEvents.OnSelectedNextLevelNode.RemoveListener(SelectedNextLevelNode);
        GameEvents.OnSetNearCurrentLevelNode.RemoveListener(SetNearCurrentLevelNode);
    }

    private void OnDestroy()
    {
        GameEvents.OnButtonTypeBoxComplete.RemoveListener(OnButtonComplete);
        GameEvents.OnSetLevelNodeBeenExplored.RemoveListener(SetLevelNodeBeenExplored);
        GameEvents.OnSelectedNextLevelNode.RemoveListener(SelectedNextLevelNode);
        GameEvents.OnSetNearCurrentLevelNode.RemoveListener(SetNearCurrentLevelNode);
    }

    private void Start()
    {
        foreach (Transform child in containerLevelNode)
        {
            RegisterLevelNode(child.GetComponent<LevelNode>());
        }
        
        IsReady = true;
        Debug.Log($"[LevelNodeManager] IsReady = {IsReady}");
    }

    private void OnButtonComplete(ButtonTypeBoxContext buttonContext)
    {
        Debug.Log("[LevelNodeManager - OnButtonComplete] OnButtonComplete Get Called!");
        
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
        //GameStateManager.Instance.UpdateLevelNodeGameProgress();
        GameManager.instance.LoadLevel(_currentLevelNode);
    }

    public LevelNode FindLevelNodebyID(string levelNodeID)
    {
        foreach (var levelNodeData in _levelNodeDatas)
        {
            if (levelNodeData.levelNodeID == levelNodeID)
            {
                Debug.Log($"Explored Level Node = { levelNodeData.levelNode}");
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
        if (levelNode.TileType == LevelNodeType.StartPoint)
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
        GameStateManager.Instance.AddLevelNodeBeenExplored(_currentLevelNode.LevelNodeID);
        GameStateManager.Instance.SetCurrentLevelNode(levelNode.LevelNodeID);
        
        _currentLevelNode = levelNode;
        ResetAllLevelNode(_currentLevelNode);
    }
    
    public void UnSelectedNextLevelNode()
    {
        Debug.Log("UnSelectedNextLevelNode get called!");
        _currentLevelNode.ResetToHidden();
        
        string prevNodeID = GameStateManager.Instance.GetLastExploredNodeID();
        
        GameStateManager.Instance.RemoveLevelNodeBeenExplored();
        GameStateManager.Instance.SetCurrentLevelNode(prevNodeID);
        
        LevelNode prevLevelNode = FindLevelNodebyID(prevNodeID);
        if (prevLevelNode == null)
        {
            Debug.LogWarning($"[UnSelectedNextLevelNode] Could not find node with ID: {prevNodeID}");
            return;
        }
        _currentLevelNode = prevLevelNode;
        _currentLevelNode.IntiliazeLevelNode(_currentLevelNode.LevelNodeID);
    }

    private void SetAllLevelNodeBeenExplored(List<string> levelNodeIDs)
    {
        Debug.Log($"[LevelNodeManager] SetLevelNodeBeenExplored called, IsReady={IsReady}, nodeCount={_levelNodeDatas.Count}");
        /*foreach (var levelNodeID in  levelNodeIDs)
        {
            LevelNode exploredLevelNode = FindLevelNodebyID(levelNodeID);
            Debug.Log("explored level node " + exploredLevelNode);
            exploredLevelNode.SetBeenExplored();
        }*/
    }
    
    public void SetLevelNodeBeenExplored(string levelNodeID)
    {
        //set perv explore node
        SetAllLevelNodeBeenExplored(GameStateManager.Instance.GetExploredNodeIDs());
            
        LevelNode exploredLevelNode = FindLevelNodebyID(levelNodeID);

        if (exploredLevelNode == null)
        {
            Debug.LogWarning($"LevelNod has no been assign to level node {levelNodeID}");
            return;
        }
        
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
