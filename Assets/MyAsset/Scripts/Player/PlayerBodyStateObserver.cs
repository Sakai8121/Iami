using UniRx;
using VContainer;
using VContainer.Unity;
using UnityEngine;
using System;
using System.Collections.Generic;

public class PlayerBodyStateObserver : IInitializable, IDisposable
{
    readonly PlayerBodyStateHolder _stateHolder;
    readonly CompositeDisposable _disposables = new();
    
    readonly Dictionary<PlayerBodyState, Action> _stateActions;
    
    [Inject]
    public PlayerBodyStateObserver(PlayerBodyStateHolder stateHolder,BodyPhysicsDataCalculator bodyPhysicsDataCalculator)
    {
        _stateHolder = stateHolder;

        _stateActions = new Dictionary<PlayerBodyState, Action>
        {
            { PlayerBodyState.Stretching, () => {
                Debug.Log("Player is Stretching");
                // Stretching ÇÃèàóù
                bodyPhysicsDataCalculator.StartStretch();
            }},
            { PlayerBodyState.Contracted, () => {
                Debug.Log("Player is Contracted");
                // Stretching ÇÃèàóù
                bodyPhysicsDataCalculator.StartContract();
            }},
            { PlayerBodyState.RotatingLeft, () => {
                Debug.Log("Player is Rotating Left");
                // Rotating Left ÇÃèàóù
                bodyPhysicsDataCalculator.StartRotateLeft();
            }},
            { PlayerBodyState.RotatingRight, () => {
                Debug.Log("Player is Rotating Right");
                // Rotating Right ÇÃèàóù
                bodyPhysicsDataCalculator.StartRotateRight();
            }},
            { PlayerBodyState.CancelRotate, () => {
                Debug.Log("Player is Rotating Right");
                // Rotating Right ÇÃèàóù
                bodyPhysicsDataCalculator.CancelRotate();
            }},
            // ïKóvÇ…âûÇ∂Çƒí«â¡
        };

        bodyPhysicsDataCalculator.EndContractAction +=
            () => stateHolder.RemovePlayerBodyState(PlayerBodyState.Contracted);
        bodyPhysicsDataCalculator.EndRotateRightAction +=
            () => stateHolder.RemovePlayerBodyState(PlayerBodyState.RotatingRight);
        bodyPhysicsDataCalculator.EndRotateLeftAction +=
            () => stateHolder.RemovePlayerBodyState(PlayerBodyState.RotatingLeft);
        bodyPhysicsDataCalculator.EndCancelRotateAction +=
            () => stateHolder.RemovePlayerBodyState(PlayerBodyState.CancelRotate);
    }

    public void Initialize()
    {
        _stateHolder.CurrentPlayerState
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
                Debug.Log(kvp.Key);
                kvp.Value.Invoke();
            }
        }
    }

    public void Dispose()
    {
        _disposables.Dispose();
    }
}
