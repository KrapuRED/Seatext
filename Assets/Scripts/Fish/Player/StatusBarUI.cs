using UnityEngine;

public abstract class StatusBarUI : MonoBehaviour
{
    [SerializeField] private string statusName;
    [SerializeField] private string statusID;

    public abstract void UpdateStatusBar(float currentValue, float maxValue);
}
