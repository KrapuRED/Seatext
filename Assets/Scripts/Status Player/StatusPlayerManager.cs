using System;
using UnityEngine;

public class StatusPlayerManager : MonoBehaviour
{
    public static StatusPlayerManager Instance { get; private set; }

    [Header("Player Health Status")]
    [SerializeField] private float currentPlayerHealthStatus;
    
    [Header("Player Trash Status")]
    [SerializeField] private float maxPlayerHungerStatus;
    [SerializeField] private float currentPlayerTrashStatus;
    
    [Header("Status Player Manager Config")]
    [SerializeField] private bool initialized;
    
    public float CurrentPlayerHealthStatus => currentPlayerHealthStatus;
    public float CurrentPlayerTrashStatus => currentPlayerTrashStatus;
    public float MaxPlayerHungerStatus => maxPlayerHungerStatus;
    
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    
    public void InitializedStatus(PlayerDataSO playerData)
    {
        if (initialized)
            return;
        
        if (playerData == null)
        {
            Debug.LogError("Fish Data is null");
            return;
        }

        currentPlayerHealthStatus = playerData.baseFishStats.maxFishHealth;

        maxPlayerHungerStatus = playerData.maxHunger;
        currentPlayerTrashStatus  = playerData.startingTrash;
        
        initialized = true;
    }

    public void UpdateStatusHealth(float healthValue)
    {
        if (!initialized)
        {
            Debug.LogError("Status Player Manager not been initialized");
            return;
        }
        
        currentPlayerHealthStatus = healthValue;
    }
    
    public void UpdateStatusTrash(float trashValue)
    {
        if (!initialized)
        {
            Debug.LogError("Status Player Manager not been initialized");
            return;
        }
        
        currentPlayerTrashStatus = trashValue;
    }
}
