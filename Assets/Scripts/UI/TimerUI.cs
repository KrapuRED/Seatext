using UnityEngine;
using TMPro;

public class TimerUI : MonoBehaviour
{
    [Header("Timer UI Config")] 
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI timerTextValue;

    private void OnEnable()
    {
        GameEvents.OnUpdateTimerGamePlay.AddListener(OnUpdateTimerGamePlay);
    }

    void OnUpdateTimerGamePlay(float timer)
    {
        timerTextValue.text = timer.ToString("F0");
    }
}
