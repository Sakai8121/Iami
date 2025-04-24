
using System;
using UnityEngine;

public interface IEntity
{
    bool IsTruth();
    void Init(float moveSpeed,float actionInterval,bool isTruth);
    void EnableEntity();
    void DisEnableEntity();
    void Action();
    void Caught(Vector2 target);
    void Move();
    void Destroy();
    void SuccessAnimation();
    void FailAnimation();
    void Visible();
    void InVisible();
}