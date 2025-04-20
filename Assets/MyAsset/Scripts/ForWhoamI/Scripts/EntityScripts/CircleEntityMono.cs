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

    float _acceleration = 5;
    float _moveSpeed;
    float _actionInterval;
    
    Coroutine? _actionLoopCoroutine;

    void FixedUpdate()
    {
        if(!_isEnable)
            return;
        
        Move();
    }
    
    public void Init(float moveSpeed,float actionInterval)
    {
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
        DisEnableEntity();
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