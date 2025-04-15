# nullable enable

using VContainer;

public class PlayerHandStateHolder
{
    PlayerHandState _currentPlayerHandState;

    public void AddPlayerBodyState(PlayerHandState playState)
    {
        if (playState == PlayerHandState.Catching)
            RemovePlayerBodyState(PlayerHandState.None);

        if (playState == PlayerHandState.None)
            RemovePlayerBodyState(PlayerHandState.Catching);

        _currentPlayerHandState |= playState;
    }

    public void RemovePlayerBodyState(PlayerHandState playState)
    {
        _currentPlayerHandState &= ~playState;
    }

    public bool IsContainState(PlayerHandState playState)
    {
        return (_currentPlayerHandState & playState) != 0;
    }
}

[System.Flags]
public enum PlayerHandState
{
    None = 0,
    Catching = 1 << 0, // 0001
}
