#nullable enable

using UnityEngine;
using VContainer;

public class BodyPivotChangerMono : MonoBehaviour
{
    [SerializeField] Transform parentTransform = null!;
    [SerializeField] Transform childTransform = null!;
    
    [Inject]
    public void Construct(BodyColliderStateHolder bodyColliderStateHolder)
    {
        bodyColliderStateHolder.BodyPivotChangeAction += ChangePivot;
    }

    void ChangePivot(BodyPivot bodyPivot,BodyColliderDirection bodyColliderDirection)
    {
        var pivotPosition = Vector3.zero;
        float pivotX = childTransform.lossyScale.x / 2.0f;
        float pivotY = childTransform.lossyScale.y / 2.0f;
        switch (bodyPivot)
        {
            case BodyPivot.UnderLeft:
                if (bodyColliderDirection == BodyColliderDirection.Under)
                {
                    pivotPosition.x = pivotX * (-1);
                    pivotPosition.y = pivotY * (-1);
                }
                else if (bodyColliderDirection == BodyColliderDirection.Left)
                {
                    pivotPosition.x = pivotY;
                    pivotPosition.y = pivotX * (-1);
                }
                break;
            case BodyPivot.UnderRight:
                if (bodyColliderDirection == BodyColliderDirection.Under)
                {
                    pivotPosition.x = pivotX;
                    pivotPosition.y = pivotY * (-1);
                }
                else if (bodyColliderDirection == BodyColliderDirection.Right)
                {
                    pivotPosition.x = pivotY * (-1);
                    pivotPosition.y = pivotX * (-1);
                }
                break;
            case BodyPivot.UpperLeft:
                if (bodyColliderDirection == BodyColliderDirection.Up)
                {
                    pivotPosition.x = pivotX;
                    pivotPosition.y = pivotY * (-1);
                }
                else if (bodyColliderDirection == BodyColliderDirection.Left)
                {
                    pivotPosition.x = pivotY * (-1);
                    pivotPosition.y = pivotX * (-1);
                }
                break;
            case BodyPivot.UpperRight:
                if (bodyColliderDirection == BodyColliderDirection.Up)
                {
                    pivotPosition.x = pivotX * (-1);
                    pivotPosition.y = pivotY * (-1);
                }
                else if (bodyColliderDirection == BodyColliderDirection.Right)
                {
                    pivotPosition.x = pivotY;
                    pivotPosition.y = pivotX * (-1);
                }
                break;
        }
        childTransform.parent = null;
        parentTransform.position = childTransform.position + pivotPosition;
        childTransform.parent = parentTransform;
    }
}