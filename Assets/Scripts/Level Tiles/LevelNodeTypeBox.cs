using TMPro;
using UnityEngine;

public class LevelNodeTypeBox : TypingBox
{
    [Header("Level Node TypeBox Config")]
    [SerializeField] private WordLevel _wordLevel;
    [SerializeField] private LevelNodeTextUI _textLevelNode;
    [SerializeField] private WordData _wordData;

    public void GetWord()
    {
        _wordData = WordBankManager.instance.GetRandomWordData(_wordLevel);
        SetTextToType(_wordData.word);
    }

    public override void SetTextToType(string text)
    {
        base.SetTextToType(text);
        _textLevelNode.SetWordTextUI(ChangeColorText());
    }
}
