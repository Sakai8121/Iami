using System;
using UniRx;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class PlayerHandStateObserver : IInitializable, IDisposable
{
    readonly PlayerHandStateHolder _stateHolder;
    readonly CompositeDisposable _disposables = new();

    [Inject]
    public PlayerHandStateObserver(PlayerHandStateHolder stateHolder)
    {
        _stateHolder = stateHolder;

        _stateHolder.CurrentPlayerState
            .Skip(1) // 初期値を無視する場合
            .Subscribe(OnStateChanged)
            .AddTo(_disposables);
    }

    public void Initialize()
    {
        _stateHolder.CurrentPlayerState
                .Skip(1) // 初期値を無視する場合
                .Subscribe(OnStateChanged)
                .AddTo(_disposables);
    }

    private void OnStateChanged(PlayerHandState newState)
    {
        if ((newState & PlayerHandState.None) != 0)
        {
            Debug.Log("Player is Stretching");
            // Stretching 処理
        }

        if ((newState & PlayerHandState.Catching) != 0)
        {
            Debug.Log("Player is Rotating Left");
            // Rotating Left 処理
        }

        // 他の状態も同様に
    }

    public void Dispose()
    {
        _disposables.Dispose();
    }
}
