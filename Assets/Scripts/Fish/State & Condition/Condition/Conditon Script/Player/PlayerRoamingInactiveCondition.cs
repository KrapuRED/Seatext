using UnityEngine;

[CreateAssetMenu(fileName = "PlayerRoamingInactiveCondition", menuName = "State Machine/Player/Condition/RoamingInactiveCondition")]

public class PlayerRoamingInactiveCondition : PlayerConditionSO
{
    public float IdleTimeThreshold = 5f;

    private float timer;

    protected override bool CheckCondition(PlayerContex contex)
    {
        if (!contex.IsIdle)
        {
            timer = 0f; // Reset timer if player is active
            return false;
        }

        timer += Time.deltaTime;

        if (timer >= IdleTimeThreshold)
        {
            //Debug.Log("[PlayerRoamingInactiveCondition - CheckCondition] Player has been idle for too long, transitioning to Roaming Inactive State");
            return true;
        }

        return false;
    }
}
