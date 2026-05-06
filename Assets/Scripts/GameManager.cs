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
    }

    private void OnDisable()
    {
        GameEvents.OnChangeToSelectLevel.RemoveListener(LoadLevelSelect);
    }

    public void LevelNodeDone()
    {
        levelNodeDone = true;
    }
    
    public void LoadLevel(LevelNode levelNode)
    {
        levelNodeDone = false;
        selectedLevelNode = levelNode;
        SceneController.Instance.LoadScene("Main");
        //GameEvents.OnSetTimerGamePlay.Invoke(120);
    }

    public void LoadLevelSelect()
    {
        SceneController.Instance.LoadScene("LevelSelect");
    }


}
