using UnityEngine;

public class FollowPositionFromTop : MonoBehaviour
{
    [SerializeField] Transform target;

    // ��[����̑��΃I�t�Z�b�g�itarget�̃��[�J�����ɏ]���j
    [SerializeField] Vector3 offsetFromTop = new Vector3(0, 0.2f, 0);

    void LateUpdate()
    {
        if (target == null) return;

        // ��[�̃��[���h���W�����߂�
        Vector3 top = target.position + target.up * (target.lossyScale.y/2.0f);

        // �I�t�Z�b�g��target�̃��[�J����Ԃ��烏�[���h��Ԃ֕ϊ�
        Vector3 offsetWorld = target.TransformDirection(offsetFromTop);

        // �ʒu��ݒ�itop + offset�j
        transform.position = top + offsetWorld;

        // ��]�͒Ǐ]�i�K�v�Ȃ璲��OK�j
        transform.rotation = target.rotation;

        // �X�P�[���͈ێ�
    }
}
