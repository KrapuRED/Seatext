using UnityEngine;

public class TypeBox : MonoBehaviour
{
    [SerializeField] private string currentTextToType;
    [SerializeField] private string remainingTypedText;
    [SerializeField] private SetTypeBoxEventSO _setTypeBoxEvent;

    private void Start()
    {
        _setTypeBoxEvent.Raise(this);
    }
    public void SetTextToType(string text)
    {
        currentTextToType = text;
    }

    public virtual void CheckingText(string typing)
    {
        Debug.Log($"[TypeBox - CheckingText] Is Correct Letter : {IsCorrectLetter(typing)}");
    }
    private bool IsCorrectLetter(string typedText)
    {
        return currentTextToType.Contains(typedText);
    }
    private bool IsTextComplete()
    {
        return false;
    }
}
