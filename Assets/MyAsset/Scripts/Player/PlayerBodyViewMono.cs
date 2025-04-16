#nullable enable

using UnityEngine;

public class PlayerBodyViewMono: MonoBehaviour
{
    [SerializeField] Transform playerBodyTransform = null!;

    public void ChangeBodyView(Vector2 scale)
    {
        playerBodyTransform.localScale = scale;
    }

    public void ChangeRotation(Quaternion rotation)
    {
        playerBodyTransform.rotation = rotation;
    }

    public void ChangePosition(Vector3 position)
    {
        playerBodyTransform.position = position;
    }
}