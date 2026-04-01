using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public static PauseManager instance;

    [SerializeField] private bool isPaused;
    [SerializeField] private List<IPausable> pausables = new List<IPausable>();

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void Register(IPausable p)
    {
        pausables.Add(p);
    }

    public void SetPause(bool pause)
    {
        foreach (var p in pausables)
        {
            if (pause) p.OnPause();
            else p.OnResume();
        }
    }

    public void ResumeGame()
    {
        PanelManager.instance.ClosePanel("panel-00");
        isPaused = false;
        SetPause(isPaused);
    }

    public void RestartGame()
    {

    }

    public void QuitGame()
    {
        
    }
}
