#nullable enable

using UniRx;
using VContainer;

public class PlayerBodyStateHolder
{
    private readonly ReactiveProperty<PlayerBodyState> _currentPlayerState = new(PlayerBodyState.None);
    public IReadOnlyReactiveProperty<PlayerBodyState> CurrentPlayerState => _currentPlayerState;

    [Inject]
    public PlayerBodyStateHolder(PlayerInputController playerInputController)
    {
        playerInputController.StartStretchAction += () => AddPlayerBodyState(PlayerBodyState.Stretching);
        playerInputController.EndStretchAction += () => AddPlayerBodyState(PlayerBodyState.Contracted);
        playerInputController.RotateLeftAction += () => AddPlayerBodyState(PlayerBodyState.RotatingLeft);
        playerInputController.RotateRightAction += () => AddPlayerBodyState(PlayerBodyState.RotatingRight);
    }

    public void AddPlayerBodyState(PlayerBodyState playState)
    {
        var current = _currentPlayerState.Value;

        if (playState == PlayerBodyState.Stretching)
            current &= ~PlayerBodyState.Contracted;

        if (playState == PlayerBodyState.Contracted)
            current &= ~PlayerBodyState.Stretching;

        current |= playState;
        _currentPlayerState.Value = current;
    }

    public void RemovePlayerBodyState(PlayerBodyState playState)
    {
        _currentPlayerState.Value &= ~playState;
    }

    public bool IsContainState(PlayerBodyState playState)
    {
        return (_currentPlayerState.Value & playState) != 0;
    }
}


[System.Flags]
public enum PlayerBodyState
{
    None = 0,
    Stretching = 1 << 0, // 0001
    Contracted = 1 << 1, // 0010
    RotatingLeft = 1 << 2,
    RotatingRight = 1 << 3,
}
