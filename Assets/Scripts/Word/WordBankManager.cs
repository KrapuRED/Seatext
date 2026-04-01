using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public enum WordLevel
{
    none,
    easy,
    medium,
    hard
}

[System.Serializable]
public class WordData
{
    public string word;
    public WordLevel wordLevel;
}

public class WordBankManager : MonoBehaviour
{
    public static WordBankManager instance;

    [Header("Word Config")]
    [SerializeField] private List<string> _word = new List<string>();
    [SerializeField] private List<WordData> _activeWordData = new List<WordData>();
    [SerializeField] private List<WordData> _inactiveWordData = new List<WordData>();

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        SetActiveWordData();
    }

    private void SetActiveWordData()
    {
        foreach (var word in _word)
        {
            int lenght = word.Length;
            _inactiveWordData.Add(CreateWordData(word, lenght));
        }
    }

    private WordData CreateWordData(string word, int lenght)
    {
       WordData newWordData = new WordData();
       newWordData.word = word;

        switch (lenght)
        {
            case <= 6:
                newWordData.wordLevel = WordLevel.easy;
                break;
            case <= 9:
                newWordData.wordLevel = WordLevel.medium;
                break;
            case > 9:
                newWordData.wordLevel = WordLevel.hard;
                break;
        }

        return newWordData;
    }

    private void RemoveWordDataFromActive(WordData wordData)
    {
        if (!_activeWordData.Contains(wordData))
        {
            Debug.LogWarning($"WordData : {wordData.word} those not contain in the activeword data!");
            return;
        }

        _activeWordData.Remove(wordData);
        _inactiveWordData.Add(wordData);
    }

    private void RemoveWordDataFromInactive(WordData wordData)
    {
        if (!_inactiveWordData.Contains(wordData))
        {
            Debug.LogWarning($"WordData : {wordData.word} those not contain in the activeword data!");
            return;
        }

        _inactiveWordData.Remove(wordData);
        _activeWordData.Add(wordData);
    }

    public WordData GetRandomWordData(WordLevel worldLevel)
    {
        //Find the all word with the same level and get random word data from it
        var validWords = _inactiveWordData.Where(wordData => wordData.wordLevel == worldLevel).ToList();

        //Check if there is any valid word data available for the requested level
        if (validWords.Count == 0)
        {
            Debug.LogWarning("[WordBankManager - GetRandomWordData] No available words for this level!");
            return null;
        }

        int randomIndex = Random.Range(0, validWords.Count);
        WordData availableWord = validWords[randomIndex];

        RemoveWordDataFromInactive(availableWord);
        return availableWord;
    }

    public void CheckWordByWordData(string typedWord)
    {
        foreach (var wordData in _activeWordData)
        {
            if (wordData.word == typedWord)
            {
                Debug.Log($"[WordBankManager - CheckWordByWordData] Typed word : {typedWord} is correct!");
                RemoveWordDataFromActive(wordData);
                return;
            }
        }
    }
}
