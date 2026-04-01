using UnityEngine;
using TMPro;

public class TypeBoxUI : MonoBehaviour
{
    public TextMeshProUGUI textUI;

    public virtual void SetFishWordText(string text)
    {
        textUI.text = text;
    }
}
