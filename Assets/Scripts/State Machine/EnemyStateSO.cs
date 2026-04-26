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

    protected abstract void EnterState(EnemyContex contex);
    protected abstract void ExcuteState(EnemyContex contex);
}
