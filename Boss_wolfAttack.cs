using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_wolfAttack : MonoBehaviour
{
    public int attackDamage = 15;
    public int enragedAttackDamage = 30;
    public float attackRange = 1f;
    public float additionalAttackRange = 1f; 
    public LayerMask attackMask;
    public Vector3 attackOffset;
    public Vector3 additionalAttackOffset;
    public GameObject batPrefab; 
    public GameObject fireballPrefab;
    public Transform summonPoint;
    public Transform playerTransform;
    public GameObject teleportAnimationPrefab;

    private void Start()
    {
        InvokeRepeating("SummonBat", 0f, 10f);
    }
    
    public void Attack()
    {
        PerformAttack(attackOffset, attackRange, attackDamage);
        PerformAttack(additionalAttackOffset, additionalAttackRange, attackDamage); 
    }

    public void EnragedAttack()
    {
        PerformAttack(attackOffset, attackRange, enragedAttackDamage);
        PerformAttack(additionalAttackOffset, additionalAttackRange, enragedAttackDamage);
        StartCoroutine(FireFireballsAbovePlayer());
    }

    private void SummonBat()
    {
        if (batPrefab && summonPoint)
        {
            Instantiate(batPrefab, summonPoint.position, Quaternion.identity);
            AudioManager.Instance.PlaySFX("BatSummon");
        }
    }
    private IEnumerator FireFireballsAbovePlayer()
    {
        AudioManager.Instance.PlaySFX("FireBallBorn");
        Vector3 spawnPosition = playerTransform.position + Vector3.up * 5; 
        for (int i = 0; i < 3; i++)
        {
            Vector3 fireballSpawnPosition = spawnPosition + Vector3.right * i * 2; 

            GameObject teleportAnimation = Instantiate(teleportAnimationPrefab, fireballSpawnPosition, Quaternion.identity);
            Animator anim = teleportAnimation.GetComponent<Animator>();
            if (anim != null)
            {
                yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length); 
            }
            Destroy(teleportAnimation); 
            Instantiate(fireballPrefab, fireballSpawnPosition, Quaternion.identity); 
            yield return new WaitForSeconds(0.2f); 
        }
    }

    private void PerformAttack(Vector3 offset, float range, int damage)
    {
        Vector3 pos = transform.position;
        pos += transform.right * offset.x;
        pos += transform.up * offset.y;

        Collider2D colInfo = Physics2D.OverlapCircle(pos, range, attackMask);
        if (colInfo != null)
        {
            colInfo.GetComponent<PlayerMovement_Kdx>().TakeDamage(damage);
        }
    }

    void OnDrawGizmosSelected()
    {
        DrawAttackGizmo(attackOffset, attackRange);
        DrawAttackGizmo(additionalAttackOffset, additionalAttackRange);
    }

    private void DrawAttackGizmo(Vector3 offset, float range)
    {
        Vector3 pos = transform.position;
        pos += transform.right * offset.x;
        pos += transform.up * offset.y;

        Gizmos.DrawWireSphere(pos, range);
    }
    public void BossDeadSound()
    {
        AudioManager.Instance.PlaySFX("BossDie");
    }
}
