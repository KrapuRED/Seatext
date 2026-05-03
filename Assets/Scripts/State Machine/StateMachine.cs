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
    [SerializeField] private Fish currentFish;
    [SerializeField] private List<DataStateCondtion> dataStateCondtions = new List<DataStateCondtion>();
    [SerializeField] private StateSO _activeState;

    private void Start()
    {
        foreach (var data in currentFish.FishData.dataStateCondtions)
        {
            dataStateCondtions.Add(data);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log($"[StateMachine - Update] Counting State Machine For Fish : {dataStateCondtions.Count}");
        currentFish.FishEyeSight.UpdateEyeSight();

        foreach (var data in dataStateCondtions)
        {
            //Debug.Log($"[StateMachine - Update] Checking Condition For State : {data.nameStateCondition}");
            if (data.condition.CheckCondition(currentFish.Contex))
            {
                StateSO nextState = data.state;

                if (nextState != _activeState)
                {
                    _activeState?.ExitState(currentFish.Contex);
                    _activeState = data.state;
                    _activeState.EnterState(currentFish.Contex);
                }

                break;
            }
        }

        if (_activeState != null)
        _activeState.ExcuteState(currentFish.Contex);
    }

    public void ResetStateMachine()
    {
        //Debug.Log($"[StateMachine - ResetStateMachine] Reset State Machine");
        _activeState = null;
    }
}
