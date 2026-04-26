using UnityEngine;

[CreateAssetMenu(fileName = "PlayerIsIdleCondition", menuName = "State Machine/Player/Condition/IsIdleCondition")]
public class PlayerIsIdleCondition :  PlayerConditionSO
{
    [SerializeField] private float idleTime;

    private float idleTimer;

    protected override bool CheckCondition(PlayerContex contex)
    {
        //if player not typing in sometime will be in idle state and cannot eat food
        Debug.Log($"[PlayerIsIdleCondition - CheckCondition] Check Player Is Idle Condition");

        if (contex.IsActiveFish)
        {
            idleTimer = 0f;
            return false;
        }
        else
        {
            idleTimer += Time.deltaTime;
            if (idleTimer >= idleTime)
            {
                return true;
            }
        }

        return false;
    }
}
