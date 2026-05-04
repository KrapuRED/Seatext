using UnityEngine;

[CreateAssetMenu(fileName = "SatisfiedState", menuName = "State Machine/State/SatisfiedState")]
public class SatisfiedState : EnemyStateSO
{
    protected override void EnterState(EnemyContex contex)
    {
        //Debug.Log($"{contex.enemyFish.name} is {name}");
    }

    protected override void ExcuteState(EnemyContex contex)
    {
        float distance = Vector2.Distance(contex.enemyPosition.position, contex.endWaypoint.position);
        contex.fishMovement.MoveFish(contex.endWaypoint, distance ,contex.fishSpeed.GetFishSpeed(1), true);
    }

    protected override void ExitState(EnemyContex contex)
    {
        //Debug.Log($"{contex.enemyFish.name} is exiting {name}");
    }
}
