using UnityEngine;
using TMPro;

public class FishTextUI : TypeBoxUI
{
    [SerializeField]
    private ScalingTypeBoxUI _scalingTypeBoxUI;

    public override void SetWordTextUI(string text)
    {
        base.SetWordTextUI(text);
        Vector2 preferredSize = textUI.GetPreferredValues(text);
        _scalingTypeBoxUI.SetScalengTypeBoxUI(preferredSize);
    }
}
