using UniRx;
using VContainer;
using VContainer.Unity;
using UnityEngine;
using System;
using System.Collections.Generic;

public class PlayerBodyStateObserver : IInitializable, IDisposable
{
    readonly PlayerBodyStateHolder _stateHolder;
    readonly BodyTransformHolderMono _bodyTransformHolderMono;
    readonly BodyColliderStateHolder _bodyColliderStateHolder;
    
    readonly CompositeDisposable _disposables = new();
    
    readonly Dictionary<PlayerBodyState, Action> _stateActions;
    
    [Inject]
    public PlayerBodyStateObserver(PlayerBodyStateHolder stateHolder,BodyPhysicsDataCalculator bodyPhysicsDataCalculator,
        BodyColliderStateHolder bodyColliderStateHolder,BodyTransformHolderMono bodyTransformHolderMono,
        BodyColliderViewMono bodyColliderViewMono)
    {
        _stateHolder = stateHolder;
        _bodyTransformHolderMono = bodyTransformHolderMono;
        _bodyColliderStateHolder = bodyColliderStateHolder;
        
        _stateActions = new Dictionary<PlayerBodyState, Action>
        {
            { PlayerBodyState.Stretching, () => {
                // Stretching の処理
                bodyPhysicsDataCalculator.StartStretch();
            }},
            { PlayerBodyState.Contracted, () => {
                // Stretching の処理
                bodyPhysicsDataCalculator.StartContract();
            }},
            { PlayerBodyState.RotatingLeft, () => {
                // Rotating Left の処理
                bodyPhysicsDataCalculator.StartRotateLeft();
            }},
            { PlayerBodyState.RotatingRight, () => {
                // Rotating Right の処理
                bodyPhysicsDataCalculator.StartRotateRight();
            }},
            { PlayerBodyState.CancelRotate, () => {
                // Rotating Right の処理
                bodyPhysicsDataCalculator.CancelRotate();
            }},
            // 必要に応じて追加
        };

        bodyPhysicsDataCalculator.EndContractAction +=
            () => stateHolder.RemovePlayerBodyState(PlayerBodyState.Contracted);
        bodyPhysicsDataCalculator.EndRotateRightAction += EndRotateRightAction;
        bodyPhysicsDataCalculator.EndRotateLeftAction += EndRotateLeftAction;
        bodyPhysicsDataCalculator.EndCancelRotateAction +=
            () => stateHolder.RemovePlayerBodyState(PlayerBodyState.CancelRotate);

        bodyColliderViewMono.CallCancelRotateAction += () => stateHolder.AddPlayerBodyState(PlayerBodyState.CancelRotate);
        bodyColliderViewMono.CallCancelStretchAction += stateHolder.CancelStretch;
    }

    public void Initialize()
    {
        _stateHolder.CurrentPlayerBodyState
                .Skip(1) // 初期値を無視する場合
                .Subscribe(OnStateChanged)
                .AddTo(_disposables);
    }

    void OnStateChanged(PlayerBodyState newState)
    {
        foreach (var kvp in _stateActions)
        {
            if ((newState & kvp.Key) != 0)
            {
                kvp.Value.Invoke();
            }
        }
    }

    void EndRotateRightAction()
    {
        _stateHolder.RemovePlayerBodyState(PlayerBodyState.RotatingRight);
        _bodyColliderStateHolder.ChangeBodyColliderDirection(_bodyTransformHolderMono.RotateZ());
    }
    
    void EndRotateLeftAction()
    {
        _stateHolder.RemovePlayerBodyState(PlayerBodyState.RotatingLeft);
        _bodyColliderStateHolder.ChangeBodyColliderDirection(_bodyTransformHolderMono.RotateZ());
    }

    public void Dispose()
    {
        _disposables.Dispose();
    }
}
