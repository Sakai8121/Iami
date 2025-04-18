using System;
using UnityEngine;
using VContainer;

public class BodyColliderStateHolder
{
    public bool IsEnableCollider => _isEnable;
    
    bool _isEnable;
    BodyColliderDirection _currentDirection;
    BodyPivot _currentPivot;

    public event Action<BodyPivot,BodyColliderDirection> BodyPivotChangeAction = (_,_) => { };

    //rotateDirectionは１が右回転で-1が左回転
    public void ChangeBodyPivot(int rotateDirection)
    {
        switch (_currentDirection)
        {
            case BodyColliderDirection.Under:
                if (rotateDirection == 1)
                    _currentPivot = BodyPivot.UnderRight;
                else
                    _currentPivot = BodyPivot.UnderLeft;
                break;
            
            case BodyColliderDirection.Right:
                if (rotateDirection == 1)
                    _currentPivot = BodyPivot.UpperRight;
                else
                    _currentPivot = BodyPivot.UnderRight;
                break;
            
            case BodyColliderDirection.Up:
                if (rotateDirection == 1)
                    _currentPivot = BodyPivot.UpperLeft;
                else
                    _currentPivot = BodyPivot.UpperRight;
                break;
            
            case BodyColliderDirection.Left:
                if (rotateDirection == 1)
                    _currentPivot = BodyPivot.UnderLeft;
                else
                    _currentPivot = BodyPivot.UpperLeft;
                break;
        }
        BodyPivotChangeAction(_currentPivot,_currentDirection);
    }
    
    public void EnableBodyCollider()
    {
        _isEnable = true;
    }

    public void DisEnableBodyCollider()
    {
        _isEnable = false;
    }

    public void ChangeBodyColliderDirection(float rotateZ)
    {
        int rotateIndex = Mathf.RoundToInt(rotateZ / 90) % 4;
        switch (rotateIndex)
        {
            case 0:
                _currentDirection = BodyColliderDirection.Under;
                break;
            case 1:
                _currentDirection = BodyColliderDirection.Left;
                break;
            case 2:
                _currentDirection = BodyColliderDirection.Up;
                break;
            case 3:
                _currentDirection = BodyColliderDirection.Right;
                break;
        }
    }
}

public enum BodyColliderDirection
{
    Under,
    Right,
    Left,
    Up
}

public enum BodyPivot
{
    UnderRight,
    UnderLeft,
    UpperRight,
    UpperLeft
}
