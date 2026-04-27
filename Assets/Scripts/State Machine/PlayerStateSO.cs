using UnityEngine;

public abstract class PlayerStateSO : StateSO
{
    public override void EnterState(FishContex contex)
    {
        if (contex is PlayerContex enemyContex)
        {
            EnterState(enemyContex);
        }
    }

    public override void ExcuteState(FishContex contex)
    {
        if (contex is PlayerContex enemyContex)
        {
            ExcuteState(enemyContex);
        }
    }

    public override void ExitState(FishContex contex)
    {
        if (contex is PlayerContex enemyContex)
        {
            ExitState(enemyContex);
        }
    }

    protected abstract void EnterState(PlayerContex contex);
    protected abstract void ExcuteState(PlayerContex contex);
    protected abstract void ExitState(PlayerContex contex);
}
