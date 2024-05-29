using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Move : StateMachineBehaviour
{
    public float speed = 2.5f;
    public float attackRange = 3f;
    Transform player;
    Rigidbody2D rb;
    Boss_wolf_Move boss;
    EnemyHealth enemyHealth;
    private bool shouldPlayMoveSound = false;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = animator.GetComponent<Rigidbody2D>();
        boss = animator.GetComponent<Boss_wolf_Move>();
        enemyHealth = animator.GetComponent<EnemyHealth>();
        shouldPlayMoveSound = true;
        PlayMoveSound(animator);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss.LookAtPlayer();
        Vector2 target = new Vector2(player.position.x, rb.position.y);
        Vector2 newPos = Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);
        if(Vector2.Distance(player.position,rb.position) <= attackRange)
        {
            shouldPlayMoveSound = false; 
            AudioManager.Instance.StopBossEnragedRunSound();
            AudioManager.Instance.StopBossRunSound();
            animator.SetTrigger("Attack");
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Attack");
    }
    private void PlayMoveSound(Animator animator)
    {
        if (shouldPlayMoveSound)
        {
            if (enemyHealth.isEnraged == true)
                AudioManager.Instance.StartBossEnragedRunSound("BossEnrageMove");
            if (enemyHealth.isEnraged == false)
                AudioManager.Instance.StartBossRunSound("BossMove");
        }
    }
    public void PlayAttackSound()
    {
        AudioManager.Instance.PlaySFX("BossAttack1");
    }
}
