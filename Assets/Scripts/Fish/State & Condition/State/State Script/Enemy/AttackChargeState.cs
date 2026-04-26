using UnityEngine;

[CreateAssetMenu(fileName = "AttackChargeState", menuName = "State Machine/State/AttackChargeState")]
public class AttackChargeState : EnemyStateSO
{
    [SerializeField] private float chargeDelay;

    private float chargeTimer;
    private Transform chargeDirection;

    protected override void EnterState(EnemyContex contex)
    {
        Debug.Log($"{contex.enemyFish.name} is {name}");
        contex.fishMovement.RotateFish(contex.endWaypoint);
        chargeTimer = chargeDelay;
    }

    protected override void ExcuteState(EnemyContex contex)
    {
        if (chargeTimer > 0 )
        {
            chargeTimer -= Time.deltaTime;
            contex.fishSightVisual.OnSightVisual(contex.endWaypoint);
            Debug.Log("Charge in " + Mathf.Round(chargeTimer));
        }

        if (chargeTimer <= 0)
        {
            contex.fishSightVisual.Dettach();
            chargeDirection = contex.endWaypoint;
            contex.fishMovement.MoveFish(chargeDirection, contex.fishSpeed.GetFishSpeed(1));
        }
    }
}
