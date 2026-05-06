using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    [Header("Game Level Node Settings")]
    [SerializeField] private LevelNode selectedLevelNode;
    [SerializeField] private bool levelNodeDone;
    
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
        GameEvents.OnSetTimerGamePlay.Invoke(10f);
    }

    public void LevelNodeDone()
    {
        levelNodeDone = true;
        //Show Survive Panel
        PanelManager.instance.OpenPanel("panel-01");
    }

    public void LevelNodeFailed()
    {
        
    }
    
    public void LoadLevel(LevelNode levelNode)
    {
        levelNodeDone = false;
        selectedLevelNode = levelNode;
        SceneController.Instance.LoadScene("Main");
        GameEvents.OnSetTimerGamePlay.Invoke(selectedLevelNode.levelData.durationLevelNode);
    }

    public void LoadLevelSelect()
    {
        SceneController.Instance.LoadScene("LevelSelect");
    }


}
