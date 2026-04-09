using UnityEngine;
using UnityEngine.UI;

public class StatusHungerUI : StatusBarUI
{
    [Header("Status Hunger UI Config")]
    [SerializeField] private Slider hungerSlider;
    [SerializeField] private Slider trashSlider;

    public override void UpdateStatusBar(float currentValue, float maxValue)
    {
        hungerSlider.value = maxValue/100;
        trashSlider.value = currentValue/100;
    }
}
