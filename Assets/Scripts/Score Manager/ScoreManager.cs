using System;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private ObjectiveConfigUI objectiveConfigUI;

    [SerializeField] private int currentScore;
    
    private void Start()
    {
        if (objectiveConfigUI == null)
        {
            Debug.LogError($"objective Config UI not been assign to {gameObject.name}");
            return;
        }
        
        objectiveConfigUI.UpdateScoreUI(currentScore);
    }
}
