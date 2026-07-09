using System;
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
        
        healthSlider.value = currentValue / maxValue;    
    }
}
