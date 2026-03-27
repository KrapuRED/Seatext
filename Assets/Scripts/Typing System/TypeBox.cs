using UnityEngine;

public class TypeBox : MonoBehaviour
{
    public string currentTextToType;
    [SerializeField] protected string remainingTypedText;
    [SerializeField] protected bool _isStillMacthing;
    public bool IsStillMacthing => _isStillMacthing;

    public SetTypeBoxEventSO setTypeBoxEvent;

    private void Start()
    {
        setTypeBoxEvent.Raise(this);
    }
    public virtual void SetTextToType(string text)
    {
        currentTextToType = text;
        remainingTypedText = text;
    }

    public virtual bool CheckingText(string typing)
    {
        return false;
    }

    protected bool IsCorrectLetter(string typedText)
    {
        return remainingTypedText[0].ToString() == typedText;
    }

    public void RemoveText()
    {
        string remainingText = remainingTypedText.Remove(0, 1);
        remainingTypedText = remainingText;
        Debug.Log($"[TypeBox - RemoveText] Is Remove Letter! remaining text {remainingText}");
    }

    public bool IsTextComplete()
    {
        return remainingTypedText.Length <= 0;
    }

    public virtual void ResetTypeBox()
    {

    }
}
