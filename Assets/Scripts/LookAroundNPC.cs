using UnityEngine;
using System.Collections;

public class LookAroundNPC : MonoBehaviour
{
    [Header("回転間隔（秒）")]
    public float minInterval = 2f;
    public float maxInterval = 4f;

    [Header("回転速度（0 なら瞬時）")]
    public float rotateSpeed = 180f; // degrees per second（0なら即座に回転）

    private Quaternion targetRotation;

    private void Start()
    {
        StartCoroutine(RotateRoutine());
    }

    private void Update()
    {
        // ゆっくり向く
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

            // ランダムなY軸角度に向く（左右をキョロキョロする感じ）
            float randomY = Random.Range(0f, 360f);
            targetRotation = Quaternion.Euler(0f, randomY, 0f);
        }
    }
}
