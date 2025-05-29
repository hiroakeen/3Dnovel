using UnityEngine;

[RequireComponent(typeof(Animator))]
public class SimplePlayerAnimator : MonoBehaviour
{
    [Header("移動速度のしきい値（これ以上なら歩いてるとみなす）")]
    [SerializeField] private float moveThreshold = 0.01f;

    private Animator animator;
    private Vector3 lastPosition;

    private void Start()
    {
        animator = GetComponent<Animator>();
        lastPosition = transform.position;
    }

    private void Update()
    {
        // 実際の移動距離から速度を算出
        float moveSpeed = (transform.position - lastPosition).magnitude / Time.deltaTime;
        lastPosition = transform.position;

        // 移動してるかどうかを判定
        bool isMoving = moveSpeed > moveThreshold;

        // Animatorにboolを渡す（"isMoving"というパラメーターを使う）
        animator.SetBool("isMoving", isMoving);
    }
}
