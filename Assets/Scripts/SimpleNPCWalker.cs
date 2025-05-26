using UnityEngine;
using System.Collections;

public class SimpleNPCWalker : MonoBehaviour
{
    [Header("�ړ��ݒ�")]
    public float moveSpeed = 1f;
    public float moveDuration = 1.5f;
    public float waitDuration = 2f;

    [Header("�����ύX")]
    public bool rotateIn2D = true;

    [Header("�ړ�����")]
    public float moveRange = 3f; // �͈͐������a
    private Vector3 startPosition;

    private Vector3 currentDirection;
    private bool isMoving = false;
    private bool isTalking = false;

    private void Start()
    {
        startPosition = transform.position;
        StartCoroutine(WalkRoutine());
    }

    private IEnumerator WalkRoutine()
    {
        while (true)
        {
            if (isTalking)
            {
                yield return null;
                continue;
            }

            PickNewDirection();

            // �ړ��t�F�[�Y
            isMoving = true;
            float timer = 0f;
            while (timer < moveDuration)
            {
                if (isTalking) break;

                // �͈͊O�Ȃ狭���X�g�b�v
                Vector3 nextPos = transform.position + currentDirection * moveSpeed * Time.deltaTime;
                if (Vector3.Distance(startPosition, nextPos) > moveRange)
                {
                    break;
                }

                transform.Translate(currentDirection * moveSpeed * Time.deltaTime);
                timer += Time.deltaTime;
                yield return null;
            }
            isMoving = false;

            // ��~�t�F�[�Y
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

            // �����ύX
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
    /// ��b���͓����Ȃ��悤�ɂ���iTalkTrigger�Ȃǂ���Ăяo���j
    /// </summary>
    public void SetTalking(bool talking)
    {
        isTalking = talking;
    }
}
