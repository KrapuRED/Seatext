using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "SetPositionPlayerEventSO", menuName = "Events/SetPositionPlayerEventSO")]
public class SetPositionPlayerEventSO : ScriptableObject
{
    public UnityAction<Transform> OnSetPosition;

    public void OnRaise(Transform position)
    {
        OnSetPosition?.Invoke(position);
    }

    public void Register(UnityAction<Transform> listener)
    {
        OnSetPosition += listener;
    }

    public void Unregister(UnityAction<Transform> listener)
    {
        OnSetPosition -= listener;
    }
}
