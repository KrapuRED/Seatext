using UnityEngine;

[CreateAssetMenu(fileName = "IsChargeAttack", menuName = "State Machine/Condition/IsChargeAttack")]
public class IsChargeAttack : ConditionSO
{
    public override bool CheckCondition(EnemyContex contex)
    {
        return true;
    }
}
