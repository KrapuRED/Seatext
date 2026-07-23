using TMPro;
using UnityEngine;

public class MultiplierUI : MonoBehaviour
{
    [SerializeField] private TMP_Text multiplierText;
    [SerializeField] private string multiplierName;
    
    public void UpdatemultiplierUI(int combo, float multiplier)
    {
        if (!string.IsNullOrEmpty(multiplierName))
            multiplierText.text = $"{combo}x {multiplierName} {multiplier}x";
    }

    public void ShowMultiplierEatingUI(Transform locationShow, float multiplier)
    {
        transform.position = locationShow.position;
        multiplierText.text = $"{multiplier}x";
    }
    
    public void ResetMultiplierUI()
    {
        multiplierText.text = "";
    }
}
