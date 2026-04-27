using UnityEngine;

[CreateAssetMenu(fileName = "RoamingState", menuName = "State Machine/State/RoamingState")]
public class RoamingState : EnemyStateSO
{
    protected override void EnterState(EnemyContex contex)
    {
        Debug.Log($"{contex.enemyFish.name} is {name}");

    }

    protected override void ExcuteState(EnemyContex contex)
    {
        contex.fishMovement.MoveFish(contex.endWaypoint, contex.fishSpeed.GetFishSpeed(0.5f));
    }

    protected override void ExitState(EnemyContex contex)
    {
        Debug.Log($"{contex.enemyFish.name} exit {name}");
    }
}
