#nullable enable

using System;
using TMPro;
using UnityEngine;
using VContainer;

public class TimerTextViewMono: MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText = null!;

    TimeHolder _timeHolder;
    
    [Inject]
    public void Construct(TimeHolder timeHolder)
    {
        _timeHolder = timeHolder;
    }

    void Update()
    {
        ChangeTimerText(_timeHolder.CurrentTime);
    }

    public void ChangeTimerText(float time)
    {
        timerText.text = FormatTime(time);
    }
    
    string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        int hundredths = Mathf.FloorToInt((time * 100f) % 100f); // 小数第2位

        return $"{minutes:00}:{seconds:00}.{hundredths:00}";
    }
}