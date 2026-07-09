using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    [Header("Game Level Node Settings")]
    [SerializeField] private bool _levelNodeDone;
    [SerializeField] private bool isFailed;
    [SerializeField] private string _levelNodeID;
    [SerializeField] private LevelDataSO  _levelData;
    
    public LevelDataSO LevelDataSO => _levelData;
    public string LevelNodeID => _levelNodeID;
    public bool LevelDone => _levelNodeDone;
    public bool IsFailed => isFailed;
    
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
        PanelManager.instance.OpenPanelByTypePanel(PanelType.PanelSurvive);
    }

    public void LevelNodeFailed()
    {
        _levelNodeDone = true;
        isFailed =  true;

        PanelManager.instance.OpenPanelByTypePanel(PanelType.PanelDead);
    }
    
    public void LoadLevel(LevelNode levelNode)
    {
        _levelData = levelNode.LevelDataSO;
        _levelNodeID = levelNode.LevelNodeID;
        _levelNodeDone = false;
        
        SceneController.instance.LoadScene("Main");
    }

    public void LoadLevelSelect()
    {
        GameStateManager.Instance.AddLevelNodeBeenExplored(_levelNodeID);
        SceneController.instance.LoadScene("LevelSelect");
    }
}
