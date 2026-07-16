using UnityEngine;

[CreateAssetMenu(fileName = "PowerUpSO", menuName = "Power Up/PowerUpSO")]
public class PowerUpSO : ScriptableObject
{
    public string powerUpName;
    public string powerUpDescription;
    
    public CurrencyType currencyType; 
    public int powerUpCost;
    
    public BoostType powerUpBoostType;
    public int valuePowerUp; 
}
