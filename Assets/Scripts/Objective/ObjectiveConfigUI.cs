using TMPro;
using UnityEngine;

public class ObjectiveConfigUI : MonoBehaviour
{
    [Header("Objective Config UI")]
    [SerializeField] private TMP_Text nameObjective;
    [SerializeField] private TMP_Text countObjective;
    [SerializeField] private TMP_Text scoreText;
    
    public void InitialazeObjectiveUI(string objectiveName)
    {
        nameObjective.text = objectiveName;
    }
    
    public void UpdateObjectiveUI(int currentCountObjective, int maxCountObjective)
    {
        countObjective.text = $"{currentCountObjective}/{maxCountObjective}";
    }
    
    public void UpdateScoreUI(int score)
    {
        scoreText.text = $"Score: {score}";
    }

}
