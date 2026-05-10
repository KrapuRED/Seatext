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

    [SerializeField] private List<LevelNodeProgress> LevelNodeGameProgress = new List<LevelNodeProgress>();
    [SerializeField] private List<string> _levelNodeBeenExplored = new();
    [SerializeField] private string _currentLevelNodeID;

    public string CurrentLevelNodeID => _currentLevelNodeID;
    public List<string> GetExploredNodeIDs() => _levelNodeBeenExplored;
    
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else 
            Destroy(this);
    }

    public string GetLastExploredNodeID()
    {
        if (_levelNodeBeenExplored.Count == 0) return null;
        return _levelNodeBeenExplored[_levelNodeBeenExplored.Count - 1];
    }
    
    public void SetCurrentLevelNode(string levelNodeID)
    {
        _currentLevelNodeID = levelNodeID;
    }

    public void AddLevelNodeBeenExplored(string levelNodeID)
    {
        if (!_levelNodeBeenExplored.Contains(levelNodeID))
            _levelNodeBeenExplored.Add(levelNodeID);
    }

    public void RemoveLevelNodeBeenExplored()
    {
        if (_levelNodeBeenExplored.Count == 0) return;
        _levelNodeBeenExplored.RemoveAt(_levelNodeBeenExplored.Count - 1);
    }

    public bool IsLevelNodeBeenExplored(string levelNodeID)
    {
        return _levelNodeBeenExplored.Contains(levelNodeID);
    }
    
    public bool IsLevelNodeGameProgressExist(string levelNodeID)
    {
        return LevelNodeGameProgress.Exists(x => x.levelNodeID == levelNodeID);
    }
    
    public void SetLevelNodeGameProgress(string levelNodeID, LevelNodeState levelNodeState)
    {
        LevelNodeProgress newProgress = new LevelNodeProgress()
        {
            levelNodeID = levelNodeID,
            levelNodeState = levelNodeState
        };
        
        LevelNodeGameProgress.Add(newProgress);
    }

    public void UpdateLevelNodeGameProgress(string levelNodeID, LevelNodeState levelNodeState)
    {
        if (!IsLevelNodeGameProgressExist(levelNodeID))
        {
            Debug.Log("there are no LevelNodeProgress with the ID " + levelNodeID);
            return;
        }
        
        LevelNodeProgress levelNodeProgress = LevelNodeGameProgress.Find(x => x.levelNodeID == levelNodeID);
        levelNodeProgress.levelNodeState = levelNodeState;
    }
}
