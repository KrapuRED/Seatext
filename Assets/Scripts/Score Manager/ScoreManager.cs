using System;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance {get; private set;}
    
    [SerializeField] private ObjectiveConfigUI objectiveConfigUI;

    [SerializeField] private int currentScore;

    private ComboManager _comboManager;
    
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (objectiveConfigUI == null)
        {
            Debug.LogError($"objective Config UI not been assign to {gameObject.name}");
            return;
        }
        
        objectiveConfigUI.UpdateScoreUI(currentScore);
    }

    private int CalculateScore(int scoreValue)
    {
        float multiplierTyping = _comboManager.ValueMultiplierTyping; // float
        float multiplierEating = _comboManager.ValueMultiplierEating; // float
        float finalMultiplier = 0;
        
        bool hasMultiplierTyping = multiplierTyping > 0;
        bool hasMultiplierEating = multiplierEating > 0;

        if (hasMultiplierTyping && hasMultiplierEating)
            finalMultiplier = multiplierEating * multiplierTyping;
        else if (hasMultiplierEating)
            finalMultiplier = multiplierEating;
        else if (hasMultiplierTyping)
            finalMultiplier = multiplierTyping;
        else
        {
            finalMultiplier = 1f;
        }
        
        int newScore = Mathf.RoundToInt(scoreValue * finalMultiplier);

        return newScore;
    }

    public void UpdateScore(int scoreValue)
    {
        if (_comboManager == null)
            _comboManager = ComboManager.instance;
        
        currentScore += CalculateScore(scoreValue);
        
        objectiveConfigUI.UpdateScoreUI(currentScore);
    }
}
