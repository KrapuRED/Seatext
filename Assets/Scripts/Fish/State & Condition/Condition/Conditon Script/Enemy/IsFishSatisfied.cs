using UnityEngine;

[CreateAssetMenu(fileName = "IsFishSatisfied", menuName = "State Machine/Condition/IsFishSatisfied")]
public class IsFishSatisfied : EnemyConditionSO
{
    public int fishSatisfied;

    protected override bool CheckCondition(EnemyContex contex)
    {
        Debug.Log($"[IsFishSatisfied - CheckCondition] Food Been Eaten : {contex.enemyFish.FoodBeenEaten} / Fish Satisfied : {fishSatisfied}");
        if (contex.enemyFish.FoodBeenEaten == fishSatisfied)
            return true;

        return false;
    }
}
