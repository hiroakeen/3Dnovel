using UnityEngine;
using System.Collections;

public class LookAroundNPC : MonoBehaviour
{
    [Header("��]�Ԋu�i�b�j")]
    public float minInterval = 2f;
    public float maxInterval = 4f;

    [Header("��]���x�i0 �Ȃ�u���j")]
    public float rotateSpeed = 180f; // degrees per second�i0�Ȃ瑦���ɉ�]�j

    private Quaternion targetRotation;

    private void Start()
    {
        StartCoroutine(RotateRoutine());
    }

    private void Update()
    {
        // ����������
        if (rotateSpeed > 0f)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
        }
        else
        {
            transform.rotation = targetRotation;
        }
    }

    private IEnumerator RotateRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minInterval, maxInterval));

            // �����_����Y���p�x�Ɍ����i���E���L�����L�������銴���j
            float randomY = Random.Range(0f, 360f);
            targetRotation = Quaternion.Euler(0f, randomY, 0f);
        }
    }
}
