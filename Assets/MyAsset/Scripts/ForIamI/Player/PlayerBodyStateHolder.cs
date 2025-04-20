#nullable enable

using UniRx;
using UnityEngine;
using VContainer;

public class PlayerBodyStateHolder
{
    readonly ReactiveProperty<PlayerBodyState> _currentPlayerBodyState = new(PlayerBodyState.None);
    public IReadOnlyReactiveProperty<PlayerBodyState> CurrentPlayerBodyState => _currentPlayerBodyState;

    BodyColliderStateHolder _bodyColliderStateHolder;
    BodyTransformHolderMono _bodyTransformHolderMono;
    
    [Inject]
    public PlayerBodyStateHolder(PlayerInputController playerInputController,BodyColliderStateHolder bodyColliderStateHolder,
        BodyTransformHolderMono bodyTransformHolderMono)
    {
        _bodyColliderStateHolder = bodyColliderStateHolder;
        _bodyTransformHolderMono = bodyTransformHolderMono;
        
        playerInputController.StartStretchAction += () => AddPlayerBodyState(PlayerBodyState.Stretching);
        playerInputController.EndStretchAction += () => AddPlayerBodyState(PlayerBodyState.Contracted);
        playerInputController.RotateLeftAction += () => AddPlayerBodyState(PlayerBodyState.RotatingLeft);
        playerInputController.RotateRightAction += () => AddPlayerBodyState(PlayerBodyState.RotatingRight);
    }

    public void CancelStretch(BodyColliderDirection bodyColliderDirection)
    {
        var transformUp = _bodyTransformHolderMono.TransformUp();
        Debug.LogError($"{transformUp} {bodyColliderDirection}");
        if (transformUp == Vector3.up)
        {
            if(bodyColliderDirection == BodyColliderDirection.Up)
                RemovePlayerBodyState(PlayerBodyState.Stretching);
        }
        else if (transformUp == Vector3.down)
        {
            if(bodyColliderDirection == BodyColliderDirection.Under)
                RemovePlayerBodyState(PlayerBodyState.Stretching);
        }
        else if (transformUp == Vector3.right)
        {
            if(bodyColliderDirection == BodyColliderDirection.Right)
                RemovePlayerBodyState(PlayerBodyState.Stretching);
        }
        else if (transformUp == Vector3.left)
        {
            if(bodyColliderDirection == BodyColliderDirection.Left)
                RemovePlayerBodyState(PlayerBodyState.Stretching);
        }
    }

    public void AddPlayerBodyState(PlayerBodyState playState)
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
        
        // キャンセル処理なら左右回転を元に戻す
        if (playState == PlayerBodyState.CancelRotate)
        {
            current &= ~PlayerBodyState.RotatingRight;
            current &= ~PlayerBodyState.RotatingLeft;
        }
        
        //Rotateする前にピボットを切り替える
        if(playState == PlayerBodyState.RotatingLeft && !IsContainState(PlayerBodyState.RotatingLeft))
            _bodyColliderStateHolder.ChangeBodyPivot(-1);
        if(playState == PlayerBodyState.RotatingRight && !IsContainState(PlayerBodyState.RotatingRight))
            _bodyColliderStateHolder.ChangeBodyPivot(1);

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
