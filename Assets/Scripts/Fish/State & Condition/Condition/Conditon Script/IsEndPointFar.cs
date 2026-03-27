using UnityEngine;

[CreateAssetMenu(fileName = "IsEndPointFar", menuName = "State Machine/Condition/IsEndPointFar")]
public class IsEndPointFar : ConditionSO
{
    public float distance;

    public override bool CheckCondition(EnemyContex contex)
    {
        float distanceToEndPoint = Vector2.Distance(contex.enemyPosition.position, contex.endWypointPoint.position);
        return distanceToEndPoint > distance;
    }
}
