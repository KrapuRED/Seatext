using UnityEngine;

public abstract class EnemyStateSO : StateSO
{
    public override void EnterState(FishContex contex)
    {
        if (contex is EnemyContex enemyContex)
        {
            EnterState(enemyContex);
        }
    }

    public override void ExcuteState(FishContex contex)
    {
        if (contex is EnemyContex enemyContex)
        {
            ExcuteState(enemyContex);
        }
    }

    public override void ExitState(FishContex contex)
    {
        if (contex is EnemyContex enemyContex)
        {
            ExitState(enemyContex);
        }
    }

    protected abstract void EnterState(EnemyContex contex);
    protected abstract void ExcuteState(EnemyContex contex);
    protected abstract void ExitState(EnemyContex contex);
}
