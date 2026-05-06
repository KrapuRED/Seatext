using System;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager instance {get; private set; }

    [Header("Currency Data")] 
    [SerializeField] private float seaCoinValue;
    
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
        }
    }

    public void UseCurrency()
    {
        
    }

    public void SetCurrency()
    {
        
    }
}
