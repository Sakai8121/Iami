#nullable enable

using UnityEngine;
using VContainer.Unity;

public class TimeHolder: ITickable
{
    public float CurrentTime => _timeElapsed;
    
    float _timeElapsed = 0f;
    bool _isRunning = false;

    public void StartTimer()
    {
        _isRunning = true;
    }

    public void StopTimer()
    {
        _isRunning = false;
    }

    public void Tick()
    {
        CountTime();
    }
    
    void CountTime()
    {
        if (!_isRunning) return;

        _timeElapsed += Time.deltaTime;
    }
}