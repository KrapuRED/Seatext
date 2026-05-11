using UnityEngine;

[CreateAssetMenu(fileName = "IsFishSatisfied", menuName = "State Machine/Condition/IsFishSatisfied")]
public class IsFishSatisfied : EnemyConditionSO
{
    public int fishSatisfied;

    protected override bool CheckCondition(EnemyContex contex)
    {
        if (contex.enemyFish.FoodBeenEaten >= fishSatisfied)
            return true;

        return false;
    }
}
