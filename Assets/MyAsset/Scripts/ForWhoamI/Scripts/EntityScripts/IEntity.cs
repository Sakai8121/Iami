
using System;
using UnityEngine;

public interface IEntity
{
    void Init(float moveSpeed,float actionInterval,bool isTruth,Action<bool,IEntity> checkTruth);
    void EnableEntity();
    void DisEnableEntity();
    void Action();
    void Caught(Vector2 target);
    void Move();
    void Destroy();
    void SuccessAnimation();
    void FailAnimation();
}