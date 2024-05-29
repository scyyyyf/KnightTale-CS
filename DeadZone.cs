using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone : MonoBehaviour
{
    public static PlayerMovement_Kdx currentPlayerMovement;
    private void Start()
    {
        currentPlayerMovement = FindObjectOfType<PlayerMovement_Kdx>();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            currentPlayerMovement = other.GetComponent<PlayerMovement_Kdx>();

            if (currentPlayerMovement != null && !currentPlayerMovement.isDead)
            {
                currentPlayerMovement.TakeDamage(20);
                currentPlayerMovement.RespawnAtNearestPoint();
            }
        }
    }
}
