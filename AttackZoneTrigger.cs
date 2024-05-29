using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackZoneTrigger : MonoBehaviour
{
    public int AtkValue = 10;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerMovement_Kdx playerScript = collision.GetComponent<PlayerMovement_Kdx>();
            if (playerScript != null)
            {
                playerScript.TakeDamage(AtkValue);
            }
        }
    }
}
