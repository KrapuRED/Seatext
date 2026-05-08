using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    [Header("Game Level Node Settings")]
    [SerializeField] private bool _levelNodeDone;
    [SerializeField] private LevelDataSO  _levelData;
    
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
        GameEvents.OnChangeToSelectLevel.AddListener(LoadLevelSelect);
        GameEvents.OnEndDuration.AddListener(LevelNodeDone);
    }

    private void OnDisable()
    {
        GameEvents.OnChangeToSelectLevel.RemoveListener(LoadLevelSelect);
        GameEvents.OnEndDuration.RemoveListener(LevelNodeDone);
    }

    private void Start()
    {
        GameEvents.OnSetTimerGamePlay.Invoke(_levelData.durationLevelNode);
    }

    private void LevelNodeDone()
    {
        _levelNodeDone = true;
        //Show Survive Panel
        PanelManager.instance.OpenPanel("panel-01");
    }

    public void LevelNodeFailed()
    {
        
    }
    
    public void LoadLevel(LevelNode levelNode)
    {
        _levelData = levelNode.levelData;
        _levelNodeDone = false;
        SceneController.Instance.LoadScene("Main");
        GameEvents.OnSetTimerGamePlay.Invoke(_levelData.durationLevelNode);
    }

    public void LoadLevelSelect()
    {
        SceneController.Instance.LoadScene("LevelSelect");
    }
}
