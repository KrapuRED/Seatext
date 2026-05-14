using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance { get; private set; }
    
    Coroutine _loadingScene;
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
        
        yield return null;
        yield return null;
        yield return null;
        yield return new WaitForEndOfFrame();
        
        switch (sceneName)
        {
            case "Main":
                ManagerTimer.instance.StartTimer(GameManager.instance.LevelDataSO.durationLevelDataNode);
                break;
            case "LevelSelect":
                yield return new WaitUntil(() => LevelNodeManager.Instance != null);

                GameEvents.OnSetLevelNodeBeenExplored.Invoke(GameManager.instance.LevelNodeID);
                break;
        }
        
        _loadingScene = null;
    }
}
