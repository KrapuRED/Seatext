using UnityEngine;

[CreateAssetMenu(fileName = "PlayerIdleState", menuName = "State Machine/Player/State/IdleState")]

public class PlayerIdleState : PlayerStateSO
{
    protected override void EnterState(PlayerContex contex)
    {
        //Debug.Log($"[PlayerIdleState - EnterState] Enter Idle State");
        contex.IsIdle = true;
        contex.fishMouth.SetMouthState(false);
    }

    protected override void ExcuteState(PlayerContex contex)
    {
        //Debug.Log($"[PlayerIdleState - ExcuteState] Excute Idle State");
    }

    protected override void ExitState(PlayerContex contex)
    {
        Debug.Log($"[PlayerIdleState - ExitState] Exit Idle State");
    }
}
