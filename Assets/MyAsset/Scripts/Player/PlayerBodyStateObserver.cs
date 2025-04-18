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
        BodyColliderStateHolder bodyColliderStateHolder,BodyTransformHolderMono bodyTransformHolderMono)
    {
        _stateHolder = stateHolder;
        _bodyTransformHolderMono = bodyTransformHolderMono;
        _bodyColliderStateHolder = bodyColliderStateHolder;

        _stateActions = new Dictionary<PlayerBodyState, Action>
        {
            { PlayerBodyState.Stretching, () => {
                // Stretching ÇÃèàóù
                bodyPhysicsDataCalculator.StartStretch();
            }},
            { PlayerBodyState.Contracted, () => {
                // Stretching ÇÃèàóù
                bodyPhysicsDataCalculator.StartContract();
            }},
            { PlayerBodyState.RotatingLeft, () => {
                // Rotating Left ÇÃèàóù
                bodyPhysicsDataCalculator.StartRotateLeft();
            }},
            { PlayerBodyState.RotatingRight, () => {
                // Rotating Right ÇÃèàóù
                bodyPhysicsDataCalculator.StartRotateRight();
            }},
            { PlayerBodyState.CancelRotate, () => {
                // Rotating Right ÇÃèàóù
                bodyPhysicsDataCalculator.CancelRotate();
            }},
            // ïKóvÇ…âûÇ∂Çƒí«â¡
        };

        bodyPhysicsDataCalculator.EndContractAction +=
            () => stateHolder.RemovePlayerBodyState(PlayerBodyState.Contracted);
        bodyPhysicsDataCalculator.EndRotateRightAction += EndRotateRightAction;
        bodyPhysicsDataCalculator.EndRotateLeftAction += EndRotateLeftAction;
        bodyPhysicsDataCalculator.EndCancelRotateAction +=
            () => stateHolder.RemovePlayerBodyState(PlayerBodyState.CancelRotate);
    }

    public void Initialize()
    {
        _stateHolder.CurrentPlayerBodyState
                .Skip(1) // èâä˙ílÇñ≥éãÇ∑ÇÈèÍçá
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
