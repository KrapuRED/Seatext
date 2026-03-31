using UnityEngine;

public enum TypeTypingBox
{
    none,
    UI
}
                                                                                                                                                                                                            
public class TypingBox : MonoBehaviour
{
    
    public string currentTextToType;
    [SerializeField] protected string remainingTypedText;
    [SerializeField] protected bool _isStillMacthing;
    [SerializeField] private TypeTypingBox _typeTypingBox;
    public bool IsStillMacthing => _isStillMacthing;
    public TypeTypingBox typeTypingBox => _typeTypingBox;

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
        bool isCorrectLetter = IsCorrectLetter(typing);
        Debug.Log($"[TypingBox - CheckingText] Is Correct Letter : {isCorrectLetter}");

        if (isCorrectLetter)
        {
            // Remove the correctly typed letter from the current text
            _isStillMacthing = true;
            RemoveText();
        }
        else
        {
            Debug.Log($"[TypingBox - CheckingText] Wrong Letter! Typed : {typing}, Expected : {remainingTypedText[0]}");
            _isStillMacthing = false;
        }

        return isCorrectLetter;
    }

    protected bool IsCorrectLetter(string typedText)
    {
        return remainingTypedText[0].ToString() == typedText;
   }

    public void RemoveText()
    {
        string remainingText = remainingTypedText.Remove(0, 1);
        remainingTypedText = remainingText;
        Debug.Log($"[TypingBox - RemoveText] Is Remove Letter! remaining text {remainingText}");
    }

    public bool IsTextComplete()
    {
        return remainingTypedText.Length <= 0;
    }

    public virtual void ResetTypeBox()
    {

    }
}
