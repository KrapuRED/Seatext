using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController instance { get; private set; }
    
    private string _pendingExploredNodeID = string.Empty;
    
    Coroutine _loadingScene;
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
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void LoadScene(string sceneName)
    {
        // Implement scene loading logic here, e.g., using UnityEngine.SceneManagement
        if (_loadingScene != null)
            return;
        
        _loadingScene =  StartCoroutine(LoadSceneAsync(sceneName));
    }

    IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation scene = SceneManager.LoadSceneAsync(sceneName);
        scene.allowSceneActivation = false;

        do
        {
            yield return null; 
        } while (scene.progress < 0.9f);
        
        scene.allowSceneActivation = true;
        
        
        _loadingScene = null;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch (scene.name)
        {
            case "Main":
                if (ManagerTimer.instance != null & GameManager.instance != null)
                    ManagerTimer.instance.StartTimer(GameManager.instance.LevelDataSO.durationLevelDataNode);
                else
                {
                    Debug.LogError("Main Scene managers are missing!");
                }
                break;
            case "LevelSelect":
                // 1. Cache the node ID we want to process
                _pendingExploredNodeID = GameManager.instance.LevelNodeID;

                // 2. Subscribe to the "Ready" event instead of firing blindly
                GameEvents.OnLevelNodeManagerReady.AddListener(HandleLevelNodeManagerReady);
                break;
        }
    }

    private void HandleLevelNodeManagerReady()
    {
        // Unsubscribe immediately so it doesn't run multiple times
        GameEvents.OnLevelNodeManagerReady.RemoveListener(HandleLevelNodeManagerReady);

        // 3. Now that the manager is 100% initialized, it's safe to fire!
        if (!string.IsNullOrEmpty(_pendingExploredNodeID))
        {
            GameEvents.OnShowUI.Invoke();
            GameEvents.OnSetLevelNodeBeenExplored.Invoke(_pendingExploredNodeID);
            _pendingExploredNodeID = string.Empty; // Clear cache
        }
    }
}
