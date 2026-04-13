using UnityEngine;

[CreateAssetMenu(fileName = "IsFishSatisfied", menuName = "State Machine/Condition/IsFishSatisfied")]
public class IsFishSatisfied : ConditionSO
{
    public int fishSatisfied;

    public override bool CheckCondition(EnemyContex contex)
    {
        Debug.Log($"[IsFishSatisfied - CheckCondition] Food Been Eaten : {contex.enemyFish.foodBeenEaten} / Fish Satisfied : {fishSatisfied}");
        if (contex.enemyFish.foodBeenEaten == fishSatisfied)
            return true;

        return false;
    }
}
