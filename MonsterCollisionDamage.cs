using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterCollisionDamage : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerMovement_Kdx player = collision.GetComponent<PlayerMovement_Kdx>();
            if (player != null && !player.isInvincible)
            {
                player.TakeDamage(10);
                EnemyAI_BatAtk batAttack = GetComponentInParent<EnemyAI_BatAtk>();
                if (batAttack != null)
                {
                    batAttack.isCharging = false; 
                    batAttack.isReturning = true; 
                }
            }
        }
    }
}
