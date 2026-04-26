using UnityEngine;

[CreateAssetMenu(fileName = "StateNearEndPoint", menuName = "State Machine/State/StateNearEndPoint")]
public class StateNearEndPoint : EnemyStateSO
{
    protected override void EnterState(EnemyContex contex)
    {
        Debug.Log($"{contex.enemyFish.name} is {name}");
    }

    protected override void ExcuteState(EnemyContex contex)
    {
        contex.enemyFishTypeBox.RemoveWordFromFish();
        Destroy(contex.fishObject);
    }
}
