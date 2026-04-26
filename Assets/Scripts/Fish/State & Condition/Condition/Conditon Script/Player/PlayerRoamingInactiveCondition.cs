using UnityEngine;

[CreateAssetMenu(fileName = "PlayerRoamingInactiveCondition", menuName = "State Machine/Player/Condition/RoamingInactiveCondition")]

public class PlayerRoamingInactiveCondition : PlayerConditionSO
{
    protected override bool CheckCondition(PlayerContex contex)
    {
        return false;
    }
}
