using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI timerText;

    [Header("Configuração")]
    public int startMinutes = 5;
    public int startSeconds = 0;

    [Header("Eventos")]
    public UnityEvent onTimerEnd;

    private int minutes;
    private int seconds;
    private float elapsedTime;
    private bool isRunning = true;

    void Start()
    {
        minutes = startMinutes;
        seconds = startSeconds;
        UpdateTimerUI();
    }

    void Update()
    {
        if (!isRunning) return;

        elapsedTime += Time.deltaTime;

        if (elapsedTime >= 1f)
        {
            elapsedTime = 0f;
            Tick();
        }
    }

    void Tick()
    {
        if (seconds > 0)
        {
            seconds--;
        }
        else
        {
            if (minutes > 0)
            {
                minutes--;
                seconds = 59;
            }
            else
            {
                isRunning = false;
                onTimerEnd?.Invoke();
                return;
            }
        }

        UpdateTimerUI();
    }

    void UpdateTimerUI()
    {
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
