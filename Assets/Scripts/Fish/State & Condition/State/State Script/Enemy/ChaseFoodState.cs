using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[CreateAssetMenu(fileName = "ChaseFoodState", menuName = "State Machine/State/ChaseFoodState")]
public class ChaseFoodState : EnemyStateSO
{
    public float distanceToEat;

    protected override void EnterState(EnemyContex contex)
    {
        //Debug.Log($"{contex.enemyFish.name} is {name}");

    }

    protected override void ExcuteState(EnemyContex contex)
    {
        Transform foodPosition = contex.enemyFishEyeSight.currentObject?.transform;
        
        if (foodPosition == null && foodPosition.CompareTag("Player"))
            return;

            contex.fishMovement.MoveFish(foodPosition, contex.fishSpeed.GetFishSpeed(1));
    }
    protected override void ExitState(EnemyContex contex)
    {
        //Debug.Log($"{contex.enemyFish.name} exit {name}");
    }
}
