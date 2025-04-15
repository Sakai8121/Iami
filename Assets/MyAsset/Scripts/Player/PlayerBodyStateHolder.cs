#nullable enable

using VContainer;

public class PlayerBodyStateHolder
{
    PlayerBodyState _currentPlayerState;

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
        if (playState == PlayerBodyState.Stretching)
            RemovePlayerBodyState(PlayerBodyState.Contracted);

        if (playState == PlayerBodyState.Contracted)
            RemovePlayerBodyState(PlayerBodyState.Stretching);

        _currentPlayerState |= playState;
    }

    public void RemovePlayerBodyState(PlayerBodyState playState)
    {
        _currentPlayerState &= ~playState;
    }

    public bool IsContainState(PlayerBodyState playState)
    {
        return (_currentPlayerState & playState) != 0;
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
