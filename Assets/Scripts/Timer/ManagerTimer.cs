using System;
using UnityEngine;

public class ManagerTimer : MonoBehaviour, IPausable
{
    public static ManagerTimer instance {get; private set; }
    private float _timer;
    private bool _isPaused;
    
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        GameEvents.OnSetTimerGamePlay.AddListener(StartTimer);
        PauseManager.instance.Register(this);
    }

    private void OnDisable()
    {
        GameEvents.OnSetTimerGamePlay.RemoveListener(StartTimer);
        PauseManager.instance.Unregister(this);
    }

    private void Update()
    {
        if (_isPaused)
            return;
            
        if (_timer <= 0)
        {
            GameEvents.OnEndDuration.Invoke();
            return;
        }
        _timer -= Time.deltaTime;
        GameEvents.OnUpdateTimerGamePlay.Invoke(_timer);
    }

    public void StartTimer(float durationGame)
    {
        Debug.Log($"StartTimer with {durationGame} seconds");    
        _timer = durationGame;
        GameEvents.OnUpdateTimerGamePlay.Invoke(_timer);
    }

    public void OnPause()
    {
        _isPaused = true;
    }

    public void OnResume()
    {
        _isPaused = false;
    }
}
