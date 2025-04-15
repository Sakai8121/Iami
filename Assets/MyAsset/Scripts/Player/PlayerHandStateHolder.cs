# nullable enable

using UniRx;
using VContainer;

public class PlayerHandStateHolder
{
    private readonly ReactiveProperty<PlayerHandState> _currentPlayerState = new(PlayerHandState.None);
    public IReadOnlyReactiveProperty<PlayerHandState> CurrentPlayerState => _currentPlayerState;

    public void AddPlayerBodyState(PlayerHandState playState)
    {
        var current = _currentPlayerState.Value;

        if (playState == PlayerHandState.Catching)
            current &= ~PlayerHandState.None;

        if (playState == PlayerHandState.None)
            current &= ~PlayerHandState.Catching;

        current |= playState;
        _currentPlayerState.Value = current;
    }

    public void RemovePlayerBodyState(PlayerHandState playState)
    {
        _currentPlayerState.Value &= ~playState;
    }

    public bool IsContainState(PlayerHandState playState)
    {
        return (_currentPlayerState.Value & playState) != 0;
    }
}

[System.Flags]
public enum PlayerHandState
{
    None = 0,
    Catching = 1 << 0, // 0001
}
