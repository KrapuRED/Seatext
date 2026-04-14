using UnityEngine;

[CreateAssetMenu(fileName = "AttackChargeState", menuName = "State Machine/State/AttackChargeState")]
public class AttackChargeState : StateSO
{
    [SerializeField] private float chargeDelay;

    private float chargeTimer;
    private Transform chargeDirection;

    public override void EnterState(EnemyContex contex)
    {
        Debug.Log($"{contex.enemyFish.name} is {name}");
        contex.enemyFishMovement.RotateFish(contex.endWypointPoint);
        chargeTimer = chargeDelay;
    }

    public override void ExcuteState(EnemyContex contex)
    {
        if (chargeTimer > 0 )
        {
            chargeTimer -= Time.deltaTime;
            contex.fishSightVisual.OnSightVisual(contex.endWypointPoint);
            Debug.Log("Charge in " + Mathf.Round(chargeTimer));
        }

        if (chargeTimer <= 0)
        {
            Debug.Log("Charge...");
            contex.fishSightVisual.Dettach();
            chargeDirection = contex.endWypointPoint;
            contex.enemyFishMovement.MoveFish(chargeDirection, contex.enemyFishSpeed.GetFishSpeed(0));
        }
    }
}
