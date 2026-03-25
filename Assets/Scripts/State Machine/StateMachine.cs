using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Transition
{
    public string name;
    public State targetState;
    public Condition condition;
}

public class StateMachine : MonoBehaviour
{
    [Header("State and Condition Config")]
    [SerializeField] private List<Transition> transitions = new List<Transition>();

    // Update is called once per frame
    void Update()
    {
        foreach (var transition in transitions)
        {
            if (transition.condition.CheckCondition())
            {
                transition.targetState.EnterState();
            }
        }
    }
}
