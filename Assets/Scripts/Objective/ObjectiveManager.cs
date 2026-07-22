using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[System.Serializable]
public class ObjectiveData
{
    public string nameObjective; 
    public ObjectiveDataSO obnjectData;
    [Range(0, 100)] public int changeRate;
}

public class ObjectiveManager : MonoBehaviour
{
    public static ObjectiveManager Instance {get; private set;}

    [SerializeField] private string counterName;
    [SerializeField] private List<ObjectiveData> objectiveDatas = new();
    [SerializeField] private int countObjectives;
    
    [Header("References")]
    [SerializeField] private ObjectiveConfigUI objectiveConfigUI;
    
    private ObjectiveData _currentObjectiveData;

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
        _currentObjectiveData = GetRandomObjective();

        if (objectiveConfigUI == null)
        {
            Debug.LogError($"objective Config UI not been assign to {gameObject.name}");
            return;
        }
        
        objectiveConfigUI.InitialazeObjectiveUI(_currentObjectiveData.obnjectData.objectiveName);
        objectiveConfigUI.UpdateObjectiveUI(countObjectives, _currentObjectiveData.obnjectData.countObjectives);
        
        ManagerTimer.instance.AssignCounterTime(counterName, _currentObjectiveData.obnjectData.timerObjetive);
    }

    private ObjectiveData GetRandomObjective()
    {
        int roll = Random.Range(0, 100);
        int accumlate = 0;

        foreach (var objectiveData in objectiveDatas)
        {
            accumlate += objectiveData.changeRate;
            
            if (roll <= accumlate)
                return objectiveData;
        }
        
        return null;
    }

    public void UpdateObjective(FishType fishType)
    {
        if (fishType != _currentObjectiveData.obnjectData.fishType)
            return;
        
        countObjectives++;
        bool stillActive = ManagerTimer.instance.CheckCounterTime(counterName);

        if (countObjectives >= objectiveDatas.Count)
        {
            if (stillActive)
                Debug.Log("Objective is Done but over time");
            else
                Debug.Log("Objective is Done but not over time");
            
            Debug.Log($"Objective Count: {countObjectives}, {_currentObjectiveData.obnjectData.name} is done!");
            return;
        }
        
        objectiveConfigUI.UpdateObjectiveUI(countObjectives, _currentObjectiveData.obnjectData.countObjectives);
    }
}
