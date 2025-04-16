using System;
using UnityEngine;
using VContainer;
using Cysharp.Threading.Tasks;
using System.Threading;

public class BodyPhysicsDataCalculator
{
    public ObservableProperty<Vector2> BodyScale { get; private set; }
    public ObservableProperty<Quaternion> BodyRotation { get; private set; }
    public ObservableProperty<Vector3> BodyPosition { get; private set; }

    public event Action EndStretchAction = () => { };
    public event Action EndContractAction = () => { };
    public event Action EndRotateRightAction = () => { };
    public event Action EndRotateLeftAction = () => { };
    public event Action EndCancelRotateAction = () => { };

    const float maxBodySize = 5.0f;
    const float minBodySize = 1.0f;
    const float rotateValue = 90;

    const float stretchSpeed = 3f;
    const float contractSpeed = 3f;
    const float rotateSpeed = 120.0f;
    
    const float cancelRotationSpeed = 120.0f; // 1秒で360度回転する速度

    const float bodyXSize = 0.35f;

    CancellationTokenSource _stretchCts;
    CancellationTokenSource _contractCts;
    CancellationTokenSource _rotateCts;
    CancellationTokenSource _cancelRotateCts;

    Quaternion _preRotation;
    RotateState _currentRotateState;

    readonly BodyTransformHolderMono _bodyTransformHolderMono;

    [Inject]
    public BodyPhysicsDataCalculator(BodyTransformHolderMono bodyTransformHolderMono)
    {
        _bodyTransformHolderMono = bodyTransformHolderMono;
        
        BodyScale = new ObservableProperty<Vector2>(new Vector2(bodyXSize, minBodySize));
        BodyRotation = new ObservableProperty<Quaternion>(Quaternion.identity);
        BodyPosition = new ObservableProperty<Vector3>(new Vector3(0,0,0));

        _preRotation = Quaternion.identity;
        _currentRotateState = RotateState.None;
    }

    public void StartStretch()
    {
        _contractCts?.Cancel();
        _stretchCts?.Cancel();
        _stretchCts = new CancellationTokenSource();
        StretchAsync(_stretchCts.Token).Forget();
    }

    public void StartContract()
    {
        _stretchCts?.Cancel();
        _contractCts?.Cancel();
        _contractCts = new CancellationTokenSource();
        ContractAsync(_contractCts.Token).Forget();
    }

    public void StartRotateRight()
    {
        if(_currentRotateState == RotateState.Right)
            return;

        _currentRotateState = RotateState.Right;
        
        _rotateCts?.Cancel();
        _cancelRotateCts?.Cancel();
        _rotateCts = new CancellationTokenSource();
        RotateAsync(-rotateValue, _rotateCts.Token).Forget();
    }

    public void StartRotateLeft()
    {
        if (_currentRotateState == RotateState.Left)
            return;

        _currentRotateState = RotateState.Left;
        
        _rotateCts?.Cancel();
        _cancelRotateCts?.Cancel();
        _rotateCts = new CancellationTokenSource();
        RotateAsync(rotateValue, _rotateCts.Token).Forget();
    }

    public void CancelRotate()
    {
        if (_currentRotateState == RotateState.Cancel)
            return;

        _currentRotateState = RotateState.Cancel;
        
        _rotateCts?.Cancel();
        _cancelRotateCts?.Cancel();
        _cancelRotateCts = new CancellationTokenSource();
        CancelRotateAsync(_cancelRotateCts.Token).Forget();
    }

    async UniTaskVoid StretchAsync(CancellationToken token)
    {
        while (BodyScale.Value.y < maxBodySize)
        {
            token.ThrowIfCancellationRequested();
            BodyScale.Value = new Vector2(bodyXSize, BodyScale.Value.y + stretchSpeed * Time.deltaTime);
            await UniTask.Yield(PlayerLoopTiming.Update, token);
        }
        BodyScale.Value = new Vector2(bodyXSize, maxBodySize);
        EndStretchAction();
    }

    async UniTaskVoid ContractAsync(CancellationToken token)
    {
        while (BodyScale.Value.y > minBodySize)
        {
            token.ThrowIfCancellationRequested();
            float changeScale = contractSpeed * Time.deltaTime;
            BodyScale.Value = new Vector2(bodyXSize, BodyScale.Value.y - changeScale);
            InchwormMove(changeScale);
            await UniTask.Yield(PlayerLoopTiming.Update, token);
        }
        BodyScale.Value = new Vector2(bodyXSize, minBodySize);
        EndContractAction();
    }

    async UniTaskVoid RotateAsync(float angle, CancellationToken token)
    {
        float rotated = 0f;
        while (Mathf.Abs(rotated) < Mathf.Abs(angle))
        {
            token.ThrowIfCancellationRequested();

            float delta = rotateSpeed * Time.deltaTime;
            delta = Mathf.Sign(angle) * delta; // ← 符号を調整

            if (Mathf.Abs(rotated + delta) > Mathf.Abs(angle))
                delta = angle - rotated;

            rotated += delta;
            BodyRotation.Value *= Quaternion.Euler(0, 0, delta);
            await UniTask.Yield(PlayerLoopTiming.Update, token);
        }

        _preRotation = BodyRotation.Value;

        if (_currentRotateState == RotateState.Right)
            EndRotateRightAction();
        else if (_currentRotateState == RotateState.Left)
            EndRotateLeftAction();
        _currentRotateState = RotateState.None;
    }


    async UniTaskVoid CancelRotateAsync(CancellationToken token)
    {
        Quaternion targetRotation = _preRotation;

        while (Quaternion.Angle(BodyRotation.Value, targetRotation) > cancelRotationSpeed * Time.deltaTime)
        {
            token.ThrowIfCancellationRequested();

            BodyRotation.Value = Quaternion.RotateTowards(
                BodyRotation.Value,
                targetRotation,
                cancelRotationSpeed * Time.deltaTime
            );
            await UniTask.Yield(PlayerLoopTiming.Update, token);
        }

        // 誤差が残らないように最終的にピッタリ合わせる
        BodyRotation.Value = targetRotation;
        _preRotation = targetRotation;

        EndCancelRotateAction();
        _currentRotateState = RotateState.None;
    }

    void InchwormMove(float changeScale)
    {
        BodyPosition.Value = _bodyTransformHolderMono.BodyPosition() + _bodyTransformHolderMono.TransformUp() * changeScale;
    }
}

enum RotateState
{
    None,
    Right,
    Left,
    Cancel
}
