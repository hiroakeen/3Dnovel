using UnityEngine;

[RequireComponent(typeof(Animator))]
public class SimplePlayerAnimator : MonoBehaviour
{
    [Header("�ړ����x�̂������l�i����ȏ�Ȃ�����Ă�Ƃ݂Ȃ��j")]
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
        // ���ۂ̈ړ��������瑬�x���Z�o
        float moveSpeed = (transform.position - lastPosition).magnitude / Time.deltaTime;
        lastPosition = transform.position;

        // �ړ����Ă邩�ǂ����𔻒�
        bool isMoving = moveSpeed > moveThreshold;

        // Animator��bool��n���i"isMoving"�Ƃ����p�����[�^�[���g���j
        animator.SetBool("isMoving", isMoving);
    }
}
