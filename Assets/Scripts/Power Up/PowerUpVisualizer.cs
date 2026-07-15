using System;
using UnityEngine;

public class PowerUpVisualizer : MonoBehaviour
{
    [SerializeField] private Transform powerUpContainer;

    private void Start()
    {
        PowerUpManager.instance.IntializePowerUps(powerUpContainer);
    }
}
