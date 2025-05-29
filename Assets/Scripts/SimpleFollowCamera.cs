using UnityEngine;

public class SimpleFollowCamera : MonoBehaviour
{
    [Header("プレイヤーのTransform")]
    [SerializeField] private Transform playerTransform;

    [Header("プレイヤーからの相対オフセット")]
    [SerializeField] private Vector3 offset = new Vector3(0, 2, -5);

    [Header("追従スピード")]
    [SerializeField] private float smoothSpeed = 5f;

    private void LateUpdate()
    {
        if (playerTransform == null) return;

        // ターゲット位置 = プレイヤーの位置 + 後方オフセット
        Vector3 targetPosition = playerTransform.position + playerTransform.rotation * offset;

        // カメラの位置をなめらかに補間
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * smoothSpeed);

        // プレイヤーの向いている方向を向く（完全追従）
        transform.rotation = Quaternion.Lerp(transform.rotation, playerTransform.rotation, Time.deltaTime * smoothSpeed);
    }
}
