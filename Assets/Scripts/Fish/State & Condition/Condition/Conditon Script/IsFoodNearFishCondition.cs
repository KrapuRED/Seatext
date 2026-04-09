using UnityEngine;


[CreateAssetMenu(fileName = "IsFoodNearFishCondition", menuName = "State Machine/Condition/IsFoodNearFishCondition")]
public class IsFoodNearFishCondition : ConditionSO
{
    public override bool CheckCondition(EnemyContex contex)
    {
        FishOS food = contex.enemyFishEyeSight.currentObject.GetComponent<FishOS>();

        bool seeFood;
        if ( food == null )
        {
            seeFood = false;
        }
        else
        {
            seeFood = true;
        }

        Debug.Log("IsFoodNearFishCondition: " + seeFood);

        return seeFood;
    }
}
