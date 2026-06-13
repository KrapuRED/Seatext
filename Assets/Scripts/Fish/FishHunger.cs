using UnityEngine;

public class FishHunger : MonoBehaviour
{
    [Header("Fish Status Config")]
    [SerializeField] private float maxHunger;
    [SerializeField] private float _currentHunger;
    [SerializeField] private float trashGain;

    [Header("UI")]
    [SerializeField] private StatusHungerUI statusHungerUI;
    [SerializeField] private StatusHealthUI statusHealthUI;

    [SerializeField] private PlayerFish _playerFish;

    public float currentHunger => _currentHunger;

    public void InitializeHungerBar(float hungerBar)
    {
        _currentHunger = maxHunger;
        //statusHungerUI.UpdateStatusBar(trashGain, maxHunger);
        GameEvents.OnUpdateHungerBar.Invoke(trashGain, maxHunger);
    }

    public void Starve()
    {
        if (currentHunger <= 0)
        {
            Debug.Log($"[PlayerFish - Update] PlayerFish {gameObject.name} is too hungry to move!");
            float damageValue = Time.deltaTime;
            _playerFish.TakingDamage(damageValue);
            return;
        }

        _currentHunger -= Time.deltaTime;
        //statusHungerUI.UpdateStatusBar(trashGain, _currentHunger);
        GameEvents.OnUpdateHungerBar.Invoke(trashGain, _currentHunger);
    }

    public void OnUpdateHealthBar(float currentHealth, float maxHealth)
    {
        //statusHealthUI.UpdateStatusBar(currentHealth, maxHealth);
        GameEvents.OnUpdateHungerBar.Invoke(currentHealth, maxHealth);
    }

    public void SetTrashingHungerbar(float gainTrash)
    {
        trashGain += gainTrash;
        maxHunger -= gainTrash;

        ResetHunggerBar();
    }

    public void ResetHunggerBar()
    {
        GameEvents.OnUpdateHungerBar.Invoke(trashGain, maxHunger);
        _currentHunger = maxHunger;
    }
}
