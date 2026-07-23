using System;
using UnityEngine;

public class ComboManager : MonoBehaviour
{
    public static ComboManager instance {get; private set;}
    
    [Header("Typing Combo Config")]
    [SerializeField] private string comboType;
    [SerializeField] private float maxTimerTyping;
    [SerializeField] private float maxMultiplierTyping;
    [SerializeField] private float multiplierTyping;
    [SerializeField] private float currentValueMultiplierTyping;
    [SerializeField] private MultiplierUI multiplierTypingUI;
    private int _totalTypingCombo;
    
    [Header("Eating Combo Config")]
    [SerializeField] private string combEating;
    [SerializeField] private float maxTimerEating;
    [SerializeField] private float maxMultiplierEating;
    [SerializeField] private float multiplierEating;
    [SerializeField] private float currentValueMultiplierEating;
    [SerializeField] private MultiplierUI multiplierEatingUI;

    private int _totalEatingCombo;
    
    
    public float ValueMultiplierTyping => currentValueMultiplierTyping;
    public float ValueMultiplierEating => currentValueMultiplierEating;
    
    private ManagerTimer _managerTimer;

    private void Awake()
    {
        _managerTimer = ManagerTimer.instance;

        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        CheckCombo();
    }

    private void CheckCombo()
    {
        if (_managerTimer == null)
            _managerTimer = ManagerTimer.instance;

        if (_managerTimer.CheckCounterTime(comboType))
        {
            currentValueMultiplierTyping = 0;
            _totalTypingCombo = 0;
            multiplierTypingUI.ResetMultiplierUI();
        }
        
        if (_managerTimer.CheckCounterTime(combEating))
        {
            currentValueMultiplierEating = 0;
            _totalEatingCombo = 0;
        }
    }
    
    public void StartComboTyping()
    {
        if (_managerTimer == null)
            _managerTimer = ManagerTimer.instance;

        _totalTypingCombo++;
        
        if (currentValueMultiplierTyping >= maxMultiplierTyping)
            currentValueMultiplierTyping = maxMultiplierTyping;
        else
        {
            currentValueMultiplierTyping += multiplierTyping;
        }
        
        _managerTimer.AssignCounterTime(comboType, maxTimerTyping);
        multiplierTypingUI.UpdatemultiplierUI(_totalTypingCombo, currentValueMultiplierTyping);
    }
    
    public void StartComboEating()
    {
        if (_managerTimer == null)
            _managerTimer = ManagerTimer.instance;
        
        if (currentValueMultiplierEating >= maxMultiplierEating)
            currentValueMultiplierEating = maxMultiplierEating;
        else
        {
            currentValueMultiplierEating += multiplierEating;
        }
        
        _managerTimer.AssignCounterTime(combEating, maxMultiplierEating);
    }

    public void ShowMultiplierEatingUI(Transform locationShow)
    {
        Debug.Log($"{locationShow.name} - {locationShow.position}");
        multiplierEatingUI.ShowMultiplierEatingUI(locationShow, currentValueMultiplierEating);
    }
}
