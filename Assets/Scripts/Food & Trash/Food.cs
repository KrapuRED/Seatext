using UnityEngine;

public class Food : TypingBox, IEatable
{
    [Header("Food TypingBox Config")]
    [SerializeField] private WordLevel _wordLevel;
    [SerializeField] private TypeBoxUI _typeBoxUI;
    [SerializeField] private WordData _wordData;

    [Header("Food and Trash Movement")]
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float limitYPosition;

    [Header("Events")]
    [SerializeField] private SetPositionPlayerEventSO setPositionPlayerEvent;

    public bool IsEdible { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    private void Update()
    {
        if (transform.position.y <= limitYPosition)
        {
            WordBankManager.instance.CheckWordByWordData(_wordData.word);
            TypeBoxManager.instance.RemoveTypeBox(this);

            Destroy(gameObject);
            return;
        }

        transform.Translate(Vector2.down * _moveSpeed * Time.deltaTime);
    }

    public void InitializeFood(WordLevel wordLevel)
    {
        _wordLevel = wordLevel;
        _wordData = WordBankManager.instance.GetRandomWordData(_wordLevel);

        if (_wordData == null)
        {
            Destroy(gameObject);
            return;
        }

        SetTextToType(_wordData.word);
        setTypeBoxEvent.Raise(this);
    }

    public override void SetTextToType(string text)
    {
        base.SetTextToType(text);
        _typeBoxUI.SetWordTextUI(ChangeColorText());
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
                Debug.Log($"[{gameObject.name} - CheckingText] Text Is Done : {currentTextToType}");
                WordBankManager.instance.CheckWordByWordData(_wordData.word);
                TypeBoxManager.instance.RemoveTypeBox(this);
                //call event to set this position to player fish
                setPositionPlayerEvent.OnRaise(transform);
            }

            // Update the UI with the remaining text
            _typeBoxUI.SetWordTextUI(ChangeColorText());
        }
        else
        {
            Debug.Log($"[{gameObject.name} - CheckingText] Wrong Letter! Typed : {typing}, Expected : {fullText[0]}");
            _isStillMacthing = false;
        }

        return isCorrectLetter;
    }

    public override void ResetTypeBox()
    {
        _indexChar = 0;
        SetTextToType(currentTextToType);
    }

    public void Eat()
    {
        Debug.Log($"[Food - Eat] Food {gameObject.name} has been eaten!");
        gameObject.SetActive(false);
    }
}
