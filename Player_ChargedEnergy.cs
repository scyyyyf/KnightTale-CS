using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_ChargedEnergy : MonoBehaviour
{
    [SerializeField] private float swordEnergySpeed = 20f;
    [SerializeField] private Rigidbody2D swordEnergyRb;
    [SerializeField] private GameObject explodeEffect;
    private int enemiesHit = 0;
    private HashSet<Collider2D> hitEnemies = new HashSet<Collider2D>();
    public Transform shootPoint;
    private void Awake()
    {
        shootPoint = GameObject.Find("ShootPoint").transform;
    }
    
    void Start()
    {
        swordEnergyRb.velocity = transform.right * swordEnergySpeed;
    }
    
    public void Initialize(float damage, float lifetime)
    {
        StartCoroutine(Delay(lifetime));
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"Collision with {collision.tag} at {collision.transform.position}");
        if (collision.tag == "CameraBoundary")
        {
            return;
        }
        if (collision.CompareTag("Enemy") && !hitEnemies.Contains(collision))
        {
            float damage = CalculateDamage(enemiesHit);
            var enemyHealth = collision.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
                InstantiateExplodeEffectAtCollisionPoint(collision);
                enemiesHit++;
                hitEnemies.Add(collision);
            }
        }
        if (collision.CompareTag("DestructibleWall"))
        {
            Destroy(collision.gameObject);
        }
    }
    IEnumerator Delay(float lifetime)
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }
    
    private float CalculateDamage(int hitCount)
    {
        if (hitCount >= 4)
        {
            Destroy(gameObject);
            return 0;
        }
        switch (hitCount)
        {
            case 0: return 100f;
            case 1: return 90f;
            case 2: return 60f;
            case 3: return 30f;
            default: return 0f;
        }
    }
    private void InstantiateExplodeEffectAtCollisionPoint(Collider2D collider)
    {
        var contactPoint = collider.ClosestPoint(shootPoint.position);
        // if 1:
        // if 2:
        if (contactPoint!= null && Vector2.Distance(contactPoint,shootPoint.position) > 1f )
        {
            GameObject Temp = Instantiate(explodeEffect, contactPoint, Quaternion.identity);
            Destroy(Temp, 5.0f);
        }    
    }
}
