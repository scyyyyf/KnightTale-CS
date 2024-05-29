using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class BossTrigger : MonoBehaviour
{
    public GameObject boss; 
    public Animator bossAnimator;
    private CinemachineConfiner cameraConfiner;
    public Collider2D bossAreaBounds;  
    public Collider2D defaultCameraBounds;
    private Boss_wolfAttack bossAttackScript;
    private bool hasBossBeenActivated = false;
    private void Awake()
    {
        cameraConfiner = FindObjectOfType<CinemachineConfiner>();
        if (boss != null)
        {
            bossAttackScript = boss.GetComponent<Boss_wolfAttack>();  
        }
    }
    private void Start()
    {
        if (boss != null)
            boss.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasBossBeenActivated)
        {
            ActivateBoss();
        }
    }
    private void ActivateBoss()
    {
        if (boss != null)
        {
            boss.SetActive(true);
            hasBossBeenActivated = true;
            AudioManager.Instance.ChangeMusic("BGM-Boss");
            if (cameraConfiner != null && bossAreaBounds != null)
            {
                cameraConfiner.m_BoundingShape2D = bossAreaBounds;  
                cameraConfiner.InvalidatePathCache();
            }
        }
    }
    public void OnBossDeath()
    {
        if (cameraConfiner != null && defaultCameraBounds != null)
        {
            cameraConfiner.m_BoundingShape2D = defaultCameraBounds;  
            cameraConfiner.InvalidatePathCache();
        }
    }
    public void BossDeadSound()
    {
        AudioManager.Instance.PlaySFX("BossDie");
    }
}
