using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class PowerUpTypeBox : TypingBox
{
    public PowerUpNode ownerNode;
    
    public TextMeshProUGUI textUI;
    public UnityEvent onTextComplete;
    
    public override void SetTextToType(string text)
    {
        setTypeBoxEvent.Raise(this);
        
        base.SetTextToType(text);
        textUI.text = currentTextToType;
    }

    public override bool CheckingText(string typing)
    {
        if (!ownerNode.CanBuy)
            return false;
        
        bool isCorrectLetter = base.CheckingText(typing);
        if (isCorrectLetter)
        {
            textUI.text = fullText;
            _indexChar++;

            if (IsTextComplete())
            {
                OnInkoveEvent();
                ResetTypeBox();
            }

            textUI.text = ChangeColorText();
        }
        else
        {
            ResetTypeBox();
        }
        return isCorrectLetter;
    }

    public override void ResetTypeBox()
    {
        base.ResetTypeBox();
        _indexChar = 0;
        textUI.text = currentTextToType;
    }

    public virtual void OnInkoveEvent()
    {
        onTextComplete?.Invoke();
    }
}
