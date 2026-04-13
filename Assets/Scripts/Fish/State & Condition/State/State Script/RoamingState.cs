using UnityEngine;

[CreateAssetMenu(fileName = "RoamingState", menuName = "State Machine/State/RoamingState")]
public class RoamingState : StateSO
{
    public override void EnterState(EnemyContex contex)
    {
        Debug.Log($"{contex.enemyFish.name} is {name}");

    }

    public override void ExcuteState(EnemyContex contex)
    {
        contex.enemyFishMovement.MoveFish(contex.endWypointPoint);
    }
}
