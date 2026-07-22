using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PowerUpVisualizer : MonoBehaviour
{
    [SerializeField] private Transform powerUpContainer;
    [SerializeField] private Transform firstPowerUp;

    private void Start()
    {
        PowerUpManager.instance.IntializePowerUps(powerUpContainer);
        
    }

    public void ChangeSelecedPowerUp(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return; 
        
        Vector2 position = context.ReadValue<Vector2>();
        
        DirectionNode directionH = position.x < 0 ? DirectionNode.Left : DirectionNode.Right;
        DirectionNode directionV = position.y < 0 ?  DirectionNode.Down : DirectionNode.Up;
        
        if (position.x != 0)
            PowerUpManager.instance.MovePowerUpNode(directionH);
            
        if (position.y != 0)
            PowerUpManager.instance.MovePowerUpNode(directionV);
    }
}
