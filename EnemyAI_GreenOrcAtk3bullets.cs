using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI_GreenOrcAtk3bullets : MonoBehaviour
{
    public float detectionRadius = 5f;
    public LayerMask playerLayer;
    public Transform firePoint;
    public GameObject bulletPrefab;
    public float bulletSpeed = 10f;
    public float shootCooldown = 2f; 
    private float lastShootTime = 0f;
    public bool isAttacking = false;
    private Animator animator;
    private EnermyAI_Orcmove moveScript;

    private void Start()
    {
        animator = GetComponent<Animator>();
        moveScript = GetComponent<EnermyAI_Orcmove>();
    }
    void Update()
    {
        if (Time.time - lastShootTime >= shootCooldown) 
        {
            DetectAndShootPlayer();
        }
    }

    void DetectAndShootPlayer()
    {
        Collider2D player = Physics2D.OverlapCircle(transform.position, detectionRadius, playerLayer);
        if (player != null)
        {
            moveScript.SetChasingPlayer(true, player.transform);
            if (Time.time - lastShootTime >= shootCooldown)
            {
                StartShooting(player.transform.position);
            }
        }
        else
        {
            moveScript.SetChasingPlayer(false, null);
            ResetAttackState();
        }
    }

    void StartShooting(Vector2 targetPosition)
    {
        lastShootTime = Time.time;
        isAttacking = true;
        animator.SetBool("ATK", true);
        StartCoroutine(ShootAfterDelay(targetPosition, 0.3f));
    }

    IEnumerator ShootAfterDelay(Vector2 targetPosition, float delay)
    {
        yield return new WaitForSeconds(delay);
        ShootAtPlayer(targetPosition);
        ResetAttackState();
    }
    void ResetAttackState()
    {
        isAttacking = false;
        animator.SetBool("ATK", false);
    }

    void ShootAtPlayer(Vector2 targetPosition)
    {
        float angleStep = 10f; 
        float startAngle = -angleStep; 

        for (int i = 0; i < 3; i++) 
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();

            Vector2 shootingDirection = (targetPosition - (Vector2)firePoint.position).normalized;
            shootingDirection = Quaternion.Euler(0, 0, startAngle + angleStep * i) * shootingDirection; 

            bulletRb.velocity = shootingDirection * bulletSpeed;
        }
    }

    Vector2 CalculateLaunchVelocity(Vector2 toTarget, Vector2 targetPosition)
    {
        float g = Physics2D.gravity.magnitude;
        float angle = 45f; 
        float speed = bulletSpeed; 
        float radianAngle = Mathf.Deg2Rad * angle;

        Vector2 velocity = new Vector2(
            Mathf.Cos(radianAngle) * speed,
            Mathf.Sin(radianAngle) * speed
        );

        velocity.x *= Mathf.Sign(toTarget.x);
        return velocity;
    }

    

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red; 
        Gizmos.DrawWireSphere(transform.position, detectionRadius); 
    }
    public void AttackVoice()
    {
        AudioManager.Instance.PlaySFX("EnemyAtk3");
    }
}
