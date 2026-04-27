using UnityEngine;

[CreateAssetMenu(fileName = "PlayerRoamingActiveState", menuName = "State Machine/Player/Condition/RoamingActiveState")]

public class PlayerRoamingActiveState : PlayerStateSO
{
    protected override void EnterState(PlayerContex contex)
    {
        Debug.Log($"[PlayerRoamingActiveState - EnterState] Enter Roaming Active State");
        contex.CanEatFish = true;
    }

    protected override void ExcuteState(PlayerContex contex)
    {
        Debug.Log($"[PlayerRoamingActiveState - ExcuteState] Excute Roaming Active State");
    }

    protected override void ExitState(PlayerContex contex)
    {
        Debug.Log($"[PlayerRoamingActiveState - ExitState] Exit Roaming Active State");
    }
}
