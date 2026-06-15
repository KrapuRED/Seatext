using System;
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

    private void OnDestroy()
    {
        _wordData = null;
    }

    public void GetWord()
    {
        Debug.Log($"[LevelNodeTypeBox - GetWord] Get word for node {_levelNode.LevelNodeID} with word level {_wordLevel}");

        if (GameStateManager.Instance.IsLevelNodeBeenExplored(_levelNode.LevelNodeID))
        {
            Debug.Log($"[LevelNodeTypeBox - GetWord] the node {_levelNode.LevelNodeID} is been explored");
            return;
        }
        
        WordData result = WordBankManager.instance.GetRandomWordData(_wordLevel);
        _wordData = result;

        if (string.IsNullOrEmpty(result.word))
        {
            _levelNode.ResetToHidden();
        }
        
        SetTextToType(_wordData.word);
    }

    public override bool CheckingText(string typedText)
    {
        if (_indexChar >= fullText.Length)
        {
            return false;
        }

        bool isCorrectLetter = IsCorrectLetter(typedText);

        if (isCorrectLetter)
        {
            _isStillMacthing = true;
            _indexChar++;

            if (IsTextComplete())
            {
                _levelNode.SelectedLevelNode();
                RemoveWordData();
                ResetTypeBox();
            }

            _textLevelNode.SetWordTextUI(ChangeColorText());
        }
        else
        {
            ResetTypeBox();
            _isStillMacthing = false;
        }

        return isCorrectLetter;
    }

    public void RemoveWordData()
    {
        if (_wordData == null)
            return;
        
        if (_wordData.word == string.Empty)
            return;

        WordBankManager.instance.CheckWordByWordData(_wordData.word);
        TypeBoxManager.instance.RemoveTypeBox(this);

        _wordData = null;
        currentTextToType = string.Empty;
    }

    public override void SetTextToType(string text)
    {
        base.SetTextToType(text);
        _textLevelNode.SetWordTextUI(ChangeColorText());
    }

    public override void ResetTypeBox()
    {
        _indexChar = 0;
        
        if (string.IsNullOrEmpty(currentTextToType))
            return;
        
        SetTextToType(currentTextToType);
    }
}
