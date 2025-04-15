#nullable enable

using UniRx;
using VContainer;

public class PlayerBodyStateHolder
{
    readonly ReactiveProperty<PlayerBodyState> _currentPlayerState = new(PlayerBodyState.None);
    public IReadOnlyReactiveProperty<PlayerBodyState> CurrentPlayerState => _currentPlayerState;

    [Inject]
    public PlayerBodyStateHolder(PlayerInputController playerInputController)
    {
        playerInputController.StartStretchAction += () => AddPlayerBodyState(PlayerBodyState.Stretching);
        playerInputController.EndStretchAction += () => AddPlayerBodyState(PlayerBodyState.Contracted);
        playerInputController.RotateLeftAction += () => AddPlayerBodyState(PlayerBodyState.RotatingLeft);
        playerInputController.RotateRightAction += () => AddPlayerBodyState(PlayerBodyState.RotatingRight);
    }

    void AddPlayerBodyState(PlayerBodyState playState)
    {
        var current = _currentPlayerState.Value;

        //ストレッチモードになるならコントラクトモードはやめる
        if (playState == PlayerBodyState.Stretching)
            current &= ~PlayerBodyState.Contracted;

        //コントラクトモードになるならストレッチモードはやめる
        if (playState == PlayerBodyState.Contracted)
            current &= ~PlayerBodyState.Stretching;
        
        //左回転しようとしたとき回転キャンセル中なら回転の入力を受け付けない
        if(playState == PlayerBodyState.RotatingLeft && IsContainState(PlayerBodyState.CancelRotate))
            return;
        //右回転しようとしたとき回転キャンセル中なら回転の入力を受け付けない
        if(playState == PlayerBodyState.RotatingRight && IsContainState(PlayerBodyState.CancelRotate))
            return;
        //左回転しようとしたとき右回転中ならキャンセル回転する(回転前の位置にもどる処理)
        if (playState == PlayerBodyState.RotatingLeft && IsContainState(PlayerBodyState.RotatingRight))
        {
            current &= ~PlayerBodyState.RotatingRight;
            playState = PlayerBodyState.CancelRotate;
        }
        //右回転しようとしたとき左回転中ならキャンセル回転する(回転前の位置にもどる処理)
        if (playState == PlayerBodyState.RotatingRight && IsContainState(PlayerBodyState.RotatingLeft))
        {
            current &= ~PlayerBodyState.RotatingLeft;
            playState = PlayerBodyState.CancelRotate;
        }

        current |= playState;
        _currentPlayerState.Value = current;
    }

    public void RemovePlayerBodyState(PlayerBodyState playState)
    {
        _currentPlayerState.Value &= ~playState;
    }

    bool IsContainState(PlayerBodyState playState)
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
    CancelRotate = 1 << 4,
}
