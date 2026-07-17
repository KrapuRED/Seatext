using UnityEngine;

[CreateAssetMenu(fileName = "PlayerBerserkState", menuName = "State Machine/Player/State/PlayerBerserkState")]
public class PlayerBerserkState : PlayerStateSO
{
    protected override void EnterState(PlayerContex contex)
    {

    }

    protected override void ExcuteState(PlayerContex contex)
    {
        Transform foodPosition = contex.fishEyeSight.GetNearestObjectWithTag("Food");

            contex.fishMouth.SetMouthState(true);
        
        
        if (foodPosition == null)
        {
            return;
        }

        float closestDistance = Vector2.Distance(foodPosition.position, contex.playerFish.transform.position);
        
        contex.playerFish.SetTargetPosition(foodPosition);
        contex.fishMouth.SetMouthState(closestDistance < 1f);
        contex.fishMovement.MoveFish(foodPosition, closestDistance, contex.fishSpeed.GetChaseFishSpeed());
    }

    protected override void ExitState(PlayerContex contex)
    {
        
    }
}
