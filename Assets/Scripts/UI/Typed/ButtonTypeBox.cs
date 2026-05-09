using TMPro;
using UnityEngine;
using UnityEngine.Events;

public enum ButtonTypeBoxContext
{
    ClosePanel,
    OpenPanel,
    ExploreNode,
    DoneExploreNode
}

public class ButtonTypeBox : TypingBox
{
    public TextMeshProUGUI textUI;
    public UnityEvent onTextComplete;
    [SerializeField] private ButtonTypeBoxContext buttonTypeBoxContext;
    
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
        //Debug.Log($"[ButtonTypeBox - CheckingText] Is Correct Letter : {isCorrectLetter}");
        if (isCorrectLetter)
        {
            // Update the UI with the remaining text
            textUI.text = fullText;
            _indexChar++;

            if (IsTextComplete())
            {
                //Debug.Log($"[ButtonTypeBox - CheckingText] Text Is Done : {currentTextToType}");
                onTextComplete.Invoke();
                GameEvents.OnButtonTypeBoxComplete.Invoke(buttonTypeBoxContext);
                ResetTypeBox();
            }

            textUI.text = ChangeColorText();
        }
        else
        {
            //Debug.Log($"[ButtonTypeBox - CheckingText] Wrong Letter! Typed : {typing}, Expected : {fullText[_indexChar]}");
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
}
