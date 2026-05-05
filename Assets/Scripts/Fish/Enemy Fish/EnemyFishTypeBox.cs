using UnityEngine;

public class EnemyFishTypeBox : TypingBox
{
    [Header("EnemyFishTypeBox Config")]
    [SerializeField] private EnemyFish currentEnemyFish;
    [SerializeField] private WordLevel _wordLevel;
    [SerializeField] private FishTextUI fishUI;
    [SerializeField] private WordData _wordData;
    [SerializeField] private TextAnimation _textAnimation;
    
    [Header("Events")]
    [SerializeField] private SetPositionPlayerEventSO setPositionPlayerEvent;

    private void Start()
    {
        SetWordData();
        setTypeBoxEvent.Raise(this);
    }

    private void SetWordData()
    {
        _wordData = WordBankManager.instance.GetRandomWordData(_wordLevel);

        if (_wordData == null)
        {
            Destroy(gameObject);
            return;
        }

        SetTextToType(_wordData.word);
        _textAnimation.RefreshText(_wordData.word);
    }

    public override void SetTextToType(string text)
    {
        base.SetTextToType(text);
        fishUI.SetWordTextUI(ChangeColorText());
    }

    public override bool CheckingText(string typedText)
    {
        if (_indexChar >= fullText.Length)
        {
            Debug.Log("Typing already complete!");
            return false;
        }

        bool isCorrectLetter = IsCorrectLetter(typedText);

        if (isCorrectLetter)
        {
            _isStillMacthing = true;
            _textAnimation.OnPlayChangeTextAnimation(_indexChar);
            _indexChar++;

            if (IsTextComplete())
            {
                if (PlayerFish.playerFish.IsBeenHunted)
                {
                   GameEvents.OnDodgeAttackFish?.Invoke(currentEnemyFish.Contex.fishEyeSight.AttackDirection);
                }
                else
                {
                    setPositionPlayerEvent.OnRaise(transform);
                    GameEvents.OnSetPositionPlayerEvent?.Invoke(transform);
                }

                //reset to get new word
                ResetTypeBox();
                GameEvents.OnPlayerGainingSpeed.Invoke();
                WordBankManager.instance.CheckWordByWordData(_wordData.word);
                SetWordData();
            }

            // Update the UI with the remaining text
            fishUI.SetWordTextUI(ChangeColorText());
        }
        else
        {
            _isStillMacthing = false;
        }

        return isCorrectLetter;
    }

    public void RemoveWordFromFish()
    {
        WordBankManager.instance.CheckWordByWordData(_wordData.word);
        TypeBoxManager.instance.RemoveTypeBox(this);
    }

    public override void ResetTypeBox()
    {
        _indexChar = 0;
        SetTextToType(currentTextToType);
        _textAnimation.RefreshText(_wordData.word);
    }
}
