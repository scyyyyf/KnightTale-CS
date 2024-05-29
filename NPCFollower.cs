using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCFollower : MonoBehaviour
{
    [Header("References")]
    public Transform player; 
    public Animator animator;
    [Header("ToBeContinued Trigger")]
    public bool canTriggerToBeContinued = false; 
    private float timeSinceSpawned = 0f;
    private bool toBeContinuedShown = false;
    [Header("following")]
    public float followDistance = 2f;
    public float followSpeed = 2f; 
    [Header("shooting")]
    public GameObject bulletPrefab; 
    
    public Transform shootPoint; 
    public LayerMask enemyLayer; 
    public float attackRange = 1f; 
    public float attackRate = 2f; 
    private float nextAttackTime = 0f;
    

    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        FollowPlayer();
        AutoAttack();
        TurnTowardsPlayer();
        if (canTriggerToBeContinued && !toBeContinuedShown)
        {
            timeSinceSpawned += Time.deltaTime;
            if (timeSinceSpawned >= 2f)
            {
                ShowToBeContinued();
                toBeContinuedShown = true;
            }
        }
    }

    void FollowPlayer()
    {
        Vector3 targetPosition = player.position - (Vector3.right * followDistance);
        
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
    }

    void AutoAttack()
    {
        if (Time.time >= nextAttackTime)
        {
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, attackRange, enemyLayer);
            if (hitEnemies.Length > 0)
            {
                animator.SetTrigger("Atk_Anna");
                Shoot(hitEnemies[0].transform.position);
                nextAttackTime = Time.time + 1f / attackRate;
            }
            else
            {
                animator.SetTrigger("Idle_Anna");
            }
        }
    }

    void Shoot(Vector2 target)
    {
        GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
        Vector2 direction = (target - (Vector2)shootPoint.position).normalized;
        bullet.GetComponent<Rigidbody2D>().velocity = direction * bullet.GetComponent<Bullets_Anna>().speed;
        AudioManager.Instance.PlaySFX("FollowerAtk");
    }

    void TurnTowardsPlayer()
    {
        if (player.position.x > transform.position.x)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }
    void ShowToBeContinued()
    {
        ToBeContinuedMenu toBeContinuedMenu = FindObjectOfType<ToBeContinuedMenu>();
        if (toBeContinuedMenu != null)
        {
            toBeContinuedMenu.ShowToBeContinuedScreen();
        }
    }
    public void FairyPopUpSound()
    {
        AudioManager.Instance.PlaySFX("FairyPopUpSound");
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
