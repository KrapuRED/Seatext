using UnityEngine;

public class Food : TypingBox, IEatable, IPausable
{
    [Header("Food Config")]
    [SerializeField] private DropFoodSO _dropFoodSO;
    [SerializeField] private bool isCanInitilizeByStart;

    [Header("Food TypingBox Config")]
    [SerializeField] private WordLevel _wordLevel;
    [SerializeField] private TypeBoxUI _typeBoxUI;
    [SerializeField] private WordData _wordData;

    [Header("Food and Trash Movement")]
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float limitYPosition;
    [SerializeField] private bool _canMove;

    [Header("Events")]
    [SerializeField] private SetPositionPlayerEventSO setPositionPlayerEvent;

    public FoodSize foodSize { get ; set; }

    private SpriteRenderer _spriteRenderer;

    private void Start()
    {
        if (isCanInitilizeByStart)
            InitializeFood(_wordLevel, _dropFoodSO);
    }

    private void Update()
    {
        if (!_canMove)
        {
            return;
        }

        if (transform.position.y <= limitYPosition)
        {
            RemoveWord();
            PauseManager.instance.Unregister(this);
            Destroy(gameObject);
            return;
        }

        transform.Translate(Vector2.down * _moveSpeed * Time.deltaTime);
    }

    public void InitializeFood(WordLevel wordLevel, DropFoodSO foodData)
    {
        _dropFoodSO = foodData;
        _wordLevel = wordLevel;

        _wordData = WordBankManager.instance.GetRandomWordData(_wordLevel);
        PauseManager.instance.Register(this);

        if (_wordData == null)
        {
            Destroy(gameObject);
            return;
        }

        _spriteRenderer = GetComponent<SpriteRenderer>();

        _spriteRenderer.color = Color.black;
        SetTextToType(_wordData.word);
        setTypeBoxEvent.Raise(this);
        foodSize = FoodSize.None;
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
                RemoveWord();

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

    public void Eat(FishType fishType)
    {
        Debug.Log($"[Food - Eat] {gameObject.name} has been eaten! Food Type : {_dropFoodSO.foodType}");

        if (fishType == FishType.Player)
        {
            switch (_dropFoodSO.foodType)
            {
                case FoodType.Trash:
                    Debug.Log($"[Food - Eat] Trash {gameObject.name} has been eaten! Player will lose health.");
                    PlayerFish.playerFish.PlayerFishHunger.SetTrashingHungerbar(_dropFoodSO.gainStatus);
                    break;

                case FoodType.Pellet:
                    Debug.Log($"[Food - Eat] Pellet {gameObject.name} has been eaten! Player will gain some points.");
                    break;

                case FoodType.Goldenpellet:
                    Debug.Log($"[Food - Eat] Goldenpellet {gameObject.name} has been eaten! Player will gain some points.");
                    break;
            }
        }

        RemoveWord();
        Destroy(gameObject);
    }

    private void RemoveWord()
    {
        WordBankManager.instance.CheckWordByWordData(_wordData.word);
        TypeBoxManager.instance.RemoveTypeBox(this);
    }

    public void OnPause()
    {
        _canMove = false;
    }

    public void OnResume()
    {
        _canMove = true;
    }
}
