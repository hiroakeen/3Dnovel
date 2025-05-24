using UnityEngine;
using Unity.Cinemachine;

public class ConversationCameraSwitcher : MonoBehaviour
{
    [Header("Cinemachine Virtual Cameras")]
    [SerializeField] private CinemachineCamera normalCamera;
    [SerializeField] private CinemachineCamera conversationCamera;

    private void Start()
    {
        ActivateNormalCamera();
    }

    public void ActivateConversationCamera(Transform target)
    {
        if (conversationCamera != null)
        {
            conversationCamera.LookAt = target;
            conversationCamera.Follow = target;
            conversationCamera.Priority = 20;
        }

        if (normalCamera != null)
        {
            normalCamera.Priority = 10;
        }
    }

    public void ActivateNormalCamera()
    {
        if (conversationCamera != null)
        {
            conversationCamera.Priority = 10;
        }

        if (normalCamera != null)
        {
            normalCamera.Priority = 20;
        }
    }
}
