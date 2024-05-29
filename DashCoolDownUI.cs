using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashCoolDownUI : MonoBehaviour
{
    public Image cooldownImage;
    public PlayerMovement_Kdx playerMovementScript;

    private void OnEnable()
    {
        playerMovementScript.onDash += HandleDash;
    }

    private void OnDisable()
    {
        playerMovementScript.onDash -= HandleDash;
    }

    private void HandleDash()
    {
        StartCoroutine(Cooldown());
    }

    private IEnumerator Cooldown()
    {
        float cooldown = 1f;  // 这里的冷却时间应该与PlayerMovement_Kdx中的dashingCooldown保持一致
        float elapsed = 0f;

        while (elapsed < cooldown)
        {
            elapsed += Time.deltaTime;
            cooldownImage.fillAmount = 1 - elapsed / cooldown;
            yield return null;
        }

        cooldownImage.fillAmount = 0;
    }
}
