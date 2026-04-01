using UnityEngine;
                                                                                                                                                                                                            
public class TypingBox : MonoBehaviour
{
    
    public string currentTextToType;
    [SerializeField] protected string fullText;
    [SerializeField] protected bool _isStillMacthing;
    [SerializeField] private TypeTypingBox _typeTypingBox;
    [SerializeField] protected int _indexChar;
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
        fullText = text;
    }

    public virtual bool CheckingText(string typedText)
    {
        if (_indexChar >= fullText.Length) return false;

        return fullText[_indexChar].ToString() == typedText;
    }

    protected bool IsCorrectLetter(string typedText)
    {
        if (_indexChar >= fullText.Length)
            return false;

        return fullText[_indexChar].ToString() == typedText;
    }

   public string ChangeColorText()
{
    string result = string.Empty;

    for (int i = 0; i < fullText.Length; i++)
    {
        char c = fullText[i];

        if (i < _indexChar)
        {
            result += $"<color=green>{c}</color>";
        }
        else
        {
                result += fullText[i];
        }
    }

    return result;
}

    public bool IsTextComplete()
    {
        return _indexChar >= fullText.Length;
    }

    public virtual void ResetTypeBox()
    {

    }
}
