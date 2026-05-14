using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class LevelNodeProgress
{
    public string levelNodeID;
    public LevelNodeState levelNodeState;
}

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance { get; private set; }

    [SerializeField] private List<LevelNodeProgress> _levelNodeGameProgress = new List<LevelNodeProgress>();
    [SerializeField] private List<string> _levelNodeBeenExplored = new();
    [SerializeField] private string _currentLevelNodeID;

    private Dictionary<string, LevelNodeProgress> _progressMap = new Dictionary<string, LevelNodeProgress>();
    private HashSet<string> _exploredNodeIDs = new HashSet<string>();

    public string CurrentLevelNodeID => _currentLevelNodeID;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            RebuildLookups();
        }
        else 
            Destroy(this);
    }

    private void RebuildLookups()
    {
        _progressMap.Clear();
        foreach (var progress in _levelNodeGameProgress)
        {
            if (!string.IsNullOrEmpty(progress.levelNodeID))
                _progressMap[progress.levelNodeID] = progress;
        }

        _exploredNodeIDs.Clear();
        foreach (var nodeID in _levelNodeBeenExplored)
        {
            if (!string.IsNullOrEmpty(nodeID))
                _exploredNodeIDs.Add(nodeID);
        }
    }
    
    public void SetCurrentLevelNode(string levelNodeID)
    {
        _currentLevelNodeID = levelNodeID;
    }

    public List<string> GetExploredNodeIDs() => _levelNodeBeenExplored;

    #region Explored Node Management
    public bool IsLevelNodeBeenExplored(string levelNodeID)
    {
        return _exploredNodeIDs.Contains(levelNodeID);
    }

    public void AddLevelNodeBeenExplored(string levelNodeID)
    {
        if (string.IsNullOrEmpty(levelNodeID)) return;

        if (_levelNodeBeenExplored.Contains(levelNodeID))
            return;

        if (!_exploredNodeIDs.Contains(levelNodeID))
        {
            _levelNodeBeenExplored.Add(levelNodeID);
            _exploredNodeIDs.Add(levelNodeID);
        }
    }

    public void RemoveLevelNodeBeenExplored()
    {
        if (_levelNodeBeenExplored.Count == 0) return;

        string lastID = _levelNodeBeenExplored[^1];
        _levelNodeBeenExplored.RemoveAt(_levelNodeBeenExplored.Count - 1);
        _exploredNodeIDs.Remove(lastID);
    }
    #endregion

    #region Progress Node Management
    public bool IsLevelNodeGameProgressExist(string levelNodeID)
    {
        return _progressMap.ContainsKey(levelNodeID);
    }

    public void SetLevelNodeGameProgress(string levelNodeID, LevelNodeState levelNodeState)
    {
        if (_progressMap.ContainsKey(levelNodeID))
        {
            Debug.Log("LevelNodeProgress with the ID " + levelNodeID + " already exists");
            return;
        }

        var newProgress = new LevelNodeProgress()
        {
            levelNodeID = levelNodeID,
            levelNodeState = levelNodeState,
        };

        _levelNodeGameProgress.Add(newProgress);
        _progressMap[levelNodeID] = newProgress;
    }
    #endregion

    public void UpdateLevelNodeGameProgress(string levelNodeID, LevelNodeState levelNodeState)
    {
        if (!IsLevelNodeGameProgressExist(levelNodeID))
        {
            Debug.Log("there are no LevelNodeProgress with the ID " + levelNodeID);
            return;
        }
        
        LevelNodeProgress levelNodeProgress = _levelNodeGameProgress.Find(x => x.levelNodeID == levelNodeID);
        levelNodeProgress.levelNodeState = levelNodeState;
    }
}
