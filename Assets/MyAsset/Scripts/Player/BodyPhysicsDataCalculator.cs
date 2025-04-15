using UnityEngine;
using VContainer;
using Cysharp.Threading.Tasks;
using System.Threading;

public class BodyPhysicsDataCalculator
{
    public ObservableProperty<Vector2> BodyScale { get; private set; }
    public ObservableProperty<Quaternion> BodyRotation { get; private set; }

    const float maxBodySize = 5.0f;
    const float minBodySize = 1.0f;
    const float rotateValue = 90;

    const float stretchSpeed = 1.5f;
    const float contractSpeed = 3f;
    const float rotateSpeed = 120.0f;
    
    const float cancelRotationSpeed = 360f; // 1秒で360度回転する速度

    const float bodyXSize = 0.35f;

    CancellationTokenSource _stretchCts;
    CancellationTokenSource _contractCts;
    CancellationTokenSource _rotateCts;
    CancellationTokenSource _cancelRotateCts;

    Quaternion _preRotation;
    bool _isCancelling;

    [Inject]
    public BodyPhysicsDataCalculator()
    {
        BodyScale = new ObservableProperty<Vector2>(new Vector2(bodyXSize, minBodySize));
        BodyRotation = new ObservableProperty<Quaternion>(Quaternion.identity);

        _preRotation = Quaternion.identity;
        _isCancelling = false;
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
        _rotateCts?.Cancel();
        _cancelRotateCts?.Cancel();
        _rotateCts = new CancellationTokenSource();
        RotateAsync(rotateValue, _rotateCts.Token).Forget();
    }

    public void StartRotateLeft()
    {
        _rotateCts?.Cancel();
        _cancelRotateCts?.Cancel();
        _rotateCts = new CancellationTokenSource();
        RotateAsync(-rotateValue, _rotateCts.Token).Forget();
    }

    public void CancelRotate()
    {
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
    }

    async UniTaskVoid ContractAsync(CancellationToken token)
    {
        while (BodyScale.Value.y > minBodySize)
        {
            token.ThrowIfCancellationRequested();
            BodyScale.Value = new Vector2(bodyXSize, BodyScale.Value.y - contractSpeed * Time.deltaTime);
            await UniTask.Yield(PlayerLoopTiming.Update, token);
        }
        BodyScale.Value = new Vector2(bodyXSize, minBodySize);
    }

    async UniTaskVoid RotateAsync(float angle, CancellationToken token)
    {
        float rotated = 0f;
        while (Mathf.Abs(rotated) < Mathf.Abs(angle))
        {
            token.ThrowIfCancellationRequested();
            float delta = rotateSpeed * Time.deltaTime;
            if (Mathf.Abs(rotated + delta) > Mathf.Abs(angle))
                delta = angle - rotated;

            rotated += delta;
            BodyRotation.Value *= Quaternion.Euler(0, 0, delta);
            await UniTask.Yield(PlayerLoopTiming.Update, token);
        }

        _preRotation = BodyRotation.Value;
    }

    async UniTaskVoid CancelRotateAsync(CancellationToken token)
    {
        _isCancelling = true;

        Quaternion targetRotation = _preRotation;

        while (Quaternion.Angle(BodyRotation.Value, targetRotation) > 0.01f)
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
        _isCancelling = false;
    }

}
