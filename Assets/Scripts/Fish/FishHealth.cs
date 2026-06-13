using UnityEngine;

public class FishHealth : MonoBehaviour
{
    [SerializeField] private float _maxHealth;
    [SerializeField] private float _currentHealth;

    public void SetFishHealth(float maxHealth)
    {
        Debug.Log($"Setting Fish Health to {maxHealth}");
        if (maxHealth <= 0)
        {
            Debug.Log($"Fish Health Zero");
            return;
        }
        
        _currentHealth =  _maxHealth = maxHealth;
        
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
}
