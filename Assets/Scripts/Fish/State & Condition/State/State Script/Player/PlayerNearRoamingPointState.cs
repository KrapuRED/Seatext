using UnityEngine;

[CreateAssetMenu(fileName = "PlayerNearRoamingPointState", menuName = "State Machine/Player/State/PlayerNearRoamingPoint")]

public class PlayerNearRoamingPointState : PlayerStateSO
{
   protected override void EnterState(PlayerContex contex)
    {
        //Debug.Log($"[PlayerNearRoamingPointState - EnterState] Enter Player Near Roaming Point State");
        if (contex.IsRoaming)
        {
            contex.playerFish.SetPlayerFishDirection(WaypointManager.Instance.GetRandomRoamingPoint().spawnTransform);
        }
    }

    protected override void ExcuteState(PlayerContex contex)
    {
        
    }

    protected override void ExitState(PlayerContex contex)
    {

    }
}
