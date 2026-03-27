using UnityEngine;

[CreateAssetMenu(fileName = "RoamingState", menuName = "State Machine/State/RoamingState")]
public class RoamingState : StateSO
{
    public override void ExcuteState(EnemyContex contex)
    {
        contex.enemyFishMovement.MoveFish(contex.endWypointPoint);
    }
}
