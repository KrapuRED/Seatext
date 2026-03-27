using UnityEngine;

public abstract class StateSO : ScriptableObject
{
    public abstract void ExcuteState(EnemyContex contex);
}
