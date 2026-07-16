using UnityEngine;
using UnityEngine.UI;

public class StatusHungerUI : StatusBarUI
{
    [Header("Status Hunger UI Config")]
    [SerializeField] private Slider hungerSlider;
    [SerializeField] private Slider trashSlider;

    private void OnEnable()
    {
        GameEvents.OnUpdateHungerBar.AddListener(UpdateStatusBar);
    }

    private void OnDisable()
    {
        OnRemoveListeners();
    }

    private void OnDestroy()
    {
        OnRemoveListeners();
    }

    private void OnRemoveListeners()
    {
        GameEvents.OnUpdateHungerBar.RemoveListener(UpdateStatusBar);
    }

    
    //loat currentValue = Trash, float maxValue = Hunger
    public override void UpdateStatusBar(float currentValue, float maxValue)
    {
        float fixedMax = StatusPlayerManager.Instance.MaxPlayerHungerStatus;
        
        if (fixedMax <= 0f)
            return;
        
        hungerSlider.value  = Mathf.Clamp01(maxValue / fixedMax); // hunger remaining
        trashSlider.value = Mathf.Clamp01(currentValue / fixedMax); 
    }
}
