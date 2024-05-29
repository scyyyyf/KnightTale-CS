using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrriger : MonoBehaviour
{
    public Dialogue dialogue;
    private DialogueManager dialogueManager;

    private void Start()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) 
        {
            var playerController = other.GetComponent<PlayerMovement_Kdx>();
            playerController.notMove = true;
            playerController.notJump = true;
            playerController.SetWalkingSpeed(0f);
            playerController.SetNotJump(false);
            dialogueManager.StartDialogue(dialogue, this.gameObject);
        }
    }

    public void DestroyNPC()
    {
        Destroy(gameObject);
    }

    public void DestroyPrincess1()
    {
        PlayerMovement_Kdx playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement_Kdx>();
        Destroy(gameObject);
        if (playerScript.TransformedPrincess_1 != null)
        {
            playerScript.TransformedPrincess_1.SetActive(true);
            AudioManager.Instance.PlaySFX("TransformFairy");
        }
    }

    public void DestroyPrincess2()
    {
        PlayerMovement_Kdx playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement_Kdx>();
        Destroy(gameObject);
        if (playerScript.TransformedPrincess_2 != null)
        {
            playerScript.TransformedPrincess_2.SetActive(true);
            AudioManager.Instance.PlaySFX("TransformFairy");
        }
    }
    public void UnlockChargeAttack()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            PlayerAttack playerAttack = player.GetComponent<PlayerAttack>();
            if (playerAttack != null)
            {
                playerAttack.isChargeAttackUnlocked = true;
                Debug.Log("Charge Attack Unlocked");
            }
        }
    }
    public void GuideDisapVioce()
    {
        AudioManager.Instance.PlaySFX("GuideDisapp");
    }
    public void TransformVioce()
    {
        AudioManager.Instance.PlaySFX("Transform");
    }
}
