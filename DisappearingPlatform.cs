using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearingPlatform : MonoBehaviour
{
    public float timeToDisappear = 1f;
    public float timeToRespawn = 3f;
    public int numberOfBlinks = 5;      

    private SpriteRenderer spriteRenderer;
    private Collider2D platformCollider;
    private bool isPlatformActive = true;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        platformCollider = GetComponent<Collider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && isPlatformActive)
        {
            StartCoroutine(HandleDisappearingPlatform());
        }
    }

    private IEnumerator HandleDisappearingPlatform()
    {
        isPlatformActive = false;
        float blinkInterval = timeToDisappear / (numberOfBlinks * 2);  
        for (int i = 0; i < numberOfBlinks; i++)
        {
            spriteRenderer.enabled = false;
            yield return new WaitForSeconds(blinkInterval);
            spriteRenderer.enabled = true;
            yield return new WaitForSeconds(blinkInterval);
            blinkInterval *= 0.8f;  
        }
        spriteRenderer.enabled = false;  
        platformCollider.enabled = false;

        yield return new WaitForSeconds(timeToRespawn);

        spriteRenderer.enabled = true;
        platformCollider.enabled = true;
        isPlatformActive = true;
    }
}
