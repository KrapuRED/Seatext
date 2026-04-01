using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    public void TypingText(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        string key = context.control.displayName;
        TypeBoxManager.instance.CheckTyping(key);
        Debug.Log($"[PlayerInput - OnTypingText] Typed Text : {key}");
    }

    public void PuaseGame(InputAction.CallbackContext context)
    {
        if (context.performed)
            PanelManager.instance.OpenPanel("panel-00");
    }
}
