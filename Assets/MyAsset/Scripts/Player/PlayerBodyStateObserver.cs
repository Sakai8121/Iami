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
                // Stretching ‚Ìˆ—
                bodyPhysicsDataCalculator.StartStretch();
            }},
            { PlayerBodyState.Contracted, () => {
                Debug.Log("Player is Contracted");
                // Stretching ‚Ìˆ—
                bodyPhysicsDataCalculator.StartContract();
            }},
            { PlayerBodyState.RotatingLeft, () => {
                Debug.Log("Player is Rotating Left");
                // Rotating Left ‚Ìˆ—
                bodyPhysicsDataCalculator.StartRotateLeft();
            }},
            { PlayerBodyState.RotatingRight, () => {
                Debug.Log("Player is Rotating Right");
                // Rotating Right ‚Ìˆ—
                bodyPhysicsDataCalculator.StartRotateRight();
            }},
            // •K—v‚É‰‚¶‚Ä’Ç‰Á
        };
    }

    public void Initialize()
    {
        _stateHolder.CurrentPlayerState
                .Skip(1) // ‰Šú’l‚ğ–³‹‚·‚éê‡
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
