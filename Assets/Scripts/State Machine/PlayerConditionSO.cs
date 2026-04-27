using UnityEngine;

public abstract class PlayerConditionSO : ConditionSO
{
    public override bool CheckCondition(FishContex contex)
    {
        if (contex is PlayerContex playerContex)
        {
            return CheckCondition(playerContex);
        }
        return false;
    }

    protected abstract bool CheckCondition(PlayerContex contex);
}
