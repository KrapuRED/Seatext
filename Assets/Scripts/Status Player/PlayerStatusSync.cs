using System;
using UnityEngine;

public class PlayerStatusSync : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private FishHealth _playerFishHealth;
    [SerializeField] private FishHunger _playerFishHunger;

    private StatusPlayerManager _statusPlayerManager;

    private void Start()
    {
        _statusPlayerManager = StatusPlayerManager.Instance;
        
        HandleHealthChange();
        HandleTrashChange();
    }

    public void HandleHealthChange()
    {
        if (_playerFishHealth == null)
        {
            Debug.LogError("[PlayerStatusSync] Player Fish Health is null!");
            return;
        }
        
        _playerFishHealth.SetFishHealth(_statusPlayerManager.CurrentPlayerHealthStatus, _statusPlayerManager.MaxPlayerHealthStatus);
    }
    
    public void HandleTrashChange()
    {
        if (_playerFishHunger == null)
        {
            Debug.LogError("[PlayerStatusSync] Player Fish Hunger is null!");
            return;
        }
        
        _playerFishHunger.InitializeHungerBar(_statusPlayerManager.MaxPlayerHungerStatus);
        _playerFishHunger.SetTrashingHungerbar(_statusPlayerManager.CurrentPlayerTrashStatus);
    }
}
