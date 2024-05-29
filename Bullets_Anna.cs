using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullets_Anna : MonoBehaviour
{
    public float speed = 10f; 
    public float damage = 5f;
    public GameObject explosionPrefab;

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        EnemyHealth enemy = hitInfo.GetComponent<EnemyHealth>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            Destroy(gameObject); 
        }
    }
}
