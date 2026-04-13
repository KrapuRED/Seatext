using UnityEngine;

public abstract class StateSO : ScriptableObject
{
    public abstract void EnterState(EnemyContex contex);

    public abstract void ExcuteState(EnemyContex contex);
}
