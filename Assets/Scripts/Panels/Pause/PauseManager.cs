using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PauseAbleData
{
    public string ObjectName;
    public IPausable Pausable;
}

public class PauseManager : MonoBehaviour
{
    public static PauseManager instance;

    [SerializeField] private bool isPaused;
    [SerializeField] private List<IPausable> pausables = new List<IPausable>();
    [SerializeField] private List<PauseAbleData> dataPauseAble = new List<PauseAbleData>();


    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void Register(IPausable p)
    {
        PauseAbleData data = new PauseAbleData
        {
            ObjectName = p.ToString(),
            Pausable = p
        };
        dataPauseAble.Add(data);
    }

    public void Unregister(IPausable p)
    {
        for (int i = 0; i < dataPauseAble.Count; i++)
        {
            if (dataPauseAble[i].Pausable == p)
            {
                dataPauseAble.RemoveAt(i);
                break;
            }
        }
    }

    public void SetPause(bool pause)
    {
        var ListPauses = new List<PauseAbleData>(dataPauseAble);

        Debug.Log($"[PauseManager - SetPause] Set Pause : {pause} For Pausables Count : {ListPauses.Count}");

        foreach (var p in ListPauses)
        {
            if (pause) p.Pausable.OnPause();
            else p.Pausable.OnResume();
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

    private void Test()
    {
        for (int i = 0; i < pausables.Count; i++)
        {
            Debug.Log($"[PauseManager - Test] {i + 1} Pausable : {pausables[i]}");
        }
    }
}
