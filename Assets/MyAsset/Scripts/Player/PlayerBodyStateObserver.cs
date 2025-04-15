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

    [Inject]
    readonly Dictionary<PlayerBodyState, Action> _stateActions;

    public PlayerBodyStateObserver(PlayerBodyStateHolder stateHolder)
    {
        _stateHolder = stateHolder;

        _stateActions = new Dictionary<PlayerBodyState, Action>
        {
            { PlayerBodyState.Stretching, () => {
                Debug.Log("Player is Stretching");
                // Stretching �̏���
            }},
            { PlayerBodyState.RotatingLeft, () => {
                Debug.Log("Player is Rotating Left");
                // Rotating Left �̏���
            }},
            { PlayerBodyState.RotatingRight, () => {
                Debug.Log("Player is Rotating Right");
                // Rotating Right �̏���
            }},
            // �K�v�ɉ����Ēǉ�
        };
    }

    public void Initialize()
    {
        _stateHolder.CurrentPlayerState
                .Skip(1) // �����l�𖳎�����ꍇ
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

    public void Dispose()
    {
        _disposables.Dispose();
    }
}
