using UnityEngine;
using System;

#region CustomEvents
public class CustomEvents
{
    private event Action _action = delegate { };

    public void Invoke()
    {
        _action?.Invoke();
    }
    
    public void AddListener(Action listener)
    {
        _action += listener;
    }
    public void RemoveListener(Action listener)
    {
        _action += listener;
    }
}

public class CustomEvents<T>
{
    private event Action<T> _action = delegate { };
    public void Invoke(T arg)
    {
        _action?.Invoke(arg);
    }
    
    public void AddListener(Action<T> listener)
    {
        _action += listener;
    }
    public void RemoveListener(Action<T> listener)
    {
        _action += listener;
    }
}

public class CustomEvents<T1, T2>
{
    private event Action<T1, T2> _action = delegate { };
    public void Invoke(T1 arg1, T2 arg2)
    {
        _action?.Invoke(arg1, arg2);
    }
    
    public void AddListener(Action<T1, T2> listener)
    {
        _action += listener;
    }
    public void RemoveListener(Action<T1, T2> listener)
    {
        _action += listener;
    }
}

public class CustomEvents<T1, T2, T3>
{
    private event Action<T1, T2, T3> _action = delegate { };
    public void Invoke(T1 arg1, T2 arg2, T3 arg3)
    {
        _action?.Invoke(arg1, arg2, arg3);
    }
    
    public void AddListener(Action<T1, T2, T3> listener)
    {
        _action += listener;
    }
    public void RemoveListener(Action<T1, T2, T3> listener)
    {
        _action += listener;
    }
}
#endregion

public class GameEvents
{
    public static readonly CustomEvents OnChangeToSelectLevel = new ();

    #region Fish Events
    public static readonly CustomEvents<IEatable, FishType, int> OnEatableEntered = new ();
    public static readonly CustomEvents<int> OnRemoveSpawnedFishData = new ();
    #endregion

    #region Player Fish Events
    public static readonly CustomEvents<Vector2> OnDodgeAttackFish = new();
    public static readonly CustomEvents<Transform> OnSetPositionPlayerEvent = new();
    public static readonly CustomEvents OnPlayerGainingSpeed = new();
    public static readonly CustomEvents OnPlayerEating = new();
    #endregion

    #region UI Events
    public static readonly CustomEvents<float, float> OnUpdateHealthBar = new();
    public static readonly CustomEvents<float, float> OnUpdateHungerBar = new();
    public static readonly CustomEvents<float> OnSetTimerGamePlay = new();
    public static readonly CustomEvents<float> OnUpdateTimerGamePlay = new();
    #endregion

    #region Game Play
    public static readonly CustomEvents OnPlayerDie = new();
    public static readonly CustomEvents<bool> OnPlayerActive = new();
    #endregion
}
