using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    public int damage = 15; 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            PlayerMovement_Kdx player = collision.GetComponent<PlayerMovement_Kdx>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }
        }
    }
}
