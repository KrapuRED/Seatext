using UnityEngine;

[CreateAssetMenu(fileName = "StateNearEndPoint", menuName = "State Machine/State/StateNearEndPoint")]
public class StateNearEndPoint : StateSO
{
    public override void ExcuteState(EnemyContex contex)
    {
        Debug.Log("State Near End Point");
        Destroy(contex.enemyObject);
    }
}
