using UnityEngine;

[CreateAssetMenu(fileName = "PlayerRoamingInactvieState", menuName = "State Machine/Player/State/RoamingInactvieState")]

public class PlayerRoamingInactvieState : PlayerStateSO
{
    [SerializeField] private float distanceToRoamingPoint = 1f;

    protected override void EnterState(PlayerContex contex)
    {
        Debug.Log($"[PlayerRoamingInactvieState - EnterState] Enter Roaming Inactive State");
        contex.IsRoaming = true;

        contex.playerFish.SetPlayerFishDirection(WaypointManager.Instance.GetRandomRoamingPoint().spawnTransform);
    }

    protected override void ExcuteState(PlayerContex contex)
    {
        Debug.Log($"[PlayerRoamingInactvieState - ExcuteState] Excute Roaming Inactive State");
        if(contex.RoamingPoint == null)
            return;

        float distance = Vector2.Distance(contex.playerFish.transform.position, contex.RoamingPoint.position);

        if (distance <= distanceToRoamingPoint)
        {
            contex.playerFish.SetPlayerFishDirection(WaypointManager.Instance.GetRandomRoamingPoint().spawnTransform);
        return;
        }
            
    }
    protected override void ExitState(PlayerContex contex)
    {
        Debug.Log($"[PlayerRoamingInactvieState - ExitState] Exit Roaming Inactive State");
    }
}
