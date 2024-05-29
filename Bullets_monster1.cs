using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullets_monster1 : MonoBehaviour
{
    public float damage = 10f; 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) 
        {
            PlayerMovement_Kdx player = collision.gameObject.GetComponent<PlayerMovement_Kdx>();
            if (player != null)
            {
                player.TakeDamage((int)damage); 
            }
            Destroy(gameObject); 
        }
    }
}
