using UnityEngine;
using UnityEngine.UI;

public class StatusHealthUI : StatusBarUI
{
    [SerializeField] private Slider healthSlider;

    public override void UpdateStatusBar(float currentValue, float maxValue)
    {
       healthSlider.value = currentValue / maxValue;    
    }
}
