using UnityEngine;

[CreateAssetMenu(fileName = "IsChargeAttack", menuName = "State Machine/Condition/IsChargeAttack")]
public class IsChargeAttack : EnemyConditionSO
{
    protected override bool CheckCondition(EnemyContex contex)
    {
        return true;
    }
}
