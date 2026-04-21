using System.Net.NetworkInformation;
using TMPro;
using UnityEngine;

public class LevelNodeTypeBox : TypingBox
{
    [Header("Level Node TypeBox Config")]
    [SerializeField] private WordLevel _wordLevel;
    [SerializeField] private LevelNode _levelNode;
    [SerializeField] private LevelNodeTextUI _textLevelNode;
    [SerializeField] private WordData _wordData;

    public void GetWord()
    {
        //Debug.Log("[LevelNodeTypeBox - GetWord] Get Word CALL by Level Node : " + _levelNode.name);
        if (_levelNode.levelNodeState != LevelNodeState.Visited)
        {
            _wordData = WordBankManager.instance.GetRandomWordData(_wordLevel);
            SetTextToType(_wordData.word);
        }
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
            _indexChar++;

            if (IsTextComplete())
            {
                Debug.Log($"[LevelNodeTypeBox - CheckingText] Text Is Done : {currentTextToType}");
                _levelNode.SetPlayerHere();
                RemoveWordData();
                ResetTypeBox();
            }

            _textLevelNode.SetWordTextUI(ChangeColorText());
        }
        else
        {
            //Debug.Log($"[Fish - CheckingText] Wrong Letter! Typed : {typedText}, Expected : {fullText[0]}");
            ResetTypeBox();
            _isStillMacthing = false;
        }

        return isCorrectLetter;
    }

    public void RemoveWordData()
    {
        if (_wordData.word == string.Empty)
            return;

        WordBankManager.instance.CheckWordByWordData(_wordData.word);
        TypeBoxManager.instance.RemoveTypeBox(this);

        _wordData = null;
        currentTextToType = "";
    }

    public override void SetTextToType(string text)
    {
        base.SetTextToType(text);
        _textLevelNode.SetWordTextUI(ChangeColorText());
    }

    public override void ResetTypeBox()
    {
        _indexChar = 0;
        SetTextToType(currentTextToType);
    }
}
