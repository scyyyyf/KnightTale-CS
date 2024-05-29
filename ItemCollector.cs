using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemCollector : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("AtkPowerPotion"))
        {
            //AudioManager.Instance.PlaySFX("EatPotion");
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("AtkSpeedPotion"))
        {
            //AudioManager.Instance.PlaySFX("EatPotion");
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("AtkDistancePotion"))
        {
            //AudioManager.Instance.PlaySFX("EatPotion");
            Destroy(collision.gameObject);
        }
    }
}
