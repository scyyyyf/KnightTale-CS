using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static EnemyHealth;

public class EnemyHealth : MonoBehaviour
{
    #region Variables
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject healthBarPrefab;
    public LayerMask GroundLayer;

    [Header("Enemy Health")]
    public float Hp = 50;
    public float BossRangeHp = 500;
    public float RedTime = 0.1f;
    public bool isInvulnerable = false;
    [SerializeField] private float ChangeColorTime;

    [Header("Health Bar Settings")]
    public float healthBarHeight = 0f;
    public Vector3 healthBarScale = new Vector3(1f, 1f, 1f);    
    private Coroutine hideHealthBarCoroutine;
    [SerializeField] private HealthBar healthBarInstance;

    [Header("Boob Effect")]
    public Transform PrefabBoom;
    private SpriteRenderer[] renders;
    public Vector3 explosionScale = Vector3.one;

    [Header("Knockback Effect")]
    public float knockbackStrength = 2f;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private EnermyAI_Orcmove moveScript;

    [Header("Item Drop")]
    public List<PotionDrop> potionDrops;
    
    [Header("Boss Death Event")]
    public UnityEvent onDeath;

    [Header("Score")]
    public int scoreValue = 10;

    public float originalSpeed;
    public bool isEnraged = false;

    [System.Serializable]
    public class PotionDrop
    {
        public GameObject potionPrefab;
        public float dropChance;
    }
    #endregion

    void Start()
    {
        // Get references
        rb = GetComponent<Rigidbody2D>();
        renders = GetComponentsInChildren<SpriteRenderer>();
        animator = GetComponent<Animator>();
        moveScript = GetComponent<EnermyAI_Orcmove>();

        // initialize enmey Health setting
        GameObject healthBarObj = Instantiate(healthBarPrefab, transform.position, Quaternion.identity, transform);
        float healthBarOffset = CalculateHealthBarOffset();
        healthBarObj.transform.localPosition = new Vector3(0, healthBarOffset, 0);
        healthBarObj.transform.localScale = healthBarScale;
        healthBarInstance = healthBarObj.GetComponentInChildren<HealthBar>();
        if (healthBarInstance != null)
            healthBarInstance.SetMaxHealth((int)Hp);
        healthBarInstance.gameObject.SetActive(false);
    }

    private void Update()
    {
        if(Time.time > ChangeColorTime)
        {
            SetColor(Color.white);
        }
    }

    private float CalculateHealthBarOffset()
    {
        Renderer renderer = GetComponentInChildren<Renderer>();
        if (renderer != null)
        {
            return renderer.bounds.extents.y + healthBarHeight;
        }
        return 0.5f; 
    }

    public void SetColor(Color color)
    {
        foreach (var r in renders)
        {
            r.color = color;
        }
    }

    public void TakeDamage(float damageAmount)
    {
        if (isInvulnerable)
            return;
        animator.SetTrigger("TakeDamage");
        SetColor(Color.red); 
        ChangeColorTime = Time.time + RedTime; 

        Hp -= damageAmount;
        AudioManager.Instance.PlaySFX("HitEnemy");
        if (healthBarInstance != null)
        {
            healthBarInstance.gameObject.SetActive(true);
            healthBarInstance.Sethealth((int)Hp);
        }
        if (hideHealthBarCoroutine != null)
        {
            StopCoroutine(hideHealthBarCoroutine);
        }
        hideHealthBarCoroutine = StartCoroutine(HideHealthBarDelay());

        if (Hp <= BossRangeHp)
        {
            isEnraged = true;
            GetComponent<Animator>().SetBool("IsEnraged", true);
        }
        if (Hp <= 0)
        {
            Die(); 
        }
        if (moveScript != null && originalSpeed == 0)
        {
            StartCoroutine(PauseMovementTemporarily()); 
        }
        if (playerTransform != null)
        {
            Vector2 attackDirection = transform.position - playerTransform.position;
            Vector2 knockback = new Vector2(attackDirection.x * knockbackStrength, rb.velocity.y);
            rb.velocity = knockback;
        }
    }

    private void Die()
    {
        onDeath.Invoke();
        if (PrefabBoom != null)
        {
            Transform explosionEffect = Instantiate(PrefabBoom, transform.position, Quaternion.identity);
            explosionEffect.localScale = explosionScale; 
        }
        DropPotion();
        isEnraged = false;
        if (ScoreManager.instance != null)
        {
            ScoreManager.instance.AddScore(scoreValue);
        }
        Destroy(gameObject);
    }

    private IEnumerator HideHealthBarDelay()
    {
        yield return new WaitForSeconds(3f); 
        if (healthBarInstance != null)
            healthBarInstance.gameObject.SetActive(false); 
    }

    private IEnumerator PauseMovementTemporarily()
    {
        originalSpeed = moveScript.Speed; 
        moveScript.Speed = 0; 
        yield return new WaitForSeconds(0.2f); 
        moveScript.Speed = originalSpeed;
        originalSpeed = 0;
    }

    private void DropPotion()
    {
        float offset = 1f;  
        float spacing = 1f; 
        int columnCount = 3; 
        int currentColumn = 0; 
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 10f, GroundLayer);
        Vector3 groundPosition = hit.collider ? new Vector3(transform.position.x, hit.point.y - 0.5f, transform.position.z) : transform.position;

        foreach (var potionDrop in potionDrops)
        {
            float roll = Random.Range(0, 100f);
            if (roll <= potionDrop.dropChance)
            {
                float columnOffset = (currentColumn - (columnCount - 1) / 2.0f) * spacing;
                Vector3 potionPosition = groundPosition + new Vector3(columnOffset, offset, 0);
                Instantiate(potionDrop.potionPrefab, potionPosition, Quaternion.identity);

                currentColumn = (currentColumn + 1) % columnCount; 
                if (currentColumn == 0)
                {
                    offset += spacing; 
                }
            }
        }
    }
    public void OrcDeadSound()
    {
        AudioManager.Instance.PlaySFX("OrcDie");
    }
    public void BatDeadSound()
    {
        AudioManager.Instance.PlaySFX("BatDie");
    }
    public void StopBossSound()
    {
        AudioManager.Instance.StopBossEnragedRunSound();
        AudioManager.Instance.StopBossRunSound(); 
    }
}

