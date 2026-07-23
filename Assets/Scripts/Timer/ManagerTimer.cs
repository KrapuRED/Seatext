using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CounterTime
{
    public string counterTimeName;
    public float maxTimmer;
    public float currnetTimer;
    public bool isPassMaxTime;
}

public class ManagerTimer : MonoBehaviour, IPausable
{
    public static ManagerTimer instance {get; private set; }
    
    [SerializeField] private List<CounterTime> counterTimeDatas = new();
    
    private bool _gameStarted;
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

    private void Start()
    {
        GameEvents.OnMainSceneReady.Invoke();
    }

    private void Update()
    {
        if (!_gameStarted)
            return;
        
        if (_isPaused)
            return;

        if (GameManager.instance.LevelDone)
            return;
        
        UpdateCounterTime();
        
        if (_timer < 0)
        {
            GameEvents.OnEndDuration.Invoke();
            return;
        }
        _timer -= Time.deltaTime;
        GameEvents.OnUpdateTimerGamePlay.Invoke(_timer);
    }

    public void StartTimer(float durationGame)
    {

        _timer = durationGame;
        GameEvents.OnUpdateTimerGamePlay.Invoke(_timer);
        _gameStarted = true;
    }

    private void UpdateCounterTime()
    {
        foreach (var counterTime in counterTimeDatas)
        {
            if (counterTime.currnetTimer >= counterTime.maxTimmer)
            {
                counterTime.isPassMaxTime = true;
            }
            else
            {
                counterTime.currnetTimer += Time.deltaTime;
            }
        }
    }

    public bool CheckCounterTime(string counterName)
    { 
        if (counterTimeDatas.Count <= 0)
            return false;
        
        CounterTime data = counterTimeDatas.FirstOrDefault(x => x.counterTimeName == counterName);
        
        if (data == null)
            return false;
        
        return data.isPassMaxTime;
    }
    
    public void AssignCounterTime(string counterName, float durationGame)
    {
        //if there counterTime with the counterName Reset
        CounterTime data = counterTimeDatas.FirstOrDefault(x => x.counterTimeName == counterName);
        if (data != null)
        {
            data.isPassMaxTime = false;
            data.currnetTimer = 0;
            
            return;
        }
        
        CounterTime counterTime = new CounterTime
        {
            counterTimeName = counterName,
            maxTimmer = durationGame,
            currnetTimer = 0,
            isPassMaxTime = false
        };
        
        counterTimeDatas.Add(counterTime);
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
