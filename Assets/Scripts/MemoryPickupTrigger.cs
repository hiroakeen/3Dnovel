using UnityEngine;

public class MemoryPickupTrigger : MonoBehaviour
{
    [SerializeField] private MemoryData memoryData;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            MemoryManager.Instance.AddMemory(memoryData);
            // ˆê“xæ“¾‚µ‚½‚ç”ñ•\¦‚âíœ
            gameObject.SetActive(false);
        }
    }
}
