using UnityEngine;

public class FollowPositionFromTop : MonoBehaviour
{
    [SerializeField] Transform target;

    // 上端からの相対オフセット（targetのローカル軸に従う）
    [SerializeField] Vector3 offsetFromTop = new Vector3(0, 0.2f, 0);

    void LateUpdate()
    {
        if (target == null) return;

        // 上端のワールド座標を求める
        Vector3 top = target.position + target.up * (target.lossyScale.y/2.0f);

        // オフセットをtargetのローカル空間からワールド空間へ変換
        Vector3 offsetWorld = target.TransformDirection(offsetFromTop);

        // 位置を設定（top + offset）
        transform.position = top + offsetWorld;

        // 回転は追従（必要なら調整OK）
        transform.rotation = target.rotation;

        // スケールは維持
    }
}
