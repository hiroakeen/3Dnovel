using UnityEngine;
using Unity.Cinemachine;

public class AutoSetupFollowCamera : MonoBehaviour
{
    [Header("プレイヤーのTransformを指定")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Vector3 cameraOffset = new Vector3(0, 2, -5);

    void Start()
    {
        if (playerTransform == null)
        {
            Debug.LogError("プレイヤーTransformが指定されていません。");
            return;
        }

        GameObject vcamGO = new GameObject("CM Follow Camera");
        var vcam = vcamGO.AddComponent<CinemachineCamera>();
        vcam.Follow = playerTransform;
        vcam.LookAt = playerTransform;

        // CinemachineFollow は Transform からの相対オフセットで追従
        var follow = vcamGO.AddComponent<CinemachineFollow>();
        follow.FollowOffset = cameraOffset;

        // CinemachineRotationComposer は視線の追従設定
        vcamGO.AddComponent<CinemachineRotationComposer>();

        Debug.Log("Cinemachine Camera を Unity 6 対応形式で自動設定しました。");
    }
}
