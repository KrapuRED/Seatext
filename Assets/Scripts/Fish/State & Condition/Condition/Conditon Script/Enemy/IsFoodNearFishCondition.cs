using UnityEngine;


[CreateAssetMenu(fileName = "IsFoodNearFishCondition", menuName = "State Machine/Condition/IsFoodNearFishCondition")]
public class IsFoodNearFishCondition : EnemyConditionSO
{
    public float distanceToEat;
    public float durationChasingFood;

    protected override bool CheckCondition(EnemyContex contex)
    {
        Transform target = contex.enemyFishEyeSight.currentObject;

        if (target != null)
        {
            float distance = Vector2.Distance(contex.enemyFish.transform.position, target.position);
            //Debug.Log($"[IsFoodNearFishCondition - CheckCondition] Distance To Target : {distance} | Distance To GetEatenBy : {distanceToEat}");
            if (target.TryGetComponent(out IEatable food) && distance <= distanceToEat)
            {
                if (food.foodSize < contex.enemyFish.foodSize)
                    return true;
            }
        }
        return false;
    }
}
