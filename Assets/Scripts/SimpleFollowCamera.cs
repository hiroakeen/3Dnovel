using UnityEngine;

public class SimpleFollowCamera : MonoBehaviour
{
    [Header("�v���C���[��Transform")]
    [SerializeField] private Transform playerTransform;

    [Header("�v���C���[����̑��΃I�t�Z�b�g")]
    [SerializeField] private Vector3 offset = new Vector3(0, 2, -5);

    [Header("�Ǐ]�X�s�[�h")]
    [SerializeField] private float smoothSpeed = 5f;

    private void LateUpdate()
    {
        if (playerTransform == null) return;

        // �^�[�Q�b�g�ʒu = �v���C���[�̈ʒu + ����I�t�Z�b�g
        Vector3 targetPosition = playerTransform.position + playerTransform.rotation * offset;

        // �J�����̈ʒu���Ȃ߂炩�ɕ��
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * smoothSpeed);

        // �v���C���[�̌����Ă�������������i���S�Ǐ]�j
        transform.rotation = Quaternion.Lerp(transform.rotation, playerTransform.rotation, Time.deltaTime * smoothSpeed);
    }
}
