using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpTrigger : MonoBehaviour
{
    private bool hasTriggered = false; // 增加一个标志以确保触发器只触发一次

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasTriggered) // 检查是否是玩家触发，并确保之前没有触发过
        {
            hasTriggered = true; // 设置触发标志为真，防止重复触发
            if (gameObject.CompareTag("HelpArea"))
            {
                AudioManager.Instance.PlaySFX("PriHelp");
                StartCoroutine(DestroyAfterDelay()); // 启动延迟销毁的协程
            }
        }
    }

    IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(1f); // 等待1秒
        Destroy(gameObject); // 销毁这个游戏对象
    }
}
