using UnityEngine;

[CreateAssetMenu(fileName = "PlayerIdleState", menuName = "State Machine/Player/State/IdleState")]

public class PlayerIdleState : PlayerStateSO
{
    protected override void EnterState(PlayerContex contex)
    {
        Debug.Log($"[PlayerIdleState - EnterState] Enter Idle State");
        contex.playerFish.SetActiveFish(false);
    }

    protected override void ExcuteState(PlayerContex contex)
    {
        Debug.Log($"[PlayerIdleState - ExcuteState] Excute Idle State");
    }
}
