using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCVisibilityController : MonoBehaviour
{
    public Renderer npcRenderer;
    public Animator npcAnimator;
    private GameObject player;
    

    private void Start()
    {
        npcRenderer.enabled = false;
        player = GameObject.FindGameObjectWithTag("Player");
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            npcAnimator.SetBool("isAppearing", true);
            npcRenderer.enabled = true;
        }
    }

    public void DestroyNPC()
    {
        Destroy(gameObject); 
    }

}
