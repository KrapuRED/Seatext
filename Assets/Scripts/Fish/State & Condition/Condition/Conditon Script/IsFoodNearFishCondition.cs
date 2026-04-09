using UnityEngine;


[CreateAssetMenu(fileName = "IsFoodNearFishCondition", menuName = "State Machine/Condition/IsFoodNearFishCondition")]
public class IsFoodNearFishCondition : ConditionSO
{
    public override bool CheckCondition(EnemyContex contex)
    {
        bool seeFood;
        if (contex.enemyFishEyeSight.currentObject != null)
        {
            seeFood = true;
        }
        else
        {
            seeFood = false;
        }

        Debug.Log("IsFoodNearFishCondition: " + seeFood);

        return seeFood;
    }
}
