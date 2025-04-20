#nullable enable

using UnityEngine;

public class PlayerBodyViewMono: MonoBehaviour
{
    [SerializeField] Transform scalePivotTransform = null!;
    [SerializeField] Transform pivotTransform = null!;

    public void ChangeBodyView(Vector2 scale)
    {
        scalePivotTransform.localScale = scale;
    }

    public void ChangeRotation(Quaternion rotation)
    {
        scalePivotTransform.localRotation = rotation;
    }

    public void ChangePosition(Vector3 position)
    {
        scalePivotTransform.position = position;
    }
}