using UnityEngine;
using Unity.Cinemachine;

public class AutoSetupFollowCamera : MonoBehaviour
{
    [Header("�v���C���[��Transform���w��")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Vector3 cameraOffset = new Vector3(0, 2, -5);

    void Start()
    {
        if (playerTransform == null)
        {
            Debug.LogError("�v���C���[Transform���w�肳��Ă��܂���B");
            return;
        }

        GameObject vcamGO = new GameObject("CM Follow Camera");
        var vcam = vcamGO.AddComponent<CinemachineCamera>();
        vcam.Follow = playerTransform;
        vcam.LookAt = playerTransform;

        // CinemachineFollow �� Transform ����̑��΃I�t�Z�b�g�ŒǏ]
        var follow = vcamGO.AddComponent<CinemachineFollow>();
        follow.FollowOffset = cameraOffset;

        // CinemachineRotationComposer �͎����̒Ǐ]�ݒ�
        vcamGO.AddComponent<CinemachineRotationComposer>();

        Debug.Log("Cinemachine Camera �� Unity 6 �Ή��`���Ŏ����ݒ肵�܂����B");
    }
}
