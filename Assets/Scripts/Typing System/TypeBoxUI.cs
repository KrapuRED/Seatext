using UnityEngine;
using TMPro;

public class TypeBoxUI : MonoBehaviour
{
    public TextMeshProUGUI textUI;

    public virtual void SetWordTextUI(string text)
    {
        textUI.text = text;
    }
}
