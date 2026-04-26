using UnityEngine;

[CreateAssetMenu(fileName = "SatisfiedState", menuName = "State Machine/State/SatisfiedState")]
public class SatisfiedState : EnemyStateSO
{
    protected override void EnterState(EnemyContex contex)
    {
        Debug.Log($"{contex.enemyFish.name} is {name}");
    }

    protected override void ExcuteState(EnemyContex contex)
    {
        contex.fishMovement.MoveFish(contex.endWaypoint, contex.fishSpeed.GetFishSpeed(1), true);
    }
}
