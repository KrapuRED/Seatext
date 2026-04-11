using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[CreateAssetMenu(fileName = "ChaseFoodState", menuName = "State Machine/State/ChaseFoodState")]
public class ChaseFoodState : StateSO
{
    public float distanceToEat;

    public override void ExcuteState(EnemyContex contex)
    {
        Transform foodPosition = contex.enemyFishEyeSight.currentObject?.transform;
        
        if (foodPosition == null)
            return;

            contex.enemyFishMovement.MoveFish(foodPosition);
    }
}
