using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI_PurpleOrcAtk : MonoBehaviour
{
    public float detectionRadius = 2f; 
    public LayerMask playerLayer;
    public float attackCooldown = 1f; 
    private float lastAttackTime = 0f;
    public bool isAttacking = false;
    private Animator animator;
    private EnermyAI_Orcmove moveScript;

    private void Start()
    {
        animator = GetComponent<Animator>();
        moveScript = GetComponent<EnermyAI_Orcmove>();
    }

    private void Update()
    {
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            DetectAndAttackPlayer();
        }
    }

    void DetectAndAttackPlayer()
    {
        Collider2D player = Physics2D.OverlapCircle(transform.position, detectionRadius, playerLayer);
        if (player != null)
        {
            if (Time.time - lastAttackTime >= attackCooldown)
            {
                lastAttackTime = Time.time;
                isAttacking = true;
                animator.SetTrigger("ATK");
                moveScript.SetChasingPlayer(true, player.transform);
                moveScript.enemyBehaviour = EnermyAI_Orcmove.EnemyBehaviour.IsChasing;
            }
        }
        else
        {
            if (isAttacking)
            {
                moveScript.SetChasingPlayer(false);
                moveScript.enemyBehaviour = EnermyAI_Orcmove.EnemyBehaviour.IsPatrolling;
                isAttacking = false;
            }
        }
    }

    public void OnAttack()
    {
        isAttacking = true;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }

    public void AttackVoice()
    {
        AudioManager.Instance.PlaySFX("EnemyAtk4");
    }
}
