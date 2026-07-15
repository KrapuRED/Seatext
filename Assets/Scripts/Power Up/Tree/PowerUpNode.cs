using UnityEngine;

public class PowerUpNode : MonoBehaviour
{
    [SerializeField] private BoostType boostType;
    
    [SerializeField] private string _powerUpNodeID;
    
    public BoostType BoostType => boostType;
    
    public void InitializePowerUpNode(string powerUpNodeID)
    {
        _powerUpNodeID = powerUpNodeID;
    }

    public void ConnectPowerUpNode(Transform nextPowerUpNode)
    {
        
    }
}
