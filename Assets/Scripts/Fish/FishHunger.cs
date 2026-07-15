using UnityEngine;

public class FishHunger : MonoBehaviour, ISaveStatus
{
    [Header("Fish Status Config")]
    [SerializeField] private float maxHunger;
    [SerializeField] private float _currentHunger;
    [SerializeField] private float trashGain;
    [SerializeField] private float rateDrainHunger;
    [SerializeField] private float rateDrainHP;

    [Header("UI")]
    [SerializeField] private StatusHungerUI statusHungerUI;
    [SerializeField] private StatusHealthUI statusHealthUI;

    [SerializeField] private PlayerFish _playerFish;

    private float _damageAccum;
    private float _debugTimer;
    
    public float currentHunger => _currentHunger;

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
    
    public void InitializeHungerBar(float hungerBar)
    {
        maxHunger = hungerBar;
        _currentHunger = maxHunger;
        //statusHungerUI.UpdateStatusBar(trashGain, maxHunger);
        GameEvents.OnUpdateHungerBar.Invoke(trashGain, maxHunger);
    }

    public void Starve()
    {
        if (currentHunger <= 0)
        {
            float damageThisFrame = rateDrainHP * Time.deltaTime;
            
            _damageAccum += damageThisFrame;
            _debugTimer += Time.deltaTime;
            
            if (_debugTimer >= 1f)
            {
                Debug.Log($"HP lost in the last second: {_damageAccum}");
                _playerFish.TakeStarvationDamage(_damageAccum);
                _damageAccum = 0f;
                _debugTimer = 0f;
            }
            return;
        }

        _currentHunger -= rateDrainHunger * Time.deltaTime;
        GameEvents.OnUpdateHungerBar.Invoke(trashGain, _currentHunger);
    }

    public void SetTrashingHungerbar(float gainTrash)
    {
        Debug.Log($"gainTrash: {gainTrash}");

        maxHunger -= gainTrash;
        trashGain += gainTrash;
        
        ResetHunggerBar();
    }
    
    
    public void ResetHunggerBar()
    {
        GameEvents.OnUpdateHungerBar.Invoke(trashGain, maxHunger);
        _currentHunger = maxHunger;
    }

    public void SaveStatus()
    {
        if (this == null) return;
        
        Debug.Log($"Saving Fish Hungger : trash {trashGain} hunger {currentHunger}");
        StatusPlayerManager.Instance.UpdateStatusTrash(trashGain);
        StatusPlayerManager.Instance.UpdateStatusHunger(currentHunger);
    }
}
