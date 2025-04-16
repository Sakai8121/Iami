#nullable enable

using UnityEngine;

//Transformを直接後悔はしない読み取り専用クラス
public class BodyTransformHolderMono: MonoBehaviour
{
    [SerializeField] Transform bodyTransform = null!;

    public Vector3 BodyPosition()
    {
        return bodyTransform.position;
    }

    public Vector3 TransformUp()
    {
        return bodyTransform.up;
    }
}