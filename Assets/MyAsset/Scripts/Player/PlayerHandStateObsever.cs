using System;
using UniRx;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class PlayerHandStateObsever : IInitializable, IDisposable
{
    private readonly PlayerHandStateHolder _stateHolder;
    private readonly CompositeDisposable _disposables = new();

    [Inject]
    public PlayerHandStateObsever(PlayerHandStateHolder stateHolder)
    {
        _stateHolder = stateHolder;

        _stateHolder.CurrentPlayerState
            .Skip(1) // �����l�𖳎�����ꍇ
            .Subscribe(OnStateChanged)
            .AddTo(_disposables);
    }

    public void Initialize()
    {
        _stateHolder.CurrentPlayerState
                .Skip(1) // �����l�𖳎�����ꍇ
                .Subscribe(OnStateChanged)
                .AddTo(_disposables);
    }

    private void OnStateChanged(PlayerHandState newState)
    {
        if ((newState & PlayerHandState.None) != 0)
        {
            Debug.Log("Player is Stretching");
            // Stretching ����
        }

        if ((newState & PlayerHandState.Catching) != 0)
        {
            Debug.Log("Player is Rotating Left");
            // Rotating Left ����
        }

        // ���̏�Ԃ����l��
    }

    public void Dispose()
    {
        _disposables.Dispose();
    }
}
