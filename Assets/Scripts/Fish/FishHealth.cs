using System;
using UnityEngine;

public class FishHealth : MonoBehaviour, ISaveStatus
{
    [SerializeField] private float _maxHealth;
    [SerializeField] private float _currentHealth;
    
    #region  Event

    private void OnEnable()
    {
        GameEvents.OnSaveCurrentStatus.AddListener(SaveStatus);
    }

    private void OnDisable()
    {
        OnRemoveListener();
    }

    private void OnDestroy()
    {
        OnRemoveListener();
    }

    private void OnRemoveListener()
    {
        GameEvents.OnSaveCurrentStatus.RemoveListener(SaveStatus);
    }

    #endregion
    
    public void SetFishHealth(float currentHealth ,float maxHealth)
    {
        Debug.Log($"Setting Fish Health to {maxHealth}");
        if (maxHealth <= 0)
        {
            Debug.Log($"Fish Health Zero");
            return;
        }
        
        _maxHealth = maxHealth;
        _currentHealth = currentHealth;
        
        GameEvents.OnUpdateHealthBar.Invoke(_currentHealth, _maxHealth);
    }

    public void OnTakeDamage(float damageValue)
    {
        
        _currentHealth -= damageValue;

        GameEvents.OnUpdateHealthBar.Invoke(_currentHealth, _maxHealth);
    }

    public bool IsDead()
    {
        return _currentHealth <= 0;
    }

    public void SaveStatus()
    {
        if (this == null) return;
        
        Debug.Log("Saving Fish Health : " + _currentHealth);
        StatusPlayerManager.Instance.UpdateStatusHealth(_currentHealth);
    }
}
