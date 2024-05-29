using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpTrigger : MonoBehaviour
{
    private bool hasTriggered = false; // ����һ����־��ȷ��������ֻ����һ��

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasTriggered) // ����Ƿ�����Ҵ�������ȷ��֮ǰû�д�����
        {
            hasTriggered = true; // ���ô�����־Ϊ�棬��ֹ�ظ�����
            if (gameObject.CompareTag("HelpArea"))
            {
                AudioManager.Instance.PlaySFX("PriHelp");
                StartCoroutine(DestroyAfterDelay()); // �����ӳ����ٵ�Э��
            }
        }
    }

    IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(1f); // �ȴ�1��
        Destroy(gameObject); // ���������Ϸ����
    }
}
