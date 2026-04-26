using UnityEngine;

[CreateAssetMenu(fileName = "IsEndPointFar", menuName = "State Machine/Condition/IsEndPointFar")]
public class IsEndPointFar : EnemyConditionSO
{
    public float distance;

    protected override bool CheckCondition(EnemyContex contex)
    {
        float distanceToEndPoint = Vector2.Distance(contex.enemyPosition.position, contex.endWaypoint.position);
        return distanceToEndPoint > distance;
    }
}
