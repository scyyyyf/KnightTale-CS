using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotion : MonoBehaviour
{
    
    public int healAmount = 10; 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) 
        {
            PlayerMovement_Kdx player = collision.GetComponent<PlayerMovement_Kdx>();
            if (player != null)
            {
                AudioManager.Instance.PlaySFX("Heal");
                player.Heal(healAmount); 
                Destroy(gameObject); 
            }
        }
    }
}

