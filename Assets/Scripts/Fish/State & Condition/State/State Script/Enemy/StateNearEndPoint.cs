using UnityEngine;

[CreateAssetMenu(fileName = "StateNearEndPoint", menuName = "State Machine/State/StateNearEndPoint")]
public class StateNearEndPoint : EnemyStateSO
{
    protected override void EnterState(EnemyContex contex)
    {
        //Debug.Log($"{contex.enemyFish.name} is {name}");
    }

    protected override void ExcuteState(EnemyContex contex)
    {
        contex.enemyFishTypeBox.RemoveWordFromFish();
        GameEvents.OnRemoveSpawnedFishData.Invoke(contex.EnemyFishSpawnerType ,contex.foodIndex);
        Destroy(contex.fishObject);
    }
    protected override void ExitState(EnemyContex contex)
    {
        //Debug.Log($"{contex.enemyFish.name} exit {name}");
    }
}
