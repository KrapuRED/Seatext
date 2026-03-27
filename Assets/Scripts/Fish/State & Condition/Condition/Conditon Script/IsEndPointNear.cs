using UnityEngine;

[CreateAssetMenu(fileName = "IsEndPointNear", menuName = "State Machine/Condition/IsEndPointNear")]
public class IsEndPointNear : ConditionSO
{
    public float distanceToEndPoint;

    public override bool CheckCondition(EnemyContex contex)
    {
        float distance = Vector3.Distance(contex.enemyPosition.position, contex.endWypointPoint.position);
        return distance <= distanceToEndPoint;
    }
}
