using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    public string playerInputString;
    [SerializeField] private float timerToIdle;
    [SerializeField] private float timer;

    [SerializeField] private bool isIdle;

    private void OnEnable()
    {
        Keyboard.current.onTextInput += TypingText;
    }

    private void OnDisable()
    {
        Keyboard.current.onTextInput -= TypingText;
    }

    private void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;
        if (timer >= timerToIdle && !isIdle)
        {
            isIdle = true;
            GameEvents.OnPlayerActive.Invoke(false);
        }
    }

    public void TypingText(char character)
    {
        timer = 0f;
        isIdle = false;
        GameEvents.OnPlayerActive.Invoke(true);

        string key = character.ToString();
        playerInputString = key;
        
        TypeBoxManager.instance.CheckTyping(key);

    }

    public void PuaseGame(InputAction.CallbackContext context)
    {
        if (context.performed)
            PanelManager.instance.OpenPanel("panel-00");
    }
}
