using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "SetTypeBoxEventSO", menuName = "Events/SetTypeBoxEventSO")]
public class SetTypeBoxEventSO : ScriptableObject
{
    public UnityAction<TypingBox> OnSetTypeBox;

    public void Raise(TypingBox typeBox)
    {
        OnSetTypeBox?.Invoke(typeBox);
    }

    public void Register(UnityAction<TypingBox> listener)
    {
        OnSetTypeBox += listener;
    }

    public void Unregister(UnityAction<TypingBox> listener)
    {
        OnSetTypeBox -= listener;
    }
}
