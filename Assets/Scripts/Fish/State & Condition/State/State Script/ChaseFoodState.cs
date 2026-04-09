using UnityEngine;

[CreateAssetMenu(fileName = "ChaseFoodState", menuName = "State Machine/State/ChaseFoodState")]
public class ChaseFoodState : StateSO
{
    public override void ExcuteState(EnemyContex contex)
    {
        contex.enemyFishMovement.MoveFish(contex.enemyFishEyeSight.currentObject.transform);
    }
}
