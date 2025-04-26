#nullable enable

using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class CircleEntityMono:MonoBehaviour,IEntity
{
    [SerializeField] Rigidbody2D rb = null!;
    [SerializeField] SpriteRenderer entitySpriteRenderer = null!;
    [SerializeField] Material burningMaterial = null!;
    [SerializeField] Transform rightEyeTrans = null!;
    [SerializeField] Transform leftEyeTrans = null!;

    bool _isEnable;
    bool _isTruth;

    float _acceleration = 80;
    float _moveSpeed;
    float _actionInterval;
    float _burningThreshold;

    Vector2 openSize = new Vector2(0.3f, 0.5f);


    Coroutine? _actionLoopCoroutine;
    Tween? _actionTween;

    void FixedUpdate()
    {
        if(!_isEnable)
            return;
        
        Move();
    }

    public bool IsTruth()
    {
        return _isTruth;
    }
    
    public void Init(float moveSpeed,float actionInterval,bool isTruth)
    {
        _isTruth = isTruth;
        _moveSpeed = moveSpeed;
        _actionInterval = actionInterval;

        InVisible();
        EnableEntity();

        rightEyeTrans.localScale = Vector3.zero;
        leftEyeTrans.localScale = Vector3.zero;

        if (isTruth)
            entitySpriteRenderer.material = new Material(burningMaterial);

        Action();
    }

    public void EnableEntity()
    {
        _isEnable = true;
        StartActionLoop();
    }
    
    public void DisEnableEntity()
    {
        _isEnable = false;
        StopActionLoop();
    }

    public void Action()
    {
        float randomAngle = Random.Range(90, 360f);
        _actionTween = transform.DORotate(new Vector3(0, 0, randomAngle), 0.1f, RotateMode.FastBeyond360).OnComplete(() =>
        {
            Vector2 direction = transform.up;

            rb.AddForce(direction * _acceleration, ForceMode2D.Impulse); 
        });
    }

    public void Caught(Vector2 targetPosition)
    {
        DisEnableEntity();
        if(_actionTween != null)
            _actionTween.Kill();

        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.rotation = 0f;
        transform.rotation = Quaternion.identity;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        //中心に移動
        transform.DOMove(targetPosition,TimeInfoStatic.waitMovingToGoalTime);
    }

    public void Move()
    {
        Vector2 currentVelocity = rb.linearVelocity;
        
        // 最大速度を制限
        if (currentVelocity.sqrMagnitude > _moveSpeed)
        {
            rb.linearVelocity = currentVelocity.normalized * _moveSpeed;
        }
    }

    public void Destroy()
    {
        
    }

    public void SuccessAnimation()
    {
        //目を開くアニメーション
        Debug.Log("Clear");
        BlinkEyes();
    }

    public void FailAnimation()
    {
        //燃え尽きるアニメーション（新しくマテリアルを生成する）
        Debug.Log("Miss");

        // DOTweenで1秒間かけてthresholdを1.1まで上げる
        DOTween.To(() => _burningThreshold, x => {
            _burningThreshold = x;
            entitySpriteRenderer.material.SetFloat("_Threshold", _burningThreshold);
        }, 1.1f, TimeInfoStatic.burningTime);
    }

    public void Visible()
    {
        entitySpriteRenderer.enabled = true;
    }

    public void InVisible()
    {
        entitySpriteRenderer.enabled = false;
    }

    void BlinkEyes()
    {
        Sequence blinkSequence = DOTween.Sequence();

        blinkSequence.Append(rightEyeTrans.DOScale(openSize, 0.2f).SetEase(Ease.OutQuad));
        blinkSequence.Join(leftEyeTrans.DOScale(openSize, 0.2f).SetEase(Ease.OutQuad));

        blinkSequence.AppendInterval(0.1f);

        blinkSequence.Append(rightEyeTrans.DOScale(Vector2.zero, 0.2f).SetEase(Ease.InQuad));
        blinkSequence.Join(leftEyeTrans.DOScale(Vector2.zero, 0.2f).SetEase(Ease.InQuad));
        
        blinkSequence.AppendInterval(0.1f);

        blinkSequence.Append(rightEyeTrans.DOScale(openSize, 0.2f).SetEase(Ease.OutQuad));
        blinkSequence.Join(leftEyeTrans.DOScale(openSize, 0.2f).SetEase(Ease.OutQuad));

        // ループさせたい場合（任意）
        blinkSequence.SetLoops(3, LoopType.Restart);
    }

    void StartActionLoop()
    {
        if (_actionLoopCoroutine == null)
        {
            _actionLoopCoroutine = StartCoroutine(ActionLoop());
        }
    }

    void StopActionLoop()
    {
        if (_actionLoopCoroutine != null)
        {
            StopCoroutine(_actionLoopCoroutine);
            _actionLoopCoroutine = null;
        }
    }

    IEnumerator ActionLoop()
    {
        while (_isEnable)
        {
            float interval = UnityEngine.Random.Range(_actionInterval * 0.5f, _actionInterval * 1.5f);
            yield return new WaitForSeconds(interval);
        
            if (_isEnable) // 念のためチェック
                Action();
        }
    }
}