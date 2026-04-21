using TMPro;
using UnityEngine;

public class LevelNodeTextUI : TypeBoxUI
{
    [SerializeField] private TextMeshPro _levelNodeTextUI;

    public override void SetWordTextUI(string text)
    {
        _levelNodeTextUI.alpha = 1;
        _levelNodeTextUI.SetText(text);
    }

    public void HideText()
    {
        _levelNodeTextUI.alpha = 0;
    }
}
