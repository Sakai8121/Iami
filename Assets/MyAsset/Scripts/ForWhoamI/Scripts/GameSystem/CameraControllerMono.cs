#nullable enable
using DG.Tweening;
using UnityEngine;

public class CameraControllerMono : MonoBehaviour
{
    float _stage1OrthoSize= 1f;
    float _stage2OrthoSize = 2f;
    float _stage3OrthoSize = 3f;

    float _stageHeight = 3.5f;

    float _zoomedFOV = 30f;         // ズーム時のFOV
    float _stageDurationRatio = 0.1f;
    float _zoomDurationRatio = 0.3f;
    float _returnDurationRatio = 0.1f;

    Vector3 _originalPos = Vector3.zero;
    float _originalFOV = 60f;

    Camera? _camera;

    private void Awake()
    {
        _originalPos = transform.position;
        _camera = GetComponent<Camera>();
        if (_camera != null)
        {
            _originalFOV = _camera.orthographicSize;
            _zoomedFOV = _originalFOV * 0.3f;
        }
    }

    public void SuccessCameraAnimation(float animationTime)
    {
        Sequence seq = CloseUpAnimation(animationTime); // ズームアップのみ
        Vector3 movedPos = transform.position + new Vector3(2, _stageHeight, 0);
        seq.Append(transform.DOMove(movedPos, 0.1f).SetEase(Ease.InOutQuad));
    }

    public void FailCameraAnimation(float animationTime)
    {
        Sequence seq = CloseUpAnimation(animationTime);
        float returnDuration = animationTime * _returnDurationRatio;

        // 位置戻し
        seq.Append(transform.DOMove(_originalPos, returnDuration).SetEase(Ease.InOutQuad));

        // ズームアウト（FOV戻し）
        if (_camera != null)
        {
            seq.Join(_camera.DOOrthoSize(_originalFOV, returnDuration).SetEase(Ease.OutQuad));
        }
    }

    Sequence CloseUpAnimation(float animationTime)
    {
        Vector3 startPos = transform.position;

        Vector3 stageHeight = startPos + new Vector3(0, _stageHeight, 0);

        float stageDuration = animationTime * _stageDurationRatio;
        float zoomDuration = animationTime * _zoomDurationRatio;

        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOMove(stageHeight, stageDuration).SetEase(Ease.OutQuad));
        seq.Append(_camera.DOOrthoSize(_originalFOV - (_originalFOV  - _zoomedFOV) * 0.6f, zoomDuration).SetEase(Ease.OutQuad));
        seq.Append(_camera.DOOrthoSize(_originalFOV - (_originalFOV  - _zoomedFOV) * 0.3f, zoomDuration).SetEase(Ease.OutQuad));
        seq.Append(_camera.DOOrthoSize(_originalFOV - (_originalFOV  - _zoomedFOV), zoomDuration).SetEase(Ease.OutQuad));

        return seq;
    }
}
