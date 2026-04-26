using UnityEngine;
using UnityEngine.UI;

public class StatusHealthUI : StatusBarUI
{
    [SerializeField] private Slider healthSlider;

    private void OnEnable()
    {
        GameEvents.OnUpdateHealthBar.AddListener(UpdateStatusBar);
    }

    private void OnDisable()
    {
        GameEvents.OnUpdateHealthBar.RemoveListener(UpdateStatusBar);
    }

    public override void UpdateStatusBar(float currentValue, float maxValue)
    {
       healthSlider.value = currentValue / maxValue;    
    }
}
