using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "SetTypeBoxEventSO", menuName = "Events/SetTypeBoxEventSO")]
public class SetTypeBoxEventSO : ScriptableObject
{
    public UnityAction<TypeBox> OnSetTypeBox;

    public void Raise(TypeBox typeBox)
    {
        OnSetTypeBox?.Invoke(typeBox);
    }

    public void Register(UnityAction<TypeBox> listener)
    {
        OnSetTypeBox += listener;
    }

    public void Unregister(UnityAction<TypeBox> listener)
    {
        OnSetTypeBox -= listener;
    }
}
