#nullable enable
using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class CatchEntitySystem : ITickable
{ 
    bool _isEnabled = false;
    const float reActiveTime = 6f;
    float _elapsed = 0f;

    GoalControllerMono _goalControllerMono;
    TruthCheckExecutor _truthCheckExecutor;
    EntityActionExecutor _entityActionExecutor;
    CancellationTokenSource _cts;
    
    public event Action DeactivateClickUIAction = () => { };
    public event Action ActiveClickUIAction = () => { };
    public event Action<float> ChangeUIFillAmount = (_) => { }; 

    [Inject] 
    public CatchEntitySystem(GoalControllerMono goalControllerMono,EntityActionExecutor entityActionExecutor,
        TruthCheckExecutor truthCheckExecutor)
    {
        _goalControllerMono = goalControllerMono;
        _entityActionExecutor = entityActionExecutor;
        _truthCheckExecutor = truthCheckExecutor;
        _elapsed = reActiveTime;

        DisableSystem();
    }

    public void Tick()
    {
        ChangeUIFillAmount(_elapsed/reActiveTime);
        
        if (!_isEnabled) return;

        if (Input.GetMouseButtonDown(0)) // 左クリック
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null)
            {
                IEntity? entity = hit.collider.GetComponent<IEntity>();
                if (entity != null)
                {
                    SoundManagerMono.Instance.PlaySEOneShot(SESoundData.SE.Check);
                    
                    entity.Caught(_goalControllerMono.GoalPosition());
                    _truthCheckExecutor.CheckTruth(entity.IsTruth(), entity);

                    DisableSystem();
                    _entityActionExecutor.LockActivate();
                    _elapsed = 0;
                    ReActiveTask();
                }
            }
        }
    }

    public void EnableSystem()
    {
        _isEnabled = true;
        ActiveClickUIAction();
        Debug.LogError("Ena");
    }

    void DisableSystem()
    {
        _isEnabled = false;
        DeactivateClickUIAction();
        Debug.LogError("DisE");
    }
    
    void ReActiveTask()
    {
        // 既存のタスクをキャンセルしてから新しいトークンを作る
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
                // キャンセルチェック（token.ThrowIfCancellationRequested(); でも可）
                if (token.IsCancellationRequested) return;

                await UniTask.Yield(PlayerLoopTiming.Update, token);

                _elapsed += Time.deltaTime;
            }

            EnableSystem();
        }
        catch (OperationCanceledException)
        {
            // キャンセル時は何もしない
        }
    }
}
