using System;
using UnityEngine;

public class ManagerTimer : MonoBehaviour
{
    public static ManagerTimer instance {get; private set; }
    public float timer;
    private float _timer;
    
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        StartTimer(timer);
    }

    private void Update()
    {
        if (_timer == 0)
            return;
        _timer -= Time.deltaTime;
        GameEvents.OnUpdateTimerGamePlay.Invoke(_timer);
    }

    public void StartTimer(float durationGame)
    {
        _timer = durationGame;
        GameEvents.OnUpdateTimerGamePlay.Invoke(_timer);
    }
}
