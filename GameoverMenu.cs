using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameoverMenu : MonoBehaviour
{
    public GameObject gameOverMenuUI;
    private bool isGameOverScreenShown = false;

    void Start()
    {
        gameOverMenuUI.SetActive(false);
    }

    public void ShowGameOverScreen()
    {
        if (!isGameOverScreenShown)  // 检查游戏结束界面是否已经被显示
        {
            isGameOverScreenShown = true;  // 标记为已显示
            gameOverMenuUI.SetActive(true);
            AudioManager.Instance.PauseMusic();
            AudioManager.Instance.PlaySFX("GameOver");
            AudioManager.Instance.StopRunSound();
            AudioManager.Instance.StopBossEnragedRunSound();
            Time.timeScale = 1f;
            PlayerMovement_Kdx playerController = FindObjectOfType<PlayerMovement_Kdx>();
            if (playerController != null)
            {
                playerController.enabled = false;
            }
        }
    }

    public void LoadMenu()
    {
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
