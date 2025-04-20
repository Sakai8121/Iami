#nullable enable
using System;
using UnityEngine;

public class BodyColliderViewMono : MonoBehaviour
{

    public event Action CallCancelRotateAction = () => { };
    public event Action<BodyColliderDirection> CallCancelStretchAction = (_) => { };
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        CallCancelRotateAction();
        
        foreach (ContactPoint2D contact in collision.contacts)
        {
            Vector2 normal = contact.normal;

            if (Vector2.Dot(normal, Vector2.up) > 0.5f)
            {
                Debug.Log("下から当たられた（＝相手が上にいる）");
                CallCancelStretchAction(BodyColliderDirection.Up);
            }
            if (Vector2.Dot(normal, Vector2.down) > 0.5f)
            {
                Debug.Log("上から当たられた（＝相手が下にいる）");
                CallCancelStretchAction(BodyColliderDirection.Under);
            }
            if (Vector2.Dot(normal, Vector2.left) > 0.5f)
            {
                Debug.Log("右から当たられた（＝相手が左にいる）");
                CallCancelStretchAction(BodyColliderDirection.Left);
            }
            if (Vector2.Dot(normal, Vector2.right) > 0.5f)
            {
                Debug.Log("左から当たられた（＝相手が右にいる）");
                CallCancelStretchAction(BodyColliderDirection.Right);
            }
        }
    }
}
