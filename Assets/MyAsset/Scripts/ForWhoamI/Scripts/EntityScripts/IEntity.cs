
using System;

public interface IEntity
{
    void Init(float moveSpeed,float actionInterval,bool isTruth,Action<bool,IEntity> checkTruth);
    void EnableEntity();
    void DisEnableEntity();
    void Action();
    void Caught();
    void Move();
    void Destroy();
    void SuccessAnimation();
    void FailAnimation();
}