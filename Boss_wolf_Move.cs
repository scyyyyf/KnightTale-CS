using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_wolf_Move : MonoBehaviour
{
    public Transform player;
    public bool isFlipped = false;
    public Animator animator;
    EnemyHealth enemyHealth;
    
    private void Start()
    {
        enemyHealth = animator.GetComponent<EnemyHealth>();
    }

    public void LookAtPlayer()
    {
        Vector3 flipped = transform.localScale;
        flipped.z *= -1f;

        if(transform.position.x > player.position.x && isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = false;
        }
        else if(transform.position.x < player.position.x && !isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = true;
        }
    }
    public void PlayAttackSound()
    {
        AudioManager.Instance.PlaySFX("BossAttack1");
    }
    public void PlayEnrageSound()
    {
        AudioManager.Instance.PlaySFX("BossEnrage");
    }
}
