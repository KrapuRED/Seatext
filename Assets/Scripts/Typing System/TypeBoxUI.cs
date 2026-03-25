using UnityEngine;
using TMPro;

public class TypeBoxUI : MonoBehaviour
{
    public TextMeshProUGUI fishWordText;

    public virtual void SetFishWordText(string text)
    {
        fishWordText.text = text;
    }
}
