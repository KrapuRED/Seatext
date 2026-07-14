using UnityEngine;

[CreateAssetMenu(fileName = "AttackChargeState", menuName = "State Machine/State/AttackChargeState")]
public class AttackChargeState : EnemyStateSO
{
        [SerializeField] private float chargeDelay;

    protected override void EnterState(EnemyContex contex)
    {
        Debug.Log($"{contex.enemyFish.name} is {name}");
        contex.fishMovement.RotateFish(contex.endWaypoint);
        contex.chargeTimer = chargeDelay;
    }

    protected override void ExcuteState(EnemyContex contex)
    {
        if (contex.chargeTimer > 0)
        {
            contex.chargeTimer -= Time.deltaTime;
            contex.fishSightVisual.OnSightVisual(contex.endWaypoint);
            Debug.Log("Charge in " + Mathf.Round(contex.chargeTimer));
        }

        if (contex.chargeTimer <= 0)
        {
            contex.fishSightVisual.Dettach();
            contex.chargeDirection = contex.endWaypoint;

            contex.fishMovement.MoveFish(contex.chargeDirection, 0f, contex.fishSpeed.GetFishSpeed(1));
        }
    }

    protected override void ExitState(EnemyContex contex)
    {
        Debug.Log($"{contex.enemyFish.name} is exiting {name}");
    }
}
