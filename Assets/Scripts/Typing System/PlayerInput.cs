using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private float timerToIdle;
    [SerializeField] private float timer;

    private void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;
        if (timer >= timerToIdle)
        {
            GameEvents.OnPlayerActive.Invoke(false);
        }
    }

    public void TypingText(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        timer = 0f;
        GameEvents.OnPlayerActive.Invoke(true);

        string key = context.control.displayName;
        TypeBoxManager.instance.CheckTyping(key);

    }

    public void PuaseGame(InputAction.CallbackContext context)
    {
        if (context.performed)
            PanelManager.instance.OpenPanel("panel-00");
    }
}
