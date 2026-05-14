using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    [Header("Game Level Node Settings")]
    [SerializeField] private bool _levelNodeDone;
    [SerializeField] private string _levelNodeID;
    [SerializeField] private LevelDataSO  _levelData;
    
    public LevelDataSO LevelDataSO => _levelData;
    public string LevelNodeID => _levelNodeID;
    public bool LevelDone => _levelNodeDone;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }   
    }

    private void OnEnable()
    {
        GameEvents.OnButtonTypeBoxComplete.AddListener(OnButtonComplete);
        GameEvents.OnChangeToSelectLevel.AddListener(LoadLevelSelect);
        GameEvents.OnEndDuration.AddListener(LevelNodeDone);
    }

    private void OnDisable()
    {
        GameEvents.OnButtonTypeBoxComplete.RemoveListener(OnButtonComplete);
        GameEvents.OnChangeToSelectLevel.RemoveListener(LoadLevelSelect);
        GameEvents.OnEndDuration.RemoveListener(LevelNodeDone);
    }

    private void OnButtonComplete(ButtonTypeBoxContext buttonContext)
    {
        switch (buttonContext)
        {
            case ButtonTypeBoxContext.DoneExploreNode:
                LoadLevelSelect();
                break;
        }
    }

    private void LevelNodeDone()
    {
        _levelNodeDone = true;
        //Show Survive Panel
        PanelManager.instance.OpenPanelByID("panel-01");
    }

    public void LevelNodeFailed()
    {
        
    }
    
    public void LoadLevel(LevelNode levelNode)
    {
        _levelData = levelNode.LevelDataSO;
        _levelNodeID = levelNode.LevelNodeID;
        _levelNodeDone = false;
        
        SceneController.Instance.LoadScene("Main");
    }

    public void LoadLevelSelect()
    {
        GameStateManager.Instance.AddLevelNodeBeenExplored(_levelNodeID);
        SceneController.Instance.LoadScene("LevelSelect");
    }
}
