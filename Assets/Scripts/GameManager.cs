using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private LevelNode _selectedLevelNode;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }   
    }

    private void OnEnable()
    {
        GameEvent.OnChangeToSelectLevel.AddListener(LoadLevelSelect);
    }

    private void OnDisable()
    {
        GameEvent.OnChangeToSelectLevel.RemoveListener(LoadLevelSelect);
    }

    public void LoadLevel(LevelNode levelNode)
    {
        _selectedLevelNode = levelNode;
        SceneController.Instance.LoadScene("Main");
    }

    public void LoadLevelSelect()
    {
        SceneController.Instance.LoadScene("LevelSelect");
    }


}
