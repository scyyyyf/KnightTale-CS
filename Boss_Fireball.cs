using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Fireball : MonoBehaviour
{
    public float speed = 10f;          
    public GameObject explosionEffect; 
    public LayerMask groundLayer;      
    public LayerMask playerLayer;      
    public float explosionRadius = 5f; 
    public int damage = 15; 
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.down * speed; 
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (((1 << hitInfo.gameObject.layer) & groundLayer) != 0) 
        {
            Explode();
            Destroy(gameObject); 
        }
    }

    void Explode()
    {
        Instantiate(explosionEffect, transform.position, Quaternion.identity);
        AudioManager.Instance.PlaySFX("FireBallExplode");
        Collider2D[] players = Physics2D.OverlapCircleAll(transform.position, explosionRadius, playerLayer);
        foreach (Collider2D player in players)
        {
            if (player.GetComponent<PlayerMovement_Kdx>()) 
            {
                player.GetComponent<PlayerMovement_Kdx>().TakeDamage(damage);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
