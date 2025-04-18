#nullable enable

using UniRx;
using VContainer;

public class PlayerBodyStateHolder
{
    readonly ReactiveProperty<PlayerBodyState> _currentPlayerBodyState = new(PlayerBodyState.None);
    public IReadOnlyReactiveProperty<PlayerBodyState> CurrentPlayerBodyState => _currentPlayerBodyState;

    [Inject]
    public PlayerBodyStateHolder(PlayerInputController playerInputController,BodyColliderStateHolder bodyColliderStateHolder)
    {
        playerInputController.StartStretchAction += () => AddPlayerBodyState(PlayerBodyState.Stretching);
        playerInputController.EndStretchAction += () => AddPlayerBodyState(PlayerBodyState.Contracted);
        playerInputController.RotateLeftAction += () =>
        {
            bodyColliderStateHolder.ChangeBodyPivot(-1);
            AddPlayerBodyState(PlayerBodyState.RotatingLeft);
        };
        playerInputController.RotateRightAction += () =>
        {
            bodyColliderStateHolder.ChangeBodyPivot(1);
            AddPlayerBodyState(PlayerBodyState.RotatingRight);
        };
    }

    void AddPlayerBodyState(PlayerBodyState playState)
    {
        var before = _currentPlayerBodyState.Value;
        var current = before;

        // ストレッチモードになるならコントラクトモードはやめる
        if (playState == PlayerBodyState.Stretching)
            current &= ~PlayerBodyState.Contracted;

        // コントラクトモードになるならストレッチモードはやめる
        if (playState == PlayerBodyState.Contracted)
            current &= ~PlayerBodyState.Stretching;

        // 回転キャンセル中は回転を受け付けない
        if (playState == PlayerBodyState.RotatingLeft && IsContainState(PlayerBodyState.CancelRotate))
            return;
        if (playState == PlayerBodyState.RotatingRight && IsContainState(PlayerBodyState.CancelRotate))
            return;

        // 左→右 or 右→左 の回転中ならキャンセル処理
        if (playState == PlayerBodyState.RotatingLeft && IsContainState(PlayerBodyState.RotatingRight))
        {
            current &= ~PlayerBodyState.RotatingRight;
            playState = PlayerBodyState.CancelRotate;
        }
        if (playState == PlayerBodyState.RotatingRight && IsContainState(PlayerBodyState.RotatingLeft))
        {
            current &= ~PlayerBodyState.RotatingLeft;
            playState = PlayerBodyState.CancelRotate;
        }

        current |= playState;

        // 差分をログに出力
        var added = current & ~before;
        var removed = before & ~current;

        // if (added != PlayerBodyState.None)
        // {
        //     UnityEngine.Debug.Log($"[PlayerState] Added: {added}");
        // }
        // if (removed != PlayerBodyState.None)
        // {
        //     UnityEngine.Debug.Log($"[PlayerState] Removed: {removed}");
        // }

        _currentPlayerBodyState.Value = current;
    }


    public void RemovePlayerBodyState(PlayerBodyState playState)
    {
        _currentPlayerBodyState.Value &= ~playState;
    }

    bool IsContainState(PlayerBodyState playState)
    {
        return (_currentPlayerBodyState.Value & playState) != 0;
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
