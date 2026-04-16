using UnityEngine;

[CreateAssetMenu(fileName = "SatisfiedState", menuName = "State Machine/State/SatisfiedState")]
public class SatisfiedState : StateSO
{
    public override void EnterState(EnemyContex contex)
    {
        Debug.Log($"{contex.enemyFish.name} is {name}");
    }

    public override void ExcuteState(EnemyContex contex)
    {
        contex.enemyFishMovement.MoveFish(contex.endWypointPoint, contex.enemyFishSpeed.GetFishSpeed(1), true);
    }
}
