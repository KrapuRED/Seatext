using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ButtonTypeBox : TypingBox
{
    public TextMeshProUGUI textUI;
    public UnityEvent onTextComplete;

    private void Start()
    {
        SetTextToType(currentTextToType);
        setTypeBoxEvent.Raise(this);
    }

    public override void SetTextToType(string text)
    {
        base.SetTextToType(text);
        textUI.text = currentTextToType;
    }

    public override bool CheckingText(string typing)
    {
        bool isCorrectLetter = base.CheckingText(typing);
        Debug.Log($"[ButtonTypeBox - CheckingText] Is Correct Letter : {isCorrectLetter}");
        if (isCorrectLetter)
        {
            // Update the UI with the remaining text
            textUI.text = remainingTypedText;

            if (IsTextComplete())
            {
                Debug.Log($"[ButtonTypeBox - CheckingText] Text Is Done : {currentTextToType}");
                onTextComplete.Invoke();
                ResetTypeBox();
            }
        }
        else
        {
            Debug.Log($"[ButtonTypeBox - CheckingText] Wrong Letter! Typed : {typing}, Expected : {remainingTypedText[0]}");
        }
        return isCorrectLetter;
    }

    public override void ResetTypeBox()
    {
        base.ResetTypeBox();
        textUI.text = currentTextToType;
    }
}
