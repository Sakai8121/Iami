using UnityEngine;
using VContainer;
using Cysharp.Threading.Tasks;
using System.Threading;

public class BodyPhysicsDataCalculator
{
    // ŠO•”‚©‚çŽæ“¾—p
    public ObservableProperty<Vector2> BodyScale { get; private set; }
    public ObservableProperty<Quaternion> BodyRotation { get; private set; }

    Vector2 _bodyScale;
    Quaternion _bodyRotation;

    const float maxBodySize = 5.0f;
    const float minBodySize = 1.0f;
    const float rotateValue = 90;

    const float stretchSpeed = 0.05f;
    const float contractSpeed = 0.1f;
    const float rotateSpeed = 1.0f;
    
    CancellationTokenSource _stretchCts;
    CancellationTokenSource _contractCts;
    CancellationTokenSource _rotateCts;
    CancellationTokenSource _cancelRotateCts;

    [Inject]
    public BodyPhysicsDataCalculator()
    {
        _bodyScale = new Vector2(1, minBodySize);
        _bodyRotation = Quaternion.identity;
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

    public void CancellRotate()
    {
        _rotateCts?.Cancel();
        _cancelRotateCts?.Cancel();
        _cancelRotateCts = new CancellationTokenSource();
        CancelRotateAsync(_cancelRotateCts.Token).Forget();
    }

    async UniTaskVoid StretchAsync(CancellationToken token)
    {
        while (_bodyScale.y < maxBodySize)
        {
            token.ThrowIfCancellationRequested();
            _bodyScale.y += stretchSpeed;
            await UniTask.Yield(PlayerLoopTiming.Update, token);
        }
        _bodyScale.y = maxBodySize;
    }

    async UniTaskVoid ContractAsync(CancellationToken token)
    {
        while (_bodyScale.y > minBodySize)
        {
            token.ThrowIfCancellationRequested();
            _bodyScale.y -= contractSpeed;
            await UniTask.Yield(PlayerLoopTiming.Update, token);
        }
        _bodyScale.y = minBodySize;
    }

    async UniTaskVoid RotateAsync(float angle, CancellationToken token)
    {
        float rotated = 0f;
        while (Mathf.Abs(rotated) < Mathf.Abs(angle))
        {
            token.ThrowIfCancellationRequested();
            float delta = rotateSpeed;
            if (Mathf.Abs(rotated + delta) > Mathf.Abs(angle))
                delta = angle - rotated;

            rotated += delta;
            _bodyRotation *= Quaternion.Euler(0, 0, delta);
            await UniTask.Yield(PlayerLoopTiming.Update, token);
        }
    }

    async UniTaskVoid CancelRotateAsync(CancellationToken token)
    {
        Quaternion startRotation = _bodyRotation;
        Quaternion targetRotation = Quaternion.identity;

        float t = 0f;
        const float duration = 0.5f; // adjust as needed

        while (t < 1f)
        {
            token.ThrowIfCancellationRequested();
            t += Time.deltaTime / duration;
            _bodyRotation = Quaternion.Lerp(startRotation, targetRotation, t);
            await UniTask.Yield(PlayerLoopTiming.Update, token);
        }

        _bodyRotation = targetRotation;
    }
}
