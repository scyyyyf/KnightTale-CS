using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public Image portraitImage;
    public Button continueButton;
    public Animator animator;
    [SerializeField] private Queue<string> sentences;
    [SerializeField] private GameObject currentNPC;
    [SerializeField] private Animator npcAnimator;
    private void Start()
    {
        sentences = new Queue<string>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            continueButton.onClick.Invoke();
        }
    }

    public void StartDialogue(Dialogue dialogue,GameObject npc)
    {
        AudioManager.Instance.StopRunSound();
        AudioManager.Instance.PlaySFX("DialoguePop");
        currentNPC = npc;
        npcAnimator = npc.GetComponent<Animator>();
        animator.SetBool("conversatonIsOpen", true);
        nameText.text = dialogue.name;
        portraitImage.sprite = dialogue.portrait;
        portraitImage.gameObject.SetActive(true);
        sentences.Clear();
        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }
        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }
        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentences(sentence));
    }

    IEnumerator TypeSentences(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }

    void EndDialogue()
    {
        animator.SetBool("conversatonIsOpen", false);
        var playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement_Kdx>();
        if (playerController != null)
        {
            playerController.notMove = false;
            playerController.notJump = false;
        }
        if (currentNPC != null)
        {
            npcAnimator.SetBool("isDisappearing", true);
            npcAnimator.SetBool("isIdle", false);
        }
    }
}
