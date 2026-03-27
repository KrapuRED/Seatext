using UnityEngine;

public abstract class ConditionSO : ScriptableObject
{
    public abstract bool CheckCondition(EnemyContex contex);
}
