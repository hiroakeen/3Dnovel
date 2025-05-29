using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class SimpleNPCWalker : MonoBehaviour
{
    [Header("移動設定")]
    public float moveSpeed = 1f;
    public float moveDuration = 1.5f;
    public float waitDuration = 2f;

    [Header("方向変更")]
    public bool rotateIn2D = true;

    [Header("移動制限")]
    public float moveRange = 3f; // 範囲制限半径
    private Vector3 startPosition;

    private Vector3 currentDirection;
    private bool isTalking = false;

    private Animator animator;

    private void Start()
    {
        startPosition = transform.position;
        animator = GetComponent<Animator>();
        StartCoroutine(WalkRoutine());
    }

    private IEnumerator WalkRoutine()
    {
        while (true)
        {
            if (isTalking)
            {
                SetAnimationSpeed(0f);
                yield return null;
                continue;
            }

            PickNewDirection();

          
            // 停止フェーズ
            float wait = 0f;
            while (wait < waitDuration)
            {
                if (isTalking) break;
                wait += Time.deltaTime;
                yield return null;
            }
        }
    }

    private void PickNewDirection()
    {
        if (rotateIn2D)
        {
            float x = Random.value < 0.5f ? -1f : 1f;
            currentDirection = new Vector3(x, 0, 0);

            // 向き変更
            Vector3 localScale = transform.localScale;
            localScale.x = Mathf.Sign(x) * Mathf.Abs(localScale.x);
            transform.localScale = localScale;
        }
        else
        {
            float angle = Random.Range(0f, 360f);
            currentDirection = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)).normalized;

            if (currentDirection != Vector3.zero)
            {
                Quaternion targetRot = Quaternion.LookRotation(currentDirection);
                transform.rotation = targetRot;
            }
        }
    }

    /// <summary>
    /// 会話中は動かないようにする（TalkTriggerなどから呼び出し）
    /// </summary>
    public void SetTalking(bool talking)
    {
        isTalking = talking;
        if (talking)
        {
            SetAnimationSpeed(0f);
        }
    }

    private void SetAnimationSpeed(float speed)
    {
        if (animator != null)
        {
            animator.SetFloat("Speed", speed);
        }
    }
}
