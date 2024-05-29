using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreResultMenu : MonoBehaviour
{
    public GameObject ScoreResultMenuUI;
    private bool isScoreResultMenuShown = false;

    void Start()
    {
        ScoreResultMenuUI.SetActive(false);
    }

    public void ShowGameOverScreen()
    {
        if (!isScoreResultMenuShown)  // �����Ϸ���������Ƿ��Ѿ�����ʾ
        {
            isScoreResultMenuShown = true;  // ���Ϊ����ʾ
            ScoreResultMenuUI.SetActive(true);
            AudioManager.Instance.PauseMusic();
            AudioManager.Instance.PlaySFX("GameOver");
            AudioManager.Instance.StopRunSound();
            AudioManager.Instance.StopBossEnragedRunSound();
            Time.timeScale = 0f;
            PlayerMovement_Kdx playerController = FindObjectOfType<PlayerMovement_Kdx>();
            if (playerController != null)
            {
                playerController.enabled = false;
            }
        }
    }

    public void LoadMenu()
    {
        AudioManager.Instance.StopBossRunSound();
        AudioManager.Instance.StopBossEnragedRunSound();
        AudioManager.Instance.StopRunSound();
        AudioManager.Instance.ResumeMusic();
        AudioManager.Instance.ChangeMusic("BGM");
        Time.timeScale = 1f;
        SceneManager.LoadScene("Start Screen");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
