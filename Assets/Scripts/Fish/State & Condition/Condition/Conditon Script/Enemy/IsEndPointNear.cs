using UnityEngine;

[CreateAssetMenu(fileName = "IsEndPointNear", menuName = "State Machine/Condition/IsEndPointNear")]
public class IsEndPointNear : EnemyConditionSO
{
    public float distanceToEndPoint;

    protected override bool CheckCondition(EnemyContex contex)
    {
        float distance = Vector3.Distance(contex.enemyPosition.position, contex.endWaypoint.position);
        return distance <= distanceToEndPoint;
    }
}
