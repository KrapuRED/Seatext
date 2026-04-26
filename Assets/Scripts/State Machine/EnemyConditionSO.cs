using UnityEngine;

public abstract class EnemyConditionSO : ConditionSO
{
    public override bool CheckCondition(FishContex contex)
    {
        if (contex is EnemyContex enemyContex)
        {
            return CheckCondition(enemyContex);
        }
        return false;
    }

    protected abstract bool CheckCondition(EnemyContex contex);
}
