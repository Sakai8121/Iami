# nullable enable
using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class EntityActionExecutor : ITickable, IDisposable
{
    public event Action EntityAction = () => { };
    public event Action DeactivateSpaceUIAction = () => { };
    public event Action ActiveSpaceUIAction = () => { };
    public event Action<float> ChangeUIFillAmount = (_) => { }; 

    bool _isLocked;
    bool _isEnable;
    const float reActiveTime = 1.5f;
    float _elapsed = 0f;

    CancellationTokenSource _cts;

    //���蒆�̓��b�N�������Đ�΂�ReActive�����Ȃ�
    //reActiveTime�̊ԑ���͂ł��Ȃ�
    [Inject]
    public EntityActionExecutor(TruthCheckExecutor truthCheckExecutor,
        GameStateObserver gameStateObserver)
    {
        _isEnable = true;
        _isLocked = true;
        _elapsed = reActiveTime;

        truthCheckExecutor.RestartGameAction += UnLockActivate;
        gameStateObserver.Subscribe(GameState.InGame, UnLockActivate);
    }

    public void Tick()
    {
        ChangeUIFillAmount(_elapsed/reActiveTime);

        if(!_isEnable || _isLocked) return;

        if(Input.GetKeyDown(KeyCode.Space))
        {
            ExecuteEntityAction();
        }
    }

    public void Dispose()
    {
        // ���������[�N�h�~�̂��߂ɔj��
        _cts?.Cancel();
        _cts?.Dispose();
    }

    public void LockActivate()
    {
        _isLocked = true;
        DeactivateSpaceUIAction();
    }

    public void UnLockActivate()
    {
        _isLocked = false;
        ActiveSpaceUIAction();
    }

    //�G���e�B�e�B�A�N�V���������s
    void ExecuteEntityAction()
    {
        _elapsed = 0;
        _isEnable = false;
        EntityAction();

        DeactivateSpaceUIAction();
        ReActiveTask();
    }

    void ReActiveTask()
    {
        // �����̃^�X�N���L�����Z�����Ă���V�����g�[�N�������
        _cts?.Cancel();
        _cts?.Dispose();
        _cts = new CancellationTokenSource();

        ReActiveSpaceUI(_cts.Token).Forget();
    }

    async UniTaskVoid ReActiveSpaceUI(CancellationToken token)
    {
        try
        {
            while (_elapsed < reActiveTime)
            {
                // �L�����Z���`�F�b�N�itoken.ThrowIfCancellationRequested(); �ł��j
                if (token.IsCancellationRequested) return;

                await UniTask.Yield(PlayerLoopTiming.Update, token);

                _elapsed += Time.deltaTime;
            }

            _isEnable = true;
            ActiveSpaceUIAction();
        }
        catch (OperationCanceledException)
        {
            // �L�����Z�����͉������Ȃ�
        }
    }

}