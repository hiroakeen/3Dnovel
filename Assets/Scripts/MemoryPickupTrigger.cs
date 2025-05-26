using UnityEngine;

public class MemoryPickupTrigger : MonoBehaviour
{
    [SerializeField] private MemoryData memoryData;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            MemoryManager.Instance.AddMemory(memoryData);
            // ��x�擾�������\����폜
            gameObject.SetActive(false);
        }
    }
}
