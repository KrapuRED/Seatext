using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class DataStateCondtion
{
    public string nameStateCondition;
    public string stateConditionID;
    public StateSO state;
    public ConditionSO condition;
}
public class StateMachine : MonoBehaviour
{
    [Header("State and Condition Config")]
    [SerializeField] private EnemyFish currentFish;
    [SerializeField] private List<DataStateCondtion> dataStateCondtions = new List<DataStateCondtion>();
    [SerializeField] private StateSO _activeState;

    private void Awake()
    {
        foreach (var data in currentFish.fishData.dataStateCondtions)
        {
            dataStateCondtions.Add(data);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log($"[StateMachine - Update] Counting State Machine For Fish : {dataStateCondtions.Count}");

        foreach (var data in dataStateCondtions)
        {
            if (data.condition.CheckCondition(currentFish.Contex))
            {
                _activeState = data.state;
                _activeState.ExcuteState(currentFish.Contex);
                break; // Exit the loop after finding the first valid state
            }
        }
    }
}
