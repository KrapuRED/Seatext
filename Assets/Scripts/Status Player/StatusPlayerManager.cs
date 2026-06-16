using System;
using UnityEngine;

public class StatusPlayerManager : MonoBehaviour
{
    public static StatusPlayerManager Instance { get; private set; }

    [SerializeField] private PlayerDataSO playerData;

    [Header("Player Health Status")]
    [SerializeField] private float maxPlayerHealthStatus;
    [SerializeField] private float currentPlayerHealthStatus;

    [Header("Player Trash Status")]
    [SerializeField] private float maxPlayerHungerStatus;
    [SerializeField] private float currentPlayerHungerStatus;
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

    #region Event Subscription

    private void OnEnable()
    {
        GameEvents.OnShowUI.AddListener(ShowStatus);

    }

    private void OnDisable()
    {
        OnRemovedListener();

    }

    private void OnDestroy()
    {
        OnRemovedListener();
    }

    private void OnRemovedListener()
    {
        GameEvents.OnShowUI.RemoveListener(ShowStatus);
    }

    #endregion

    private void Start()
    {
        InitializedStatus(playerData);

        ShowStatus();
    }

    private void ShowStatus()
    {
        GameEvents.OnUpdateHealthBar.Invoke(currentPlayerHealthStatus, maxPlayerHealthStatus);
        GameEvents.OnUpdateHungerBar.Invoke(currentPlayerTrashStatus, currentPlayerHungerStatus);
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
        currentPlayerHungerStatus = maxPlayerHungerStatus;
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
    

    public void HealingHealth(float healPercent)
    {
        if (!initialized)
        {
            Debug.LogError("Status Player Manager not been initialized");
            return;
        }

        float healValue = maxPlayerHealthStatus * (healPercent / 100f);

        // Add heal but don't exceed max health
        currentPlayerHealthStatus = Mathf.Min(
            currentPlayerHealthStatus + healValue,
            maxPlayerHealthStatus);
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

    public void UpdateStatusHunger(float hungerValue)
    {
        if (!initialized)
        {
            Debug.LogError("Status Player Manager not been initialized");
            return;
        }

        currentPlayerHungerStatus = hungerValue;
    }

    public void CleanTrash()
    {
        if (!initialized)
        {
            Debug.LogError("Status Player Manager not been initialized");
            return;
        }

        currentPlayerTrashStatus = 0 ;
    }
}
