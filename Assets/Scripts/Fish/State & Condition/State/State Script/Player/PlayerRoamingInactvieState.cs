using UnityEngine;

[CreateAssetMenu(fileName = "PlayerRoamingInactvieState", menuName = "State Machine/Player/State/RoamingInactvieState")]

public class PlayerRoamingInactvieState : PlayerStateSO
{
    protected override void EnterState(PlayerContex contex)
    {
        Debug.Log($"[PlayerRoamingInactvieState - EnterState] Enter Roaming Inactive State");
    }

    protected override void ExcuteState(PlayerContex contex)
    {
        Debug.Log($"[PlayerRoamingInactvieState - ExcuteState] Excute Roaming Inactive State");
    }
}
