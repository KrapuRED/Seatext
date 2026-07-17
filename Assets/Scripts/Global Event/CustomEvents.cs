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

public class CustomEvents<T1, T2, T3, T4>
{
    private event Action<T1, T2, T3, T4> _action = delegate { };
    public void Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4)
    {
        _action?.Invoke(arg1, arg2, arg3, arg4);
    }
    
    public void AddListener(Action<T1, T2, T3, T4> listener)
    {
        _action += listener;
    }
    public void RemoveListener(Action<T1, T2, T3, T4> listener)
    {
        _action += listener;
    }
}
#endregion

public class GameEvents
{
    public static readonly CustomEvents OnChangeToSelectLevel = new ();
    public static readonly CustomEvents OnLevelNodeManagerReady = new();
    public static readonly CustomEvents OnMainSceneReady = new();

    #region Camere Events
    public static readonly CustomEvents<Transform> OnChangeCameraPosition = new();
    #endregion

    #region Fish Events
    public static readonly CustomEvents<IEatable, FishType, int> OnEatableEntered = new ();
    public static readonly CustomEvents<FishSpawnerType, int> OnRemoveSpawnedFishData = new ();
    
    public static readonly CustomEvents<bool, AreaSkillEffectType?, FishSkillEffectType?, float> OnApplyingSkillEffect = new();
    public static readonly CustomEvents OnStopApplyingSkillEffect = new();
    #endregion

    #region Player Fish Events
    public static readonly CustomEvents<Vector2> OnDodgeAttackFish = new();
    public static readonly CustomEvents<Transform> OnSetPositionPlayerEvent = new();
    public static readonly CustomEvents OnPlayerGainingSpeed = new();
    public static readonly CustomEvents OnPlayerEating = new();
    public static readonly CustomEvents OnSaveCurrentStatus = new();
    #endregion

    #region UI Events
    public static readonly CustomEvents OnShowUI = new();

    public static readonly CustomEvents<float, float> OnUpdateHealthBar = new();
    public static readonly CustomEvents<float, float> OnUpdateHungerBar = new();
    public static readonly CustomEvents<float> OnSetTimerGamePlay = new();
    public static readonly CustomEvents<float> OnUpdateTimerGamePlay = new();
    
    public static readonly CustomEvents<ButtonTypeBoxContext> OnButtonTypeBoxComplete = new();
    
    public static readonly CustomEvents OnUpdatePowerUpNode = new();

    //================================= PANEL =================================
    public static readonly CustomEvents<object> OnShowAdditionalInformationPanel = new();
    public static readonly CustomEvents OnHideAdditionalInformationPanel = new();

    public static readonly CustomEvents<string> OnClosePanelByID = new();
    
    #endregion

    #region GamePlay
    public static readonly CustomEvents OnPlayerDie = new();
    public static readonly CustomEvents OnEndDuration = new();
    public static readonly CustomEvents<bool> OnPlayerActive = new();

    // ================================= Currecny Manager ================================= 
    public static readonly CustomEvents<CurrecyData> OnSetCurrency = new();
    public static readonly CustomEvents<CurrecyData> OnUpdateCurrecyUI = new();

    // ================================= LEVEL NODE ================================= 
    public static readonly CustomEvents<LevelNode> OnSetLevelNode = new();
    public static readonly CustomEvents OnRemoveAllLevelNodeReferences = new();
    public static readonly CustomEvents OnGetRandomTreasureItem = new();

    public static readonly CustomEvents<LevelNode> OnSelectedNextLevelNode= new();
    public static readonly CustomEvents OnSelectedPreviousLevelNode = new();
    public static readonly CustomEvents<string> OnSetLevelNodeBeenExplored= new();
    public static readonly CustomEvents<LevelNode> OnSetNearCurrentLevelNode = new();

    // ================================= TYEP MANAGER ================================= 
    public static readonly CustomEvents<object> OnSingleTypeBoxMatch = new();

    #endregion
}
