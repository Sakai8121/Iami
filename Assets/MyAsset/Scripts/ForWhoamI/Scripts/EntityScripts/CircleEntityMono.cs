#nullable enable

using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class CircleEntityMono:MonoBehaviour,IEntity
{
    [SerializeField] Rigidbody2D rb = null!;

    bool _isEnable;
    bool _isCaught;
    bool _isTruth;

    float _acceleration = 5;
    float _moveSpeed;
    float _actionInterval;
    
    Coroutine? _actionLoopCoroutine;

    Action<bool,IEntity> checkTruthAction;

    void FixedUpdate()
    {
        if(!_isEnable)
            return;
        
        Move();
    }
    
    public void Init(float moveSpeed,float actionInterval,bool isTruth,Action<bool,IEntity> checkTruth)
    {
        _isTruth = isTruth;
        checkTruthAction = checkTruth;
        _moveSpeed = moveSpeed;
        _actionInterval = actionInterval;

        EnableEntity();
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
        float randomAngle = Random.Range(0, 360f);
        transform.DORotate(new Vector3(0, 0, randomAngle), 0.5f, RotateMode.FastBeyond360);
    }

    public void Caught()
    {
        rb.rotation = 0;
        rb.angularVelocity = 0;
        rb.linearVelocity = new Vector2(0, 0);
        rb.freezeRotation = true;
        rb.constraints = RigidbodyConstraints2D.FreezePositionX
                        | RigidbodyConstraints2D.FreezePositionY;
        //中心に移動
        transform.DOMove(PositionHolderStatic.goalPosition,TimeInfoStatic.waitMovingToGoalTime);

        //本物かどうかを判定
        checkTruthAction(_isTruth,this);
        _isCaught = true;
    }

    public void Move()
    {
        Vector2 direction = transform.up;
        Vector2 currentVelocity = rb.linearVelocity;

        // 最大速度を超えていないときのみ加速
        if (currentVelocity.sqrMagnitude < _moveSpeed)
        {
            rb.AddForce(direction * _acceleration, ForceMode2D.Force);
        }
    }

    public void Destroy()
    {
        Destroy(this.gameObject);
        _isCaught = false;
    }

    public void SuccessAnimation()
    {
        //目を開くアニメーション

    }

    public void FailAnimation()
    {
        //燃え尽きるアニメーション（新しくマテリアルを生成する）

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