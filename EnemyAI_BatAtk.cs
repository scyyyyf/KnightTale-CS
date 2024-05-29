using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI_BatAtk : MonoBehaviour
{
    [Header("References")]
    private Animator animator;
    private Transform playerTransform;
    public EnermyAI_Orcmove enermyAI_Orcmove;
    public float detectionRadius = 5f;
    public GameObject bulletPrefab;
    public LayerMask playerLayer;
    public bool isAttacking = false;

    [Header("Shooting")]
    public Transform firePoint;
    public float bulletSpeed = 10f;
    public float shootCooldown = 2f;
    private float lastShootTime = 0f;
    public int attackCounter = 0;

    [Header("Charging")]
    public int attacksBeforeCharge = 2; 
    public float chargeSpeed = 20f;
    public bool isCharging = false;
    public bool isReturning = false;
    private Vector2 chargeTargetPosition;
    private void Start()
    {
        animator = GetComponent<Animator>();
        enermyAI_Orcmove = GetComponentInParent<EnermyAI_Orcmove>();
    }

    void Update()
    {
        if (isCharging || isAttacking)
        {
            FaceTarget(playerTransform.position);
        }
        if (isCharging)
        {
            ChargeTowardsPlayer();
        }
        else if (isReturning)
        {
            ReturnToPatrol();
        }
        if (Time.time - lastShootTime >= shootCooldown && !isCharging)
        {
            DetectAndShootPlayer();
        }
    }

    public void DetectAndShootPlayer()
    {
        Collider2D player = Physics2D.OverlapCircle(transform.position, detectionRadius, playerLayer);
        if (player != null)
        {
            playerTransform = player.transform;
            Vector2 targetPosition = player.transform.position;
            lastShootTime = Time.time;
            isAttacking = true;

            if (attackCounter < attacksBeforeCharge)
            {
                StartCoroutine(ShootAfterDelay(targetPosition, 0.3f));
            }
            else
            {
                StartCharge();
            }
        }
    }

    IEnumerator ShootAfterDelay(Vector2 targetPosition, float delay)
    {
        yield return new WaitForSeconds(delay);
        ShootAtPlayer(targetPosition);
        attackCounter++; 
        ResetAttackState();
    }

    public void ResetAttackState()
    {
        isAttacking = false;
    }

    public void ShootAtPlayer(Vector2 targetPosition)
    {
        AudioManager.Instance.PlaySFX("enermyattack5");
        float angleStep = 0f;
        float startAngle = -angleStep;

        for (int i = 0; i < 1; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
            Vector2 shootingDirection = (targetPosition - (Vector2)firePoint.position).normalized;
            shootingDirection = Quaternion.Euler(0, 0, startAngle + angleStep * i) * shootingDirection;
            bulletRb.velocity = shootingDirection * bulletSpeed;
        }
    }

    public void StartCharge()
    {
        AudioManager.Instance.PlaySFX("enermyattack6");
        isCharging = true;
        attackCounter = 0;
        chargeTargetPosition = playerTransform.position;
    }

    public void ChargeTowardsPlayer()
    {
        if (Vector3.Distance(transform.position, chargeTargetPosition) > 0.05f)
        {
            transform.position = Vector2.MoveTowards(transform.position, chargeTargetPosition, chargeSpeed * Time.deltaTime);
        }
        else
        {
            EndCharge();
        }
    }

    public void EndCharge()
    {
        isCharging = false;
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

    public void ReturnToPatrol()
    {
        Transform closestPatrolPoint = enermyAI_Orcmove.GetClosestPatrolPoint();
        if (closestPatrolPoint != null && Vector3.Distance(transform.position, closestPatrolPoint.position) > 0.05f)
        {
            transform.position = Vector2.MoveTowards(transform.position, closestPatrolPoint.position, chargeSpeed * Time.deltaTime);
        }
        else
        {
            isCharging = false;
            isReturning = false;
            attackCounter = 0;
            enermyAI_Orcmove.SetChasingPlayer(false);
        }
    }

    public void FaceTarget(Vector2 targetPosition)
    {
        bool targetIsToLeft = targetPosition.x < transform.position.x;
        bool needToFlip = (targetIsToLeft && transform.localScale.x < 0) || (!targetIsToLeft && transform.localScale.x > 0);
        if (needToFlip)
        {
            Flip();
        }
    }

    public void Flip()
    {
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
