using UnityEngine;

public class EnemyFishTypeBox : TypingBox
{
    [Header("EnemyFishTypeBox Config")]
    [SerializeField] private FishTextUI fishUI;
    [SerializeField] private WordData _wordData;

    private void Start()
    {
        _wordData = WordBankManager.instance.GetRandomWordData();
        SetTextToType(_wordData.word);
        setTypeBoxEvent.Raise(this);
    }

    public override void SetTextToType(string text)
    {
        base.SetTextToType(text);
        fishUI.SetFishWordText(ChangeColorText());
    }

    public override bool CheckingText(string typing)
    {
        if (_indexChar >= fullText.Length)
        {
            Debug.Log("Typing already complete!");
            return false;
        }

        bool isCorrectLetter = IsCorrectLetter(typing);
        Debug.Log($"[{gameObject.name} - CheckingText] Is Correct Letter : {isCorrectLetter}");

        if (isCorrectLetter)
        {
            // Remove the correctly typed letter from the current text
            _isStillMacthing = true;
            _indexChar++;

            if (IsTextComplete())
            {
                Debug.Log($"[Fish - CheckingText] Text Is Done : {currentTextToType}");
                WordBankManager.instance.CheckWordByWordData(_wordData.word);
                TypeBoxManager.instance.RemoveTypeBox(this);
                Destroy(gameObject);
            }

            // Update the UI with the remaining text
            fishUI.SetFishWordText(ChangeColorText());
        }
        else
        {
            Debug.Log($"[Fish - CheckingText] Wrong Letter! Typed : {typing}, Expected : {fullText[0]}");
            _isStillMacthing = false;
        }

        return isCorrectLetter;
    }

    public override void ResetTypeBox()
    {
        _indexChar = 0;
        SetTextToType(currentTextToType);
    }
}
