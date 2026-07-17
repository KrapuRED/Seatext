using UnityEngine;

[CreateAssetMenu(fileName = "PlayerNearRoamingPointCondition", menuName = "State Machine/Player/Condition/PlayerNearRoamingPoint")]

public class PlayerNearRoamingPointCondition : PlayerConditionSO
{
    [SerializeField] private float distanceToRoamingPoint = 1f;

    protected override bool CheckCondition(PlayerContex contex)
    {
        if (contex.RoamingPoint == null)
            return false;

        float distance = Vector2.Distance(contex.playerFish.transform.position, contex.RoamingPoint.position);

        Debug.Log($"[PlayerNearRoamingPointCondition - CheckCondition] Distance to Roaming Point: {distance}");

        if (distance <= distanceToRoamingPoint && !contex.IsBerserk)
            return true;

        return false;
    }
}
