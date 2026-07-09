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
        GameEvents.OnUpdateHealthBar.RemoveListener(UpdateStatusBar);
    }

    public override void UpdateStatusBar(float currentValue, float maxValue)
    {
        hungerSlider.value  = Mathf.Abs(maxValue / 100);
        trashSlider.value   = Mathf.Abs(currentValue / 100);
    }
}
