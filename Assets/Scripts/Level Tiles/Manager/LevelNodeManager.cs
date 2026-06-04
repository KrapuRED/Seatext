using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

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
    public static LevelNodeManager Instance { get; private set; }

    [Header("Level Node Manager Config")]
    [SerializeField] private Transform containerLevelNode;
    [SerializeField] private List<LevelNodeData> _levelNodeDatas = new List<LevelNodeData>();

    private Dictionary<string, LevelNode> _levelNodeMap = new Dictionary<string, LevelNode>();

    private List<LevelNode> _nearCurrentLevelNodes = new List<LevelNode>();

    [SerializeField] private LevelNode _currentLevelNode;
    [SerializeField] private LevelNode _previousLevelNode;

    public LevelNode CurrentLevelNode => _currentLevelNode;
    public bool IsReady { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else 
            Destroy(gameObject);

        _levelNodeDatas.Clear();
        _levelNodeMap.Clear();
        _nearCurrentLevelNodes.Clear();
    }

    private void OnEnable()
    {
        GameEvents.OnButtonTypeBoxComplete.AddListener(OnButtonComplete);
        GameEvents.OnSetLevelNodeBeenExplored.AddListener(SetLevelNodeBeenExplored);
        
        GameEvents.OnSelectedNextLevelNode.AddListener(SelectedNextLevelNode);
        
        GameEvents.OnSetNearCurrentLevelNode.AddListener(SetNearCurrentLevelNode);
        
        GameEvents.OnSelectedPreviousLevelNode.AddListener(UnSelectedNextLevelNode);
    }

    private void OnDisable() => OnRemoveListener();

    private void OnDestroy()
    {
        OnRemoveListener();

        _currentLevelNode = null;
        _previousLevelNode = null;
        _levelNodeDatas.Clear();
        _nearCurrentLevelNodes.Clear();
        
    }

    private void Start()
    {
        var gameStateManager = GameStateManager.Instance;
        var exploredNodeIDs = new HashSet<string>(gameStateManager.GetExploredNodeIDs());

        foreach (Transform child in containerLevelNode)
        {
            var node = child.GetComponent<LevelNode>();
            
            if (node == null)
            {
                continue;
            }

            RegisterLevelNode(node, exploredNodeIDs);
        }
        
        IsReady = true;
        ApplyExploredStates(exploredNodeIDs);
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

    #region Explore Node Level Management
    public void ExploreNodeLevel()
    {
        if (_currentLevelNode == null)
        {
            Debug.LogWarning($"current LevelNode is null/destroyed : {_currentLevelNode}");
            return;
        }

        var gameStateManager = GameStateManager.Instance;
        gameStateManager.AddLevelNodeBeenExplored(_previousLevelNode.LevelNodeID);
        gameStateManager.SetCurrentLevelNode(_currentLevelNode.LevelNodeID);
        //GameStateManager.Instance.UpdateLevelNodeGameProgress();

        GameManager.instance.LoadLevel(_currentLevelNode);
    }
    #endregion

    public LevelNode FindLevelNodeByID(string levelNodeID)
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
    
    public void RegisterLevelNode(LevelNode levelNode, HashSet<string> exploredIDs = null)
    {
        string levelNodeID = $"LN-{_levelNodeDatas.Count}";
        
        var gameStateManager = GameStateManager.Instance;

        bool isBeenExplored = levelNodeID != null
            ? exploredIDs.Contains(levelNodeID) 
            : gameStateManager.IsLevelNodeBeenExplored(levelNodeID);


        if (levelNode.TileType == LevelNodeType.StartPoint && !isBeenExplored)
        {
            _currentLevelNode = levelNode;
            gameStateManager.SetCurrentLevelNode(levelNodeID);
        }
        
        LevelNodeData newLevelData = new LevelNodeData
        {
            levelNodeName = levelNode.name,
            levelNodeID = levelNodeID,
            levelNode = levelNode
        };

        _levelNodeDatas.Add(newLevelData);
        _levelNodeMap[levelNodeID] = levelNode;

        if (gameStateManager.IsLevelNodeGameProgressExist(levelNodeID))
            gameStateManager.SetLevelNodeGameProgress(levelNodeID, levelNode.LevelNodeState);


        levelNode.IntiliazeLevelNode(levelNodeID);
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
        _currentLevelNode = FindLevelNodeByID(levelNode.LevelNodeID);
        
        ResetAllLevelNode(_currentLevelNode);
    }
    
    public void UnSelectedNextLevelNode()
    {
        if (!IsReady || _previousLevelNode == null) return;
        
        _currentLevelNode.ResetToHidden();
        
        _currentLevelNode = _previousLevelNode;
        _previousLevelNode = null;
        
        _currentLevelNode.OnSetPlayerHere();
    }
  
    public void SetLevelNodeBeenExplored(string levelNodeID)
    {
        if (levelNodeID == string.Empty)
            return;
        
        Debug.Log($"{levelNodeID} is been called to SetLevelNodeBeenExplored");
        LevelNode exploredLevelNode = FindLevelNodeByID(levelNodeID);
        
        if (exploredLevelNode == null)
        {
            Debug.LogWarning($"LevelNode has not been assign to level node {levelNodeID}");
            return;
        }

        _currentLevelNode = exploredLevelNode;

        var gameStateManager = GameStateManager.Instance;
        gameStateManager.SetCurrentLevelNode(exploredLevelNode.LevelNodeID);
        gameStateManager.AddLevelNodeBeenExplored(levelNodeID);
        gameStateManager.UpdateLevelNodeGameProgress(levelNodeID, exploredLevelNode.LevelNodeState);
        
        ResetAllLevelNode(exploredLevelNode);
        _currentLevelNode.OnSetPlayerHere();
    }

    private void OnRemoveListener()
    {
        GameEvents.OnButtonTypeBoxComplete.RemoveListener(OnButtonComplete);
        GameEvents.OnSetLevelNodeBeenExplored.RemoveListener(SetLevelNodeBeenExplored);
        GameEvents.OnSelectedNextLevelNode.RemoveListener(SelectedNextLevelNode);
        GameEvents.OnSetNearCurrentLevelNode.RemoveListener(SetNearCurrentLevelNode);
        GameEvents.OnSelectedPreviousLevelNode.RemoveListener(UnSelectedNextLevelNode);
    }

    private void ApplyExploredStates(HashSet<string> exploredIDs)
    {
        Debug.Log($"[LevelNodeManager] SetLevelNodeBeenExplored called, IsReady={IsReady}, nodeCount={_levelNodeDatas.Count}");
        foreach (var levelNodeID in exploredIDs)
        {
            if (!_levelNodeMap.TryGetValue(levelNodeID, out LevelNode levelNode))
            {
                Debug.LogWarning($"LevelNode with ID {levelNodeID} not found in LevelNodeManager.");
                continue;
            }
            levelNode.SetBeenExplored();
        }
        StartCoroutine(FinishInitialize());
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

    IEnumerator FinishInitialize()
    {
        yield return null;
        yield return new WaitForEndOfFrame();

        IsReady = true;

        // --- ADD THIS LINE ---
        // This safely notifies listeners (like SceneController) that it's safe to process level IDs
        GameEvents.OnLevelNodeManagerReady?.Invoke(); 

        if (_currentLevelNode != null)
        {
            GameEvents.OnChangeCameraPosition.Invoke(_currentLevelNode.transform);
        }
    }
}
