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
    public float MaxPlayerHealthStatus => maxPlayerHealthStatus;
    public float CurrentPlayerTrashStatus => currentPlayerTrashStatus;
    public float MaxPlayerHungerStatus => maxPlayerHungerStatus;
    
    
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
        
        if (!initialized)
            InitializedStatus(playerData);
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
        ShowStatus();
    }

    public void ShowStatus()
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

        maxPlayerHealthStatus = playerData.baseFishStats.maxFishHealth;
        currentPlayerHealthStatus = playerData.baseFishStats.maxFishHealth;

        maxPlayerHungerStatus = playerData.maxHunger;
        currentPlayerHungerStatus = maxPlayerHungerStatus;
        currentPlayerTrashStatus  = playerData.startingTrash;
        
        initialized = true;
    }
    
    public void UpgradeMaxHealth(float amount)
    {
        if (!initialized)
        {
            Debug.LogError("Status Player Manager not been initialized");
            return;
        }
        
        float newMaxHealthStatus =  playerData.baseFishStats.maxFishHealth + amount;
        maxPlayerHealthStatus = newMaxHealthStatus;
        
        currentPlayerHealthStatus = Mathf.Min(currentPlayerHealthStatus + amount, maxPlayerHealthStatus);

        ShowStatus(); // updates the UI bar
    }
    
    public void UpgradeMaxHunger(float amount)
    {
        if (!initialized)
        {
            Debug.LogError("Status Player Manager not been initialized");
            return;
        }
        
        float newMaxHungerStatus = playerData.maxHunger + amount;
        
        maxPlayerHungerStatus = newMaxHungerStatus;
        
        currentPlayerHungerStatus = Mathf.Min(currentPlayerHungerStatus + amount, maxPlayerHungerStatus);
        
        ShowStatus(); // updates the UI bar
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

        UpdateStatusHealth(currentPlayerHealthStatus);
    }

    public void UpdateStatusTrash(float trashValue)
    {
        if (!initialized)
        {
            Debug.LogError("Status Player Manager not been initialized");
            return;
        }
        
        Debug.Log("Updating Trash Status");
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

    public void ResetStatus()
    {
        if (!initialized)
            return;
        
        currentPlayerHealthStatus = playerData.baseFishStats.maxFishHealth;
        currentPlayerHungerStatus = maxPlayerHungerStatus;
        currentPlayerTrashStatus  = playerData.startingTrash;
    }
}
