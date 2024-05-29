using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerAttack : MonoBehaviour
{
    
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRange = 0.5f;
    
    [SerializeField] private float nextAttackTime = 0;
    [SerializeField] private LayerMask enemylayers;

    [SerializeField] private Transform shootPoint;
    [SerializeField] private GameObject swordEnergyPrefab;
    [SerializeField] private GameObject chargedAtkPrefab;
    [SerializeField] private GameObject PrincessFollower;
    [SerializeField] private Animator animator;
    [SerializeField] private Animator princessAnimator;
    

    [Header("charge")]
    [SerializeField] private bool ZHeld;
    public float ChargingTime = 0f;
    public float MinChargeTime = 1f;
    public float MaxChargeTime = 2f;
    public float chargeAttackCooldown = 2f;
    public bool isChargeAttackUnlocked = false;
    public bool isCharging = false;
    public bool isChargingOver = false;

    public WeaponManager weaponManager;
    public PlayerMovement_Kdx playerMovement;
    public GameObject debuff;
    
    private void Start()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        var saveSystem = FindObjectOfType<SaveSystem>();

        // 在第一关时，确保 isChargeAttackUnlocked 为 false
        if (currentSceneIndex == 1)
        {
            isChargeAttackUnlocked = false;
        }
        else
        {
            // 在其他关卡加载保存的状态
            if (saveSystem != null && saveSystem.LoadedData != null)
            {
                isChargeAttackUnlocked = saveSystem.LoadedData.isChargeAttackUnlocked;
            }
            else
            {
                // 如果没有保存的数据，保持默认值（或设置为 false）
                isChargeAttackUnlocked = false;
            }
        }
        playerMovement = GetComponent<PlayerMovement_Kdx>();
        weaponManager = GetComponent<WeaponManager>();
    }

    private void Update()
    {
        {
            ZHeld = false;
            ZHeld = ZHeld || Input.GetKey(KeyCode.J);

            if (Time.time >= nextAttackTime)
            {
                if (ZHeld)
                {
                    ChargingTime += Time.deltaTime;
                }
                if (ZHeld && ChargingTime < MaxChargeTime && ChargingTime > MinChargeTime && isChargeAttackUnlocked)
                {
                    isCharging = true;
                    animator.SetBool("isCharging", true);
                    AudioManager.Instance.StartChargingSound("Charging");
                    Debug.Log("isCharging");
                }
                if (ZHeld && ChargingTime > MaxChargeTime && isChargeAttackUnlocked)
                {
                    isCharging = false;
                    isChargingOver = true;
                    animator.SetBool("isChargingOver", true);
                    AudioManager.Instance.StopChargingSound();
                    AudioManager.Instance.StartChargeOverSound("ChargeOver");
                    animator.SetBool("isCharging", false);
                }
                else if (!ZHeld && (ChargingTime > 0 && ChargingTime < MinChargeTime))
                {
                    Attack();
                    AudioManager.Instance.StopChargingSound();
                    AudioManager.Instance.StopChargeOverSound();
                    Debug.Log("normal Attack");
                    ChargingTime = 0f;
                    isCharging = false;
                    isChargingOver = false;
                }
                else if (!ZHeld && ChargingTime > MaxChargeTime && !isChargeAttackUnlocked)
                {
                    Attack();
                    Debug.Log("normal Attack");
                    AudioManager.Instance.StopChargingSound();
                    AudioManager.Instance.StopChargeOverSound();
                    ChargingTime = 0f;
                    isCharging = false;
                    isChargingOver = false;
                }
                else if (!ZHeld && ChargingTime > MaxChargeTime && isChargeAttackUnlocked)
                {
                    if (playerMovement.currentHealth > 8)
                    {
                        AudioManager.Instance.StopChargingSound();
                        AudioManager.Instance.StopChargeOverSound();
                        FireChargedAtk();
                        Debug.Log("Charged Attack");
                        playerMovement.TakeDamage(5);
                        ChargingTime = 0f;
                        isCharging = false;
                        isChargingOver = false;
                    }
                    else
                    {
                        Attack();
                        Debug.Log("normal Attack");
                        AudioManager.Instance.StopChargingSound();
                        AudioManager.Instance.StopChargeOverSound();
                        ChargingTime = 0f;
                        isCharging = false;
                        isChargingOver = false;
                    }

                }
                else if (!ZHeld && ChargingTime < MaxChargeTime && ChargingTime > MinChargeTime)
                {
                    AudioManager.Instance.StopChargingSound();
                    AudioManager.Instance.StopChargeOverSound();
                    Attack();
                    Debug.Log("normal Attack");
                    animator.SetBool("isCharging", false);
                    ChargingTime = 0f;
                    isCharging = false;
                    isChargingOver = false;
                    playerMovement.canDash = true;
                }
            }
        }
    }
    void Attack()
    {
        playerMovement.notMove = true;
        animator.SetTrigger("Atk");
        AudioManager.Instance.PlaySFX("NormalAtk");
        Collider2D[] hitenemies = Physics2D.OverlapCircleAll(attackPoint.position,attackRange,enemylayers);
        FireProjectile();
        StartCoroutine(canMoveAfterDelay(0.2f));
        foreach(Collider2D enemy in hitenemies)
        {
            enemy.GetComponent<EnemyHealth>().TakeDamage(weaponManager.attackDamage);
        }
        nextAttackTime = Time.time + 1f / weaponManager.attackRate;
        animator.SetBool("isChargingOver", false);
    }

    IEnumerator canMoveAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        playerMovement.notMove = false; 
    }
    void FireChargedAtk()
    {
        animator.SetTrigger("Atk");
        AudioManager.Instance.PlaySFX("ChargedAtk");
        Instantiate(chargedAtkPrefab, shootPoint.position, shootPoint.rotation);
        princessAnimator.SetTrigger("Disappear");
        debuff.SetActive(true);
        StartCoroutine(DisablePrincessAfterAnimation());
        nextAttackTime = Time.time + chargeAttackCooldown;
        animator.SetBool("isChargingOver", false);
    }
    IEnumerator DisablePrincessAfterAnimation()
    {
        yield return new WaitForSeconds(princessAnimator.GetCurrentAnimatorStateInfo(0).length);  
        PrincessFollower.SetActive(false);  
        StartCoroutine(ReenableChargedAttack());
    }
    IEnumerator ReenableChargedAttack()
    {
        yield return new WaitForSeconds(chargeAttackCooldown);
        PrincessFollower.SetActive(true);  
        princessAnimator.SetTrigger("Appear");
        debuff.SetActive(false);
    }

    private void FireProjectile()
    {
        GameObject bullet = Instantiate(swordEnergyPrefab, shootPoint.position, shootPoint.rotation);
        swordEnergy bulletScript = bullet.GetComponent<swordEnergy>();
        if (bulletScript != null)
        {
            bulletScript.Initialize(weaponManager.attackDamage, weaponManager.bulletLifetime);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}


