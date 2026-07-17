using UnityEngine;

[CreateAssetMenu(fileName = "PlayerIsBerserk", menuName = "State Machine/Player/Condition/PlayerIsBerserk")]
public class PlayerIsBerserk : PlayerConditionSO
{
    protected override bool CheckCondition(PlayerContex contex)
    {
        if (contex.IsBerserk)
            return  true;
        
        return false;   
    }
}
