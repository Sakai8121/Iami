#nullable enable
using System;

public class GameStateObserver
{
    GameState _currentState = GameState.ShowStageTitle;

    public GameState CurrentState
    {
        get => _currentState;
        set
        {
            if (_currentState == value) return;

            _currentState = value;
            OnStateChanged?.Invoke(_currentState);
        }
    }

    public event Action<GameState>? OnStateChanged;

    public void Subscribe(GameState state, Action action)
    {
        OnStateChanged += (newState) =>
        {
            if (newState == state)
            {
                action?.Invoke();
            }
        };
    }
}
