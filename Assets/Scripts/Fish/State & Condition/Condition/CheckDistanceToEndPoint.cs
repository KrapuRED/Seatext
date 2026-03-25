using UnityEngine;

public class CheckDistanceToEndPoint : Condition
{
    public float distanceThreshold; // Distance threshold to consider as "close enough"
    public Transform endPoint; // Reference to the end point transform

    public override bool CheckCondition()
    {
        float distanceToEndPoint = Vector3.Distance(transform.position, endPoint.position);
        return distanceToEndPoint <= distanceThreshold;
    }
}
