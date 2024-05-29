using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement_Kdx : MonoBehaviour
{
    public ParticleSystem Dust;
    #region Variables
    [Header("reference")]
    public Rigidbody2D rb;
    [SerializeField] private Transform leftFoot;
    [SerializeField] private Transform rightFoot;
    [SerializeField] private float groundCheckDistance = 0.2f;
    //[SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask goundLayer;
    [SerializeField] private TrailRenderer tr;
    [SerializeField] private Transform[] respawnPoints;
    [SerializeField] private GameoverMenu gameOverMenu;
    private PlayerAttack playerAttack;

    [Header("move")]
    public float horizontal;
    private bool isRunning = false;
    public float speed = 8f;
    public bool isFacingRight= true;
    public bool IsOnGround;
    public bool notMove = false;
    public bool IsOnPlatfrom = false;
    public Rigidbody2D CurrentPlatform;

    [Header("dash")]
    public bool canDash = true;
    [SerializeField] private bool isDashing;
    [SerializeField] private float dashingPower = 20f;
    [SerializeField] private float dashingTime = 0.3f;
    [SerializeField] private float dashingCooldown = 1f;
    [SerializeField] private bool isChargingComplete = false;
    [SerializeField] private float nextDashTime = 0f;
    public event Action onDash;

    [Header("jump & doublejump")]
    public bool doubleJump;
    public bool notJump = false;
    [SerializeField] private float jumpingPower = 16f;
    [SerializeField] private float doubleJumpingPower = 12f;
    [SerializeField] private float coyoteTime = 0.2f;
    [SerializeField] private float coyoteTimeCounter;


    [Header("animation")]
    public Animator animator;

    [Header("health")]
    public int maxHealth = 100;
    public int currentHealth;
    public HealthBar healthBar;
    public bool isDead = false;

    [Header("Invincibility")]
    public float invincibilityDurationSeconds = 1f;
    public float blinkIntervalSeconds = 0.1f;
    public bool isInvincible = false;
    public bool IsHit = false;
    public int CurrentKnockbackStrength;
    public int NormalKnockbackStrength = 5;
    public int MoveKnockbackStrength = 10;
    [Header("weapon")]
    public WeaponManager weaponManager;
    [Header("Princess")]
    public GameObject TransformedPrincess_1;
    public GameObject TransformedPrincess_2;
    [Header("CameraStuff")]
    [SerializeField] private GameObject _caremaFollowGO;
    private CaremaFollowObj _cameraFollowObject;
    private float _fallSpeedYDampingChangeThreshold;
    [Header("Scene Configuration")]
    public GameScene currentSceneType = GameScene.NormalGameplay;
    #endregion

    public enum GameScene
    {
        NormalGameplay,
        TrainMode
    }
    private void Start()
    {
        playerAttack = GetComponent<PlayerAttack>();
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (currentSceneIndex == 1|| currentSceneIndex == 3) 
        {
            currentHealth = maxHealth;
        }
        else
        {
            var saveSystem = FindObjectOfType<SaveSystem>(); 
            if (saveSystem != null && saveSystem.LoadedData != null)
            {
                currentHealth = saveSystem.LoadedData.playerHealth;
            }
            else
            {
                currentHealth = maxHealth; 
            }
        }
        if (currentSceneIndex == 3)
        {
            playerAttack.isChargeAttackUnlocked = true;
        }
        healthBar.SetMaxHealth(maxHealth);
        healthBar.Sethealth(currentHealth); 
        CurrentKnockbackStrength = NormalKnockbackStrength;
        weaponManager = GetComponent<WeaponManager>();
        gameOverMenu = FindObjectOfType<GameoverMenu>();
        _cameraFollowObject = _caremaFollowGO.GetComponent<CaremaFollowObj>();
        _fallSpeedYDampingChangeThreshold = CameraManager.instance._fallSpeedYDampingChangeThreshold;
    }

    public void Update()
    {
        if (playerAttack.isCharging)
        {
            speed = 4f;  
            jumpingPower = 10f;  
            canDash = false;
            isChargingComplete = false;
            notMove = true;
        }
        else if (playerAttack.isChargingOver && !isChargingComplete)
        {
            notMove = false;
            isChargingComplete = true;  // 蓄力攻击完成
            if (!isDashing && Time.time >= nextDashTime)  // 检查是否处于冲刺冷却期外
            {
                canDash = true;  // 蓄力攻击完成后，如果不在冷却期，则允许冲刺
            }
        }
        else if (!playerAttack.isCharging && !playerAttack.isChargingOver)
        {
            // 确保在普通状态下速度和跳跃力正常
            speed = 8f;
            jumpingPower = 16f;
        }
        IsOnGround = IsGrounded();

        if (isDashing)
        {
            return;
        }
        if (notMove)
        {
            horizontal = 0;

            return;
        }
        horizontal = Input.GetAxis("Horizontal");

        CurrentKnockbackStrength = Input.GetAxis("Horizontal") == 0 ? NormalKnockbackStrength:MoveKnockbackStrength;

        animator.SetFloat("walkingSpeed", Mathf.Abs(horizontal));

        if (horizontal != 0 && IsOnGround && !isRunning)
        {
            AudioManager.Instance.StartRunSound("Run");
            isRunning = true;
        }
        else if (horizontal == 0 || !IsOnGround)
        {
            if (isRunning)
            {
                AudioManager.Instance.StopRunSound();
                isRunning = false;
            }
        }

        if (!isDashing && horizontal == 0)
        {
            tr.emitting = false;
        }
        if (!isDashing && IsOnGround)
        {
            tr.emitting = false;
        }
        if (IsGrounded())
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }
        if (IsOnGround && !Input.GetButton("Jump"))
        {
            doubleJump = false;
        }

        if (IsOnGround == true)
        {
            animator.SetBool("isFalling", false);
            animator.SetBool("isJumping", false);
        }
        if (Input.GetButtonDown("Jump") && !notJump)
        {
            CreatDust();
            if (coyoteTimeCounter > 0f || doubleJump)
            {
                animator.SetBool("isFalling", false);
                animator.SetBool("isJumping", true);

                rb.velocity = new Vector2(rb.velocity.x, doubleJump ? doubleJumpingPower : jumpingPower);
                doubleJump = !doubleJump;
                AudioManager.Instance.PlaySFX("Jump");
            }
        }
        if (rb.velocity.y < 0f && !IsOnGround)
        {
            animator.SetBool("isFalling", true);
            animator.SetBool("isJumping", true);
        }

        if (Input.GetButtonUp("Jump")&&rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x,rb.velocity.y * 0.5f);
            coyoteTimeCounter = 0f;
        }

        if ((Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.L)) && canDash)
        {
            AudioManager.Instance.PlaySFX("Dash");
            StartCoroutine(Dash());
        }

        Flip();

        if (rb.velocity.y < _fallSpeedYDampingChangeThreshold && !CameraManager.instance.IsLerpingYDamping && !CameraManager.instance.LerpingFromPlayerFalling)
        {
            CameraManager.instance.LerpYDamping(true);
        }
        if(rb.velocity.y >= 0f && CameraManager.instance.IsLerpingYDamping && CameraManager.instance.LerpingFromPlayerFalling)
        {
            CameraManager.instance.LerpingFromPlayerFalling = false;
            CameraManager.instance.LerpYDamping(false);
        }
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }
        if(!IsHit)
        {
            if (IsOnPlatfrom && CurrentPlatform != null)
            {
                rb.velocity = new Vector2(horizontal * speed, rb.velocity.y) + CurrentPlatform.velocity * Vector3.right;
            }       
            else
            {
                rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
            }
        }
    }

    private bool IsGrounded()
    {
    RaycastHit2D hitLeft = Physics2D.Raycast(leftFoot.position, Vector2.down, groundCheckDistance, goundLayer);
    RaycastHit2D hitRight = Physics2D.Raycast(rightFoot.position, Vector2.down, groundCheckDistance, goundLayer);

    // 绘制射线在编辑器中查看（可选）
    Debug.DrawRay(leftFoot.position, Vector2.down * groundCheckDistance, Color.green);
    Debug.DrawRay(rightFoot.position, Vector2.down * groundCheckDistance, Color.green);

    return hitLeft.collider != null || hitRight.collider != null;
}

    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            transform.Rotate(0f, 180f, 0f);
            CreatDust();
            _cameraFollowObject.CallTurn();
        }
        
    }

    public void TakeDamage(int damage)
    {
        if (isInvincible) return;
        IsHit = true;
        currentHealth -= damage;
        healthBar.Sethealth(currentHealth);
        if (currentHealth <= 0 && !isDead)
        {
            Die();  // 将死亡逻辑封装在Die方法中
        }
        float direction = isFacingRight ? -1f : 1f;
        rb.velocity += new Vector2(direction * CurrentKnockbackStrength, rb.velocity.y);
        int originalLayer = gameObject.layer;
        gameObject.layer = LayerMask.NameToLayer("PlayerInvincible");
        StartCoroutine(Knockback(originalLayer));
        StartCoroutine(BecomeInvincible(originalLayer));
        if (currentHealth <= 0)
        {
            if (gameOverMenu != null)
            {
                gameOverMenu.ShowGameOverScreen();
            }
            else
            {
                // 尝试获取ScoreboardMenu
                ScoreResultMenu scoreboardMenu = FindObjectOfType<ScoreResultMenu>();
                if (scoreboardMenu != null)
                {
                    scoreboardMenu.ShowGameOverScreen();
                }
                else
                {
                    Debug.LogWarning("No suitable menu found for ending the game.");
                }
            }
            animator.SetTrigger("Death");
            rb.velocity = Vector2.zero;
            this.enabled = false;
            isDead = true;
        }
        if (UnityEngine.Random.Range(0, 10) < 2)
        {
            DecreaseRandomPotionLevel();
        }
    }
    
    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Min(currentHealth, maxHealth); 
        healthBar.Sethealth(currentHealth); 
    }

    private IEnumerator Knockback(int originalLayer)
    {
        yield return new WaitForSeconds(0.2f);
        IsHit = false;
        //rb.isKinematic = false;
        rb.velocity = Vector2.zero;

        gameObject.layer = originalLayer;
    }

    private IEnumerator BecomeInvincible(int originalLayer)
    {
        isInvincible = true;
        for (float i = 0; i < invincibilityDurationSeconds; i += blinkIntervalSeconds)
        {
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f); 
            yield return new WaitForSeconds(blinkIntervalSeconds / 2);
            GetComponent<SpriteRenderer>().color = Color.white; 
            yield return new WaitForSeconds(blinkIntervalSeconds / 2);
        }
        isInvincible = false;
        gameObject.layer = originalLayer;
    }

    private IEnumerator Dash()
    {
        if (canDash)  
        {
            canDash = false;
            onDash?.Invoke();
            isDashing = true;
            isInvincible = true;
            float dashEndTime = Time.time + dashingTime;
            nextDashTime = dashEndTime + dashingCooldown;
            animator.SetBool("isDashing", true);
            CreatDust();
            float originalGravity = rb.gravityScale;
            rb.gravityScale = 0.5f;  
            rb.velocity = new Vector2(isFacingRight ? dashingPower : -dashingPower, 0f);  
            tr.emitting = true;  

            int originalLayer = gameObject.layer;
            gameObject.layer = LayerMask.NameToLayer("PlayerInvincible");

            GetComponent<SpriteRenderer>().color = Color.blue;
            yield return new WaitForSeconds(0.1f);  
            GetComponent<SpriteRenderer>().color = Color.white;
            gameObject.layer = originalLayer;
            yield return new WaitForSeconds(dashingTime - 0.1f);
            isInvincible = false;
            rb.gravityScale = originalGravity;
            isDashing = false;
            animator.SetBool("isDashing", false);

            yield return new WaitForSeconds(dashingCooldown);
            if (!playerAttack.isCharging && Time.time >= nextDashTime)
            {
                canDash = true;
            }
        }
    }

    private void DashVersion2()
    {
        canDash = false;
        isDashing = true;
        animator.SetBool("isDashing", true);
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0.5f;
        rb.velocity = new Vector2(isFacingRight ? dashingPower : -dashingPower, 0f);
        tr.emitting = true;
        StartCoroutine(DashAnimationCoroutine(() =>
        {
            animator.SetBool("isDashing", false);
            rb.gravityScale = originalGravity;
            isDashing = false;
            canDash = true;
        }));
    }

    private IEnumerator DashAnimationCoroutine(Action OnAnimationComplete)
    {
        yield return new WaitForSeconds(dashingTime);
        OnAnimationComplete?.Invoke();
    }

    void CreatDust()
    {
        Dust.Play();
    }
    
    public void SetWalkingSpeed(float speed)
    {
        animator.SetFloat("walkingSpeed", speed);
    }
    
    public void SetNotJump(bool jumpStatus)
    {
        animator.SetBool("isFalling", jumpStatus);
        animator.SetBool("isJumping", jumpStatus);
    }
    
    private void DecreaseRandomPotionLevel()
    {
        int potionToDecrease = UnityEngine.Random.Range(0, 2); 
        switch (potionToDecrease)
        {
            case 0:
                weaponManager.DecreaseAttackDamageLevel();
                UpdateSkillBar(SkillType.AttackPower, weaponManager.GetAttackLevel());
                break;
            case 1:
                weaponManager.DecreaseAttackRateLevel();
                UpdateSkillBar(SkillType.AttackSpeed, weaponManager.GetAttackSpeedLevel());
                break;
            case 2:
                weaponManager.DecreaseBulletLifetimeLevel();
                UpdateSkillBar(SkillType.AttackRange, weaponManager.GetAttackRangeLevel());
                break;
        }
    }
    
    private void UpdateSkillBar(SkillType skillType, int level)
    {
        foreach (var skillBar in FindObjectsOfType<SkillBar>())
        {
            skillBar.UpdateSkillLevel(skillType, level);
        }
    }
    
    public void RespawnAtNearestPoint()
    {
        if (isDead) 
        {
            return;
        }

        if (respawnPoints.Length == 0)
        {
            return;
        }
        Transform nearestRespawnPoint = FindNearestRespawnPoint();
        Revive(nearestRespawnPoint.position);
    }

    public Transform FindNearestRespawnPoint()
    {
        Transform nearest = null;
        float minDistance = Mathf.Infinity;
        foreach (Transform point in respawnPoints)
        {
            float distance = Vector3.Distance(transform.position, point.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearest = point;
            }
        }
        return nearest;
    }
    
    public void Revive(Vector3 respawnPosition)
    {
        healthBar.Sethealth(currentHealth);
        rb.velocity = Vector2.zero;
        transform.position = respawnPosition;
        animator.ResetTrigger("Death");
        isInvincible = false;
        gameObject.layer = LayerMask.NameToLayer("Player");
        DisappearingPlatform[] platforms = FindObjectsOfType<DisappearingPlatform>();
    }
    private void Die()
    {
        isDead = true;
        animator.SetTrigger("Death");
        rb.velocity = Vector2.zero;
        this.enabled = false;

        switch (currentSceneType)
        {
            case GameScene.NormalGameplay:
                gameOverMenu.ShowGameOverScreen();
                break;
            case GameScene.TrainMode:
                // 假设有一个 ScoreboardMenu 类负责处理计分板逻辑
                FindObjectOfType<ScoreResultMenu>().ShowGameOverScreen();
                break;
        }
    }
}
