using UnityEngine;

public abstract class StateSO : ScriptableObject
{
    public abstract void EnterState(FishContex contex);

    public abstract void ExcuteState(FishContex contex);

    public abstract void ExitState(FishContex contex);
}
