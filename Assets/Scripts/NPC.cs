using UnityEngine;

public class NPC : MonoBehaviour
{
    public void ReceiveMemory(MemoryData_SO memory)
    {
        // �L���������𖞂����Ă��邩���肵�ĕ���Ȃ�
        if (memory.memoryId == expectedMemoryId)
        {
            Debug.Log("�������L�����󂯎�����I");
            // �D���x�A�b�v�E�G���f�B���O�t���O etc.
        }
        else
        {
            Debug.Log("�����ĂȂ��L���������c");
            // �������E�딽���Ȃ�
        }
    }

    public string expectedMemoryId; // �����Ƃ��Ďg���L��ID
}
