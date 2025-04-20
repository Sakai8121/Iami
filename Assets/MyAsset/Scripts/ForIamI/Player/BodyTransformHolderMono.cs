#nullable enable

using UnityEngine;

//Transformを直接後悔はしない読み取り専用クラス
public class BodyTransformHolderMono: MonoBehaviour
{
    [SerializeField] Transform bodyTransform = null!;
    [SerializeField] Transform scalePivotTransform = null!;

    public Vector3 ScalePivotPosition()
    {
        return scalePivotTransform.position;
    }

    public Vector3 TransformUp()
    {
        return scalePivotTransform.up;
    }
    
    public float RotateZ()
    {
        return bodyTransform.rotation.eulerAngles.z;
    }
}