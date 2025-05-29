using UnityEngine;
using Unity.Cinemachine;

public class AutoSetupFollowCamera : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Vector3 cameraOffset = new Vector3(0, 2, -5);
    [SerializeField] private float damping = 0.5f;

    void Start()
    {
        if (playerTransform == null)
        {
            Debug.LogError("プレイヤーTransformが指定されていません。");
            return;
        }

        // 仮想カメラの生成
        GameObject vcamGO = new GameObject("CM Follow Camera");
        var vcam = vcamGO.AddComponent<CinemachineCamera>();
        vcam.Follow = playerTransform;
        vcam.LookAt = playerTransform;

        // 位置追従
        var posComposer = vcamGO.AddComponent<CinemachinePositionComposer>();
        posComposer.CameraDistance = cameraOffset.magnitude;
        posComposer.TargetOffset = new Vector2(cameraOffset.x, cameraOffset.y);
        posComposer.Damping.x = damping;
        posComposer.Damping.y = damping;
        posComposer.Damping.z = damping;

        //// 回転制御には Recomposer を使う
        //var recomposer = vcamGO.AddComponent<CinemachineRotationComposer>();
        //recomposer.m_RecenterToTargetHeading.m_enabled = true;
        //recomposer.m_RecenterToTargetHeading.m_RecenteringTime = 1.0f;

        Debug.Log("Unity6対応カメラ：なめらか位置・回転追従を自動セットアップしました。");
    }
}
