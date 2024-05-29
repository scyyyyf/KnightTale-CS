using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static System.TimeZoneInfo;

public class CaveTrigger : MonoBehaviour
{
    GameManager gameManager;
    public LayerMask playerMask;
    public Animator transitionLv1;
    public float transitionTime = 1f;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & playerMask) != 0)
        {
            gameManager.SaveData();
            gameManager.LoadNextLevel();
            AudioManager.Instance.ChangeMusic("BGM-secondstage");
            AudioManager.Instance.StopRunSound();
            LoadNextLevel();
        }
    }

    public void LoadNextLevel()
    {
        transitionLv1.SetTrigger("End");
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(levelIndex);
    }
}
